using AutoDialer.Contracts;
using AutoDialer.Infrastructure;

namespace AutoDialer;

public class DialService(IPhoneCallingAgent phoneCallingAgent) : IDialService
{

    private readonly IPhoneCallingAgent _phoneCallingAgent = phoneCallingAgent;

    public async Task Dial(string number, Stream messageInMp3, TimeSpan? timeout = default)
    {
        timeout ??= TimeSpan.FromSeconds(60);

        var jobId = await _phoneCallingAgent.MakeCall(number, messageInMp3);
        DateTime start = DateTime.Now;
        do
        {
            await Task.Delay(1000);
        } while (!(await _phoneCallingAgent.CallIsComplete(jobId)) && DateTime.Now - start < timeout);
        
        if (DateTime.Now - start > timeout)
        {
            throw new TimeoutException($"Timeout when dialing.");
        }
    }
}