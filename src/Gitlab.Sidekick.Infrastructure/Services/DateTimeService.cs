using System.Diagnostics.CodeAnalysis;
using Gitlab.Sidekick.Application.Interfaces.Services;

namespace Gitlab.Sidekick.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
