using Gitlab.Sidekick.Application.Models;
using MediatR;

namespace Gitlab.Sidekick.Application.Endpoints.People.Queries;

public class PeopleQuery : IRequest<EndpointResult<IEnumerable<PersonViewModel>>>
{
    public bool IncludeInactive { get; init; } = false;
}

