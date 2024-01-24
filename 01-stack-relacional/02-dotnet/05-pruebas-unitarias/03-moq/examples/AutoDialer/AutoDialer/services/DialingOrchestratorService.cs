using AutoDialer.Contracts;

namespace AutoDialer.services;

public class DialingOrchestratorService(IDialService dialService, IPhoneRepository phoneRepository)
{

    private readonly IDialService _dialService = dialService;

    private readonly IPhoneRepository _phoneRepository = phoneRepository;

    public async Task Start()
    {
        string? phone = default;

        do
        {
            phone = _phoneRepository.GetPhone();
            if (phone is not null)
            {
                await _dialService.Dial(phone, File.OpenRead(Path.Combine(AppContext.BaseDirectory, "sounds", "h.mp3")));
                _phoneRepository.SetPhoneAsUsed();
            }
        } while (phone is not null);
    }
}
