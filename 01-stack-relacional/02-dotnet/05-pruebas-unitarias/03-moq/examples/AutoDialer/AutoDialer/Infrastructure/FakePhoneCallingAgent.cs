using AutoDialer.Exceptions;

using NAudio.Wave;

namespace AutoDialer.Infrastructure;

public class FakePhoneCallingAgent : IPhoneCallingAgent
{

    private readonly Dictionary<Guid, bool> _jobs = new();

    public Task<Guid> MakeCall(string phoneNumber, Stream audioMessageInMp3)
    {
        ArgumentNullException.ThrowIfNull(phoneNumber);
        ArgumentNullException.ThrowIfNull(audioMessageInMp3);

        Console.WriteLine($"Llamando al teléfono {phoneNumber}...");
        phoneNumber = "s" + phoneNumber + "h";

        var guid = Guid.NewGuid();

        _jobs.Add(guid, false);
        PlayTones(phoneNumber)
            .ContinueWith((result) =>
            {
                _jobs[guid] = true;
            });

        audioMessageInMp3.Dispose();
        return Task.FromResult(guid);
    }

    public Task<bool> CallIsComplete(Guid jobId)
    {
        if (_jobs.TryGetValue(jobId, out var state))
        {
            if (state)
            {
                Console.WriteLine("Llamada finalizada.");
            }

            return Task.FromResult(state);
        }

        throw new JobNotFoundException(jobId);
    }

    private async Task PlayTones(string numbers)
    {
        foreach (var digit in numbers)
        {
            await PlayTone(digit);
        }
    }
    private async Task PlayTone(char digit)
    {
        var fileName = $"sounds/{digit}.mp3";
        if (File.Exists(fileName))
        {
            using (var audioFile = new AudioFileReader(fileName))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    await Task.Delay(1);
                }
            }
        }
        else
        {
            Console.WriteLine($"Sound file for {digit} not found.");
        }
    }
}
