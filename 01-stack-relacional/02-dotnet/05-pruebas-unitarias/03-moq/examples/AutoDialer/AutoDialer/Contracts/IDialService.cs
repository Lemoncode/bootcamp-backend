namespace AutoDialer.Contracts
{
    public interface IDialService
    {
        Task Dial(string number, Stream messageInMp3, TimeSpan? timeout = default);
    }
}