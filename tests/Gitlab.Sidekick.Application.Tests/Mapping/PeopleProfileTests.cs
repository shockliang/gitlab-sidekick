using AutoMapper;
using Gitlab.Sidekick.Application.Mapping;
using Xunit;

namespace Gitlab.Sidekick.Application.Tests.Mapping;

public class PeopleProfileTests
{
    [Fact]
    public void VerifyConfiguration()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<PeopleProfile>());

        configuration.AssertConfigurationIsValid();
    }
}
