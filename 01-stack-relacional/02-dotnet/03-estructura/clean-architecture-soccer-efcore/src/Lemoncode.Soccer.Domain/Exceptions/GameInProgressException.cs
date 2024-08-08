using System;

namespace Lemoncode.Soccer.Domain.Exceptions
{
    public class GameInProgressException
        : Exception
    {
        public GameInProgressException()
            : base("The game is in progress")
        {
        }
    }
}