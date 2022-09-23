namespace Gitlab.Sidekick.Application.Models.Enumerations;

public enum EndpointResultStatus
{
    Success,
    NotFound,
    Invalid,
    Duplicate,
    Unauthorized,
    Gone,
    Error
}
