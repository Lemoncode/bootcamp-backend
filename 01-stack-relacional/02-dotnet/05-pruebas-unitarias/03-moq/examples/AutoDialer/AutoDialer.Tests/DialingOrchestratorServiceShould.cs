using AutoDialer.Contracts;
using AutoDialer.services;

using Moq;

namespace AutoDialer.Tests;

public class DialingOrchestratorServiceShould
{
    private readonly Mock<IDialService> _mockDialService;
    private readonly Mock<IPhoneRepository> _mockPhoneRepository;
    private readonly DialingOrchestratorService _orchestrator;

    public DialingOrchestratorServiceShould()
    {
        _mockDialService = new Mock<IDialService>();
        _mockPhoneRepository = new Mock<IPhoneRepository>();
        _orchestrator = new DialingOrchestratorService(_mockDialService.Object, _mockPhoneRepository.Object);
    }

    [Fact]
    public async Task DialAllAvailablePhones()
    {
        var phones = new Queue<string?>(new[] { "123456789", "987654321", null });

        _mockPhoneRepository.Setup(m => m.GetPhone()).Returns(() => phones.Dequeue());
        _mockPhoneRepository.Setup(m => m.SetPhoneAsUsed()).Verifiable();

        await _orchestrator.Start();

        _mockPhoneRepository.Verify(m => m.GetPhone(), Times.Exactly(3));
        _mockPhoneRepository.Verify(m => m.SetPhoneAsUsed(), Times.Exactly(2));
        _mockDialService.Verify(m => m.Dial(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<TimeSpan?>()), Times.Exactly(2));
    }
}
