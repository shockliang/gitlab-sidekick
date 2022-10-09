using Gitlab.Sidekick.Application.Interfaces.HttpClients;
using Gitlab.Sidekick.Application.Models.Groups;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gitlab.Sidekick.Application.Core.Queries;

public class GroupQueryHandler: IRequestHandler<GroupQuery, Group>
{
    private readonly ILogger<GroupQueryHandler> _logger;
    private readonly IGitLabClient _gitLabClient;

    public GroupQueryHandler(ILogger<GroupQueryHandler> logger, IGitLabClient gitLabClient)
    {
        _logger = logger;
        _gitLabClient = gitLabClient;
    }

    public async Task<Group> Handle(GroupQuery request, CancellationToken cancellationToken)
    {
        var group = await _gitLabClient.GetGroup(request.GroupId);

        return group;
    }
}
