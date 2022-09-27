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
    private readonly GitLabClient _target;

    public GitLabClientTests()
    {
        var groupStub = new Group { Id = 1, Description = "Group stub"};
        var groupJson = JsonSerializer.Serialize(groupStub);
        var mockHttp = new MockHttpMessageHandler();

        // Setup a respond for the user api (including a wildcard in the URL)
        mockHttp.When($"{_host}/api/v4/groups/*")
            .Respond("application/json", groupJson);

        // Instantiate the client normally, but replace the message handler
        var hostUri = new Uri(_host);
        var apiUri = new Uri(hostUri, "api/v4");

        var options = new RestClientOptions(apiUri) { ConfigureMessageHandler = _ => mockHttp };
        var client = new RestClient(options);
        _target = new GitLabClient(client, _token);
    }

    [Fact]
    public async Task GetGroup_ShouldAsExpected()
    {
        // Arrange

        // Act
        var actual = await _target.GetGroup(1);

        // Assert
        actual.Should().NotBeNull();
        actual.Id.Should().Be(1);
    }
}
