
namespace AutoDialer.Infrastructure
{
    public interface IPhoneCallingAgent
    {
        Task<Guid> MakeCall(string phoneNumber, Stream audioMessageInMp3);
        Task<bool> CallIsComplete(Guid jobId);
    }
}