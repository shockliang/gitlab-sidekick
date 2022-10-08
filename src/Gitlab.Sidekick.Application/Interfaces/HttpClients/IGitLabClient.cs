using Gitlab.Sidekick.Application.Models;
using Gitlab.Sidekick.Application.Models.Enumerations;
using Gitlab.Sidekick.Application.Models.Groups;
using Gitlab.Sidekick.Application.Models.Projects;

namespace Gitlab.Sidekick.Application.Interfaces.HttpClients;

public interface IGitLabClient
{
    Task<Group> GetGroup(long groupId);

    Task<Pagination> GetProjectsPagination(long groupId, int perPageCount);

    Task<Pagination<List<Project>>> ListProjectsByPage(long groupId, int page = 1, int perPageCount = 100);

    Task<ICollection<Group>> SearchGroups(string name);

    Task<Group> CreateSubGroup(
        string name,
        string path,
        int parentId,
        string visibility = Visibility.Private);

    Task<Project> CreateProject(
        int namespaceId,
        string name,
        string path,
        string defaultBranch,
        string description,
        IEnumerable<string> tags,
        byte[] avatarBytes,
        string visibility = Visibility.Private);

    Task<Project> UpdateProjectDefaultBranchAndTags(
        long projectId,
        string defaultBranch,
        IEnumerable<string> tags = null);

    Task<Slack> CreateSlackService(
        long projectId,
        string webhook,
        string username);
}
