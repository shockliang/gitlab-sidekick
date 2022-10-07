using System.Text.Json;
using Gitlab.Sidekick.Application.Interfaces.HttpClients;
using Gitlab.Sidekick.Application.Models;
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
        var request = CreateRequestWithToken($"/groups/{groupId}", _token, Method.Get);
        var response = await _client.GetAsync(request);
        return response.IsSuccessful ? Group.FromJson(response.Content) : null;
    }

    public async Task<Pagination> GetProjectsPagination(long groupId, int perPageCount)
    {
        var request = CreateRequestWithToken($"/groups/{groupId}/projects", _token, Method.Get);
        request
            .AddParameter("per_page", perPageCount);

        var response = await _client.ExecuteGetAsync(request);
        return new Pagination(response.Headers);
    }

    public async Task<Pagination<List<Project>>> ListProjectsByPage(long groupId, int page = 1, int perPageCount = 100)
    {

        var request = CreateRequestWithToken($"/groups/{groupId}/projects", _token, Method.Get);
        request
            .AddParameter("page", page)
            .AddParameter("per_page", perPageCount);

        var response = await _client.ExecuteGetAsync(request);
        var data = Project.FromJsonToList(response.Content);
        var result = new Pagination<List<Project>>(response.Headers, data);

        return result;
    }

    public async Task<ICollection<Group>> SearchGroups(string name)
    {
        var request = CreateRequestWithToken("/groups", _token, Method.Get)
            .AddParameter("search", name);

        var response = await _client.GetAsync(request);

        return response.IsSuccessful
            ? JsonSerializer.Deserialize<List<Group>>(response.Content)
            : ArraySegment<Group>.Empty;
    }

    public async Task<Group> CreateSubGroup(
        string name,
        string path,
        int parentId,
        string visibility = Visibility.Private)
    {
        var request = CreateRequestWithToken("/groups", _token, Method.Post)
            .AddQueryParameter("name", name)
            .AddQueryParameter("path", path)
            .AddQueryParameter("visibility", visibility)
            .AddQueryParameter("parent_id", parentId);

        var response = await _client.PostAsync(request);

        return response.IsSuccessful ? Group.FromJson(response.Content) : null;
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

        var request = CreateRequestWithToken("/projects", _token, Method.Post)
            .AddQueryParameter("namespace_id", namespaceId)
            .AddQueryParameter("name", name)
            .AddQueryParameter("path", path)
            .AddQueryParameter("default_branch", defaultBranch)
            .AddQueryParameter("description", description)
            .AddQueryParameter("visibility", visibility)
            .AddQueryParameter("tag_list", tagList);

        if (avatarBytes.Any())
        {
            request
                .AddHeader("Content-Type", "multipart/form-data")
                .AddFile("avatar", avatarBytes, $"{name}-avatar.jpeg");
        }

        var response = await _client.PostAsync(request);
        return response.IsSuccessful ? JsonSerializer.Deserialize<Project>(response.Content) : null;
    }

    public async Task<Project> UpdateProjectDefaultBranchAndTags(
        long projectId,
        string defaultBranch,
        IEnumerable<string> tags = null)
    {

        var request = CreateRequestWithToken($"/projects/{projectId}", _token, Method.Put);
        var tagList = tags != null ? string.Join(", ", tags) : "";

        request
            .AddQueryParameter("default_branch", defaultBranch);

        if (tags != null && tags.Any())
            request.AddQueryParameter("tag_list", tagList);

        var response = await _client.PutAsync(request);
        return response.IsSuccessful ? Project.FromJson(response.Content) : null;
    }

    private static RestRequest CreateRequestWithToken(string resource, string token, Method method)
    {
        var request = new RestRequest(resource, method) { RequestFormat = DataFormat.Json };
        request
            .AddHeader("PRIVATE-TOKEN", token);

        return request;
    }
}
