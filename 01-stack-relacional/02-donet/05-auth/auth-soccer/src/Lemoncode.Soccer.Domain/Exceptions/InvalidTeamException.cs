using System;

namespace Lemoncode.Soccer.Domain.Exceptions
{
    public class InvalidTeamException
        : Exception
    {
        public InvalidTeamException(string message)
            : base(message)
        {
        }
    }
}