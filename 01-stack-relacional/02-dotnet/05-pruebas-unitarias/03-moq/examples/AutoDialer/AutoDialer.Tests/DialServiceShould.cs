using AutoDialer;
using AutoDialer.Contracts;
using AutoDialer.Infrastructure;

using Moq;

using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoDialer.Tests;

public class DialServiceShould
{
    private readonly Mock<IPhoneCallingAgent> _mockPhoneCallingAgent;
    private readonly DialService _dialService;

    public DialServiceShould()
    {
        _mockPhoneCallingAgent = new Mock<IPhoneCallingAgent>();
        _dialService = new DialService(_mockPhoneCallingAgent.Object);
    }

    [Fact]
    public async Task CompleteCallWithinTimeLimit()
    {
        var jobId = Guid.NewGuid();
        _mockPhoneCallingAgent.Setup(m => m.MakeCall(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync(jobId);
        _mockPhoneCallingAgent.Setup(m => m.CallIsComplete(jobId))
            .ReturnsAsync(true);

        await _dialService.Dial("1234567890", new MemoryStream());

        _mockPhoneCallingAgent.Verify(m => m.MakeCall(It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
        _mockPhoneCallingAgent.Verify(m => m.CallIsComplete(jobId), Times.Once);
    }

    [Fact]
    public async Task ThrowTimeoutExceptionWhenCallNotCompletedInTime()
    {
        var jobId = Guid.NewGuid();
        _mockPhoneCallingAgent.Setup(m => m.MakeCall(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync(jobId);
        _mockPhoneCallingAgent.Setup(m => m.CallIsComplete(jobId))
            .ReturnsAsync(false); // Simula que la llamada nunca se completa

        var exception = await Assert.ThrowsAsync<TimeoutException>(() => _dialService.Dial("1234567890", new MemoryStream(), TimeSpan.FromSeconds(3)));

        _mockPhoneCallingAgent.Verify(m => m.MakeCall(It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
        _mockPhoneCallingAgent.Verify(m => m.CallIsComplete(jobId), Times.Exactly(3));
    }
}
