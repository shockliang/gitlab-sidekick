using System.Diagnostics.CodeAnalysis;
using Gitlab.Sidekick.Api.Extensions;
using Gitlab.Sidekick.Application.Endpoints.People.Commands;
using Gitlab.Sidekick.Application.Endpoints.People.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gitlab.Sidekick.Api.Controllers;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/v{version:apiVersion}/people")]
[ApiVersion("1.0")]
public class PeopleController : ControllerBase
{
    private readonly IMediator _mediator;

    public PeopleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetPeopleAsync([FromQuery] PeopleQuery request) =>
        (await _mediator.Send(request)).ToActionResult();

    [HttpPost]
    public async Task<ActionResult> AddPersonAsync([FromBody] AddPersonCommand command) =>
        (await _mediator.Send(command)).ToActionResult();
}
