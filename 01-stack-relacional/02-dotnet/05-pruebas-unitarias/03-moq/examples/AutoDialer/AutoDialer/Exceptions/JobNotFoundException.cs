namespace AutoDialer.Exceptions
{
    public class JobNotFoundException : Exception
    {
        public JobNotFoundException()
        {
        }

        public JobNotFoundException(Guid jobId) : this($"The job with Id {jobId} was not found.")
        {
        }

        public JobNotFoundException(string? message) : base(message)
        {
        }

        public JobNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}