using Gitlab.Sidekick.Application.Models;
using MediatR;

namespace Gitlab.Sidekick.Application.Endpoints.People.Commands;

public class AddPersonCommand : IRequest<EndpointResult<PersonViewModel>>
{
    public int? Id { get; init; }
    public string FirstName { get; init; } = "";
    public string LastName { get; init; } = "";
}
