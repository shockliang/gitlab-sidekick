using Gitlab.Sidekick.Application.Models.Groups;
using MediatR;

namespace Gitlab.Sidekick.Application.Core.Queries;

public class GroupQuery : IRequest<Group>
{
    public long GroupId { get; set; }
}
