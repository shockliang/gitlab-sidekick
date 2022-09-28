using System.Text.Json;
using Gitlab.Sidekick.Application.Interfaces.HttpClients;
using Gitlab.Sidekick.Application.Models.Groups;
using RestSharp;

namespace Gitlab.Sidekick.Infrastructure.HttpClients;

public class GitLabClient : IGitLabClient
{
    private readonly RestClient _client;
    private readonly string _token;

    public GitLabClient(RestClient client, string token)
    {
        _client = client;
        _token = token;
    }

    public async Task<Group> GetGroup(long groupId)
    {
        var request = CreateRequestWithToken($"groups/{groupId}", _token, Method.Get);
        var response = await _client.GetAsync(request);
        return  await _client.GetAsync<Group>(request);
    }

    public async Task<ICollection<Group>> SearchGroups(string name)
    {
        var request = CreateRequestWithToken("groups", _token, Method.Get)
            .AddParameter("search", name);

        var response = await _client.GetAsync(request);

        if (response.IsSuccessful)
            return JsonSerializer.Deserialize<List<Group>>(response.Content);

        return ArraySegment<Group>.Empty;
    }

    private static RestRequest CreateRequestWithToken(string resource, string token, Method method)
    {
        var request = new RestRequest(resource, method) { RequestFormat = DataFormat.Json };
        request
            .AddHeader("PRIVATE-TOKEN", token);

        return request;
    }
}
