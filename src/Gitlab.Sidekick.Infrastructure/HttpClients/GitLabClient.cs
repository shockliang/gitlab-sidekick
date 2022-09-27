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

        return  await _client.GetAsync<Group>(request);
    }

    private static RestRequest CreateRequestWithToken(string resource, string token, Method method)
    {
        var request = new RestRequest(resource, method) { RequestFormat = DataFormat.Json };
        request
            .AddHeader("PRIVATE-TOKEN", token);

        return request;
    }
}
