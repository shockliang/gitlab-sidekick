using System.Text.Json;
using Gitlab.Sidekick.Application.Interfaces.HttpClients;
using Gitlab.Sidekick.Application.Models.Enumerations;
using Gitlab.Sidekick.Application.Models.Groups;
using Gitlab.Sidekick.Application.Models.Projects;
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

    public async Task<Group> CreateSubGroup(
        string name,
        string path,
        int parentId,
        string visibility = Visibility.Private)
    {
        var request = CreateRequestWithToken("groups", _token, Method.Post)
            .AddParameter("name", name)
            .AddParameter("path", path)
            .AddParameter("visibility", visibility)
            .AddParameter("parent_id", parentId);

        var response = await _client.PostAsync(request);

        return response.IsSuccessful ? JsonSerializer.Deserialize<Group>(response.Content) : null;
    }

    public async Task<Project> CreateProject(
        int namespaceId,
        string name,
        string path,
        string defaultBranch,
        string description,
        IEnumerable<string> tags,
        byte[] avatarBytes,
        string visibility = Visibility.Private)
    {
        var tagList = tags.Any()
            ? string.Join(", ", tags)
            : "";

        var request = CreateRequestWithToken("projects", _token, Method.Post)
            .AddParameter("namespace_id", namespaceId)
            .AddParameter("name", name)
            .AddParameter("path", path)
            .AddParameter("default_branch", defaultBranch)
            .AddParameter("description", description)
            .AddParameter("visibility", visibility)
            .AddParameter("tag_list", tagList);

        if (avatarBytes.Any())
        {
            request
                .AddHeader("Content-Type", "multipart/form-data")
                .AddFile("avatar", avatarBytes, $"{name}-avatar.jpeg");
        }

        var response = await _client.PostAsync(request);
        return response.IsSuccessful ? JsonSerializer.Deserialize<Project>(response.Content) : null;
    }

    private static RestRequest CreateRequestWithToken(string resource, string token, Method method)
    {
        var request = new RestRequest(resource, method) { RequestFormat = DataFormat.Json };
        request
            .AddHeader("PRIVATE-TOKEN", token);

        return request;
    }
}
