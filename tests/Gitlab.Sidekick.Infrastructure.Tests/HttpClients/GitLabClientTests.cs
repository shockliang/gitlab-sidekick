using System.Text.Json;
using FluentAssertions;
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

        var mockHttp = new MockHttpMessageHandler();

        // Setup a respond for the user api (including a wildcard in the URL)
        mockHttp.When($"{_host}/api/v4/groups/{_groupId}")
            .Respond("application/json", groupJson);

        mockHttp.When($"{_host}/api/v4/groups")
            .WithQueryString("search", _searchName)
            .Respond("application/json", groupsJson);

        // Instantiate the client normally, but replace the message handler
        var hostUri = new Uri(_host);
        var apiUri = new Uri(hostUri, "api/v4");

        var options = new RestClientOptions(apiUri) { ConfigureMessageHandler = _ => mockHttp };
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
}
