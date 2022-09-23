using Gitlab.Sidekick.Application.Interfaces.Persistence.DataServices.People.Queries;
using Gitlab.Sidekick.Domain.Entities;

namespace Gitlab.Sidekick.Infrastructure.Persistence.DataServices.People.Queries;

public class GetPeopleDataService : IGetPeopleDataService
{

    public async Task<IEnumerable<Person>> ExecuteAsync(bool includeInactive, CancellationToken cancellationToken = default)
    {
        // TODO: Create get people code
        return await Task.Run(() => new List<Person>(new[] {
            new Person { Id = 1, FirstName = "Brandon", LastName = "Smith", Active = true },
            new Person { Id = 2, FirstName = "Allison", LastName = "Brown", Active = true },
            new Person { Id = 3, FirstName = "Patricia", LastName = "McDonald" }
        }).Where(p => includeInactive || p.Active));
    }
}
