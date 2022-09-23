using Gitlab.Sidekick.Domain.Entities;

namespace Gitlab.Sidekick.Application.Interfaces.Persistence.DataServices.People.Commands;

public interface IAddPersonDataService
{
    Task<Person> ExecuteAsync(Person person, CancellationToken cancellationToken = default);
}

