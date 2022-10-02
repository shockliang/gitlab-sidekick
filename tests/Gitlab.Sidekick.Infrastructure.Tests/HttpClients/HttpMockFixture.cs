using WireMock.Server;

namespace Gitlab.Sidekick.Infrastructure.Tests.HttpClients;

public class HttpMockFixture : IDisposable
{
    public WireMockServer MockServer { get; }

    public HttpMockFixture()
    {
        MockServer = WireMockServer.StartWithAdminInterface();
    }

    public void Dispose()
    {
        MockServer.Stop();
    }
}
