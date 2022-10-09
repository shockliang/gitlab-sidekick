using Gitlab.Sidekick.Application.Core.Queries;
using Gitlab.Sidekick.Application.Interfaces.HttpClients;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Gitlab.Sidekick.Application.Tests.Core.Queries;

public class GroupQueryTests
{
    private readonly IGitLabClient _gitLabClientMock;
    private readonly ILogger<GroupQueryHandler> _loggerMock;
    private readonly GroupQueryHandler _sut;

    public GroupQueryTests()
    {
        _gitLabClientMock = Substitute.For<IGitLabClient>();
        _loggerMock = Substitute.For<ILogger<GroupQueryHandler>>();
        _sut = new GroupQueryHandler(_loggerMock, _gitLabClientMock);

    }

    [Fact]
    public async Task ItShould_Invoke_GitlabClient_ToGetGroup()
    {
        // Arrange
        var request = new GroupQuery { GroupId = 1 };

        // Act
        await _sut.Handle(request, CancellationToken.None);

        // Assert
        await _gitLabClientMock
            .Received()
            .GetGroup(request.GroupId);
    }
}
