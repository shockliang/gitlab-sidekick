namespace Gitlab.Sidekick.Application.Interfaces.Services;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
}
