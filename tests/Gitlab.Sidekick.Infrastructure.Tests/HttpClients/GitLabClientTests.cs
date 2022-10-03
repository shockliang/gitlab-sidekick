using System.Net;
using System.Text.Json;
using FluentAssertions;
using Gitlab.Sidekick.Application.Models.Enumerations;
using Gitlab.Sidekick.Application.Models.Groups;
using Gitlab.Sidekick.Infrastructure.HttpClients;
using RestSharp;
using RichardSzalay.MockHttp;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace Gitlab.Sidekick.Infrastructure.Tests.HttpClients;

public class GitLabClientTests : IClassFixture<HttpMockFixture>
{
    private readonly HttpMockFixture _mockFixture;
    private readonly WireMockServer _mockServer;
    private readonly string _host = "http://localhost";
    private readonly string _token = "some token";
    private readonly long _groupId = 1L;
    private readonly string _searchName = "my_group";
    private readonly string _groupJson;
    private readonly string _groupsJson;
    private readonly string _apiVersionUrl = "/api/v4";
    private readonly Dictionary<string, string> _headersStub;
    private readonly IList<Group> _groupStubs;
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly Uri _apiUri;
    private readonly GitLabClient _target;


    public GitLabClientTests(HttpMockFixture mockFixture)
    {
        _mockFixture = mockFixture;
        _mockServer = _mockFixture.MockServer;


        var groupStub = new Group { Id = 1, Description = "Group stub"};
        _groupJson = JsonSerializer.Serialize(groupStub);
        _groupStubs = new List<Group>
        {
            new() { Id = 1, Name = $"{_searchName}_1", Description = "Group stub" },
            new() { Id = 1, Name = $"{_searchName}_2", Description = "Group stub" }
        };

        _groupsJson = JsonSerializer.Serialize(_groupStubs);

        _headersStub = new Dictionary<string, string>
        {
            ["Content-Type"] = "application/json; charset=UTF-8"
        };

        var hostUri = new Uri(_mockServer.Url);
        _apiUri = new Uri(hostUri, _apiVersionUrl);

        var options = new RestClientOptions(_apiUri);
        var client = new RestClient(options);
        _target = new GitLabClient(client, _token);
    }

    #region Get group tests

    [Fact]
    public async Task GetGroup_ShouldAsExpected()
    {
        // Arrange
        _mockServer
            .Given(Request.Create()
                .WithPath($"{_apiVersionUrl}/groups/{_groupId}")
                .WithHeader("PRIVATE-TOKEN", _token)
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeaders(_headersStub)
                .WithBody(_groupJson));

        // Act
        var actual = await _target.GetGroup(_groupId);

        // Assert
        actual.Should().NotBeNull();
        actual.Id.Should().Be(1);
    }

    [Fact]
    public async Task ItShouldNotFound_WhenGetNotExistGroupId()
    {
        // Arrange
        _mockServer
            .Given(Request.Create()
                .WithPath($"{_apiVersionUrl}/groups/{1024}")
                .WithHeader("PRIVATE-TOKEN", _token)
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.NotFound)
                .WithHeaders(_headersStub));

        // Act
        var actual = await _target.GetGroup(1024);

        // Assert
        actual.Should().BeNull();
    }

    #endregion

    #region Search groups tests

    [Fact]
    public async Task ItShouldGetGroupList_WhenGetExistGroupName()
    {
        // Arrange
        _mockServer
            .Given(Request.Create()
                .WithPath($"{_apiVersionUrl}/groups")
                .WithHeader("PRIVATE-TOKEN", _token)
                .WithParam("search", _searchName)
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeaders(_headersStub)
                .WithBody(_groupsJson));
        // Act
        var actual = await _target.SearchGroups(_searchName);

        // Assert
        actual.Should().NotBeEmpty();
        actual.Count.Should().Be(_groupStubs.Count);
    }

    [Fact]
    public async Task ItShouldGet_EmptyCollection_WhenGetNotExistGroupName()
    {
        // Arrange
        _mockServer
            .Given(Request.Create()
                .WithPath($"{_apiVersionUrl}/groups")
                .WithHeader("PRIVATE-TOKEN", _token)
                .WithParam("search", _searchName)
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.NotFound));
        // Act
        var actual = await _target.SearchGroups("not_exist_group");

        // Assert
        actual.Should().BeEmpty();
    }

    #endregion

    #region Create sub group tests

    [Fact]
    public async Task ItShouldGetGroup_WhenCreateSuccess()
    {
        // Arrange
        var expectedName = "my-sub-group";
        var expectedPath = "my-sub-group";
        var expectedVisibility = Visibility.Private;
        var expectedParentId = 1;
        var createdGroup = new Group { Id = 2, ParentId = expectedParentId };
        var createdGroupJson = JsonSerializer.Serialize(createdGroup);

        _mockServer
            .Given(Request.Create()
                .WithPath($"{_apiVersionUrl}/groups")
                .WithHeader("PRIVATE-TOKEN", _token)
                .WithParam("name", expectedName)
                .WithParam("path", expectedPath)
                .WithParam("visibility", expectedVisibility)
                .WithParam("parent_id", expectedParentId.ToString())
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeaders(_headersStub)
                .WithBody(createdGroupJson));


        // Act
        var actual = await _target.CreateSubGroup(
            expectedName, expectedPath, expectedParentId, expectedVisibility);

        // Assert
        actual.Should().NotBeNull();
        actual.Id.Should().Be(createdGroup.Id);
        actual.ParentId.Should().Be(expectedParentId);
    }

    #endregion

    #region Create project tests

    [Fact]
    public async Task CreateProject_ShouldAsExpected()
    {
        // Arrange
        var expectedNamespaceId = 1;
        var expectedName = "my-sub-group";
        var expectedPath = "my-sub-group";
        var expectedVisibility = Visibility.Private;
        var expectedDefaultBranch = "dev";
        var expectedParentId = 1;
        var expectedDescription = "some description";
        var expectedTags = new List<string> { "tag1,", "tag2" };
        var createdGroup = new Group { Id = 2, ParentId = expectedParentId };
        var createdGroupJson = JsonSerializer.Serialize(createdGroup);

        _mockServer
            .Given(Request.Create()
                .WithPath($"{_apiVersionUrl}/projects")
                .WithHeader("PRIVATE-TOKEN", _token)
                .WithParam("namespace_id", expectedNamespaceId.ToString())
                .WithParam("name", expectedName)
                .WithParam("path", expectedPath)
                .WithParam("default_branch", expectedDefaultBranch)
                .WithParam("description", expectedDescription)
                .WithParam("visibility", expectedVisibility)
                .WithParam("tag_list", string.Join(", ",expectedTags))
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeaders(_headersStub)
                .WithBody(createdGroupJson));

        // Act
        var actual = await _target.CreateProject(
            expectedNamespaceId,
            expectedName,
            expectedPath,
            expectedDefaultBranch,
            expectedDescription,
            expectedTags,
            Array.Empty<byte>(),
            expectedVisibility);

        // Assert
        actual.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateProject_WithAvatarByteArray_ShouldSendWith_ContentType_Multipart_FormData_InHeader()
    {
        // Arrange
        var expectedNamespaceId = 1;
        var expectedName = "my-sub-group";
        var expectedPath = "my-sub-group";
        var expectedVisibility = Visibility.Private;
        var expectedDefaultBranch = "dev";
        var expectedParentId = 1;
        var expectedDescription = "some description";
        var expectedTags = new List<string> { "tag1", "tag2" };
        var createdGroup = new Group { Id = 2, ParentId = expectedParentId };
        var createdGroupJson = JsonSerializer.Serialize(createdGroup);

        _mockServer
            .Given(Request.Create()
                .WithPath($"{_apiVersionUrl}/projects")
                .WithHeader("PRIVATE-TOKEN", _token)
                .WithHeader("Content-Type", new ContentTypeMatcher("multipart/form-data"))
                .WithParam("namespace_id", expectedNamespaceId.ToString())
                .WithParam("name", expectedName)
                .WithParam("path", expectedPath)
                .WithParam("default_branch", expectedDefaultBranch)
                .WithParam("description", expectedDescription)
                .WithParam("visibility", expectedVisibility)
                .WithParam("tag_list", string.Join(", ",expectedTags))
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeaders(_headersStub)
                .WithBody(createdGroupJson));

        // Act
        var actual = await _target.CreateProject(
            expectedNamespaceId,
            expectedName,
            expectedPath,
            expectedDefaultBranch,
            expectedDescription,
            expectedTags,
            new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
            expectedVisibility);

        // Assert
        actual.Should().NotBeNull();
    }

    #endregion
}
