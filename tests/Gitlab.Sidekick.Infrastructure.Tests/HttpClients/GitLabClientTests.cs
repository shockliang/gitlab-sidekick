using System.Text.Json;
using FluentAssertions;
using Gitlab.Sidekick.Application.Models.Enumerations;
using Gitlab.Sidekick.Application.Models.Groups;
using Gitlab.Sidekick.Infrastructure.HttpClients;
using RestSharp;
using RichardSzalay.MockHttp;
using Xunit;

namespace Gitlab.Sidekick.Infrastructure.Tests.HttpClients;

public class GitLabClientTests
{
    private readonly string _host = "http://localhost";
    private readonly string _token = "some token";
    private readonly long _groupId = 1L;
    private readonly string _searchName = "my_group";
    private readonly IList<Group> _groupStubs;
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly Uri _apiUri;
    private readonly GitLabClient _target;

    public GitLabClientTests()
    {
        var groupStub = new Group { Id = 1, Description = "Group stub"};
        var groupJson = JsonSerializer.Serialize(groupStub);
        _groupStubs = new List<Group>
        {
            new() { Id = 1, Name = $"{_searchName}_1", Description = "Group stub" },
            new() { Id = 1, Name = $"{_searchName}_2", Description = "Group stub" }
        };
        var groupsJson = JsonSerializer.Serialize(_groupStubs);

        _mockHttp = new MockHttpMessageHandler();

        // Setup a respond for the user api (including a wildcard in the URL)
        _mockHttp.When(HttpMethod.Get, $"{_host}/api/v4/groups/{_groupId}")
            .Respond("application/json", groupJson);

        _mockHttp.When(HttpMethod.Get, $"{_host}/api/v4/groups")
            .WithQueryString("search", _searchName)
            .Respond("application/json", groupsJson);

        // Instantiate the client normally, but replace the message handler
        var hostUri = new Uri(_host);
        _apiUri = new Uri(hostUri, "api/v4");

        var options = new RestClientOptions(_apiUri) { ConfigureMessageHandler = _ => _mockHttp };
        var client = new RestClient(options);
        _target = new GitLabClient(client, _token);
    }

    #region Get group tests

    [Fact]
    public async Task GetGroup_ShouldAsExpected()
    {
        // Arrange

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

        _mockHttp.When(HttpMethod.Post, $"{_host}/api/v4/groups")
            .WithHeaders("PRIVATE-TOKEN", _token)
            .WithFormData("name", expectedName)
            .WithFormData("path", expectedPath)
            .WithFormData("visibility", expectedVisibility)
            .WithFormData("parent_id", expectedParentId.ToString())
            .Respond("application/json", createdGroupJson);

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

        _mockHttp.When(HttpMethod.Post, $"{_host}/api/v4/projects")
            .WithHeaders("PRIVATE-TOKEN", _token)
            .WithFormData("namespace_id", expectedNamespaceId.ToString())
            .WithFormData("name", expectedName)
            .WithFormData("path", expectedPath)
            .WithFormData("default_branch", expectedDefaultBranch)
            .WithFormData("description", expectedDescription)
            .WithFormData("visibility", expectedVisibility)
            .WithFormData("tag_list", string.Join(", ",expectedTags))
            .Respond("application/json", createdGroupJson);

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

    #endregion
}
