
namespace AutoDialer.Infrastructure
{
    public interface IPhoneCallingAgent
    {
        Task<bool> CallIsComplete(Guid jobId);
        Task<Guid> MakeCall(string phoneNumber, Stream audioMessageInMp3);
    }
}