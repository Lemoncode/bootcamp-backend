using System;

namespace Lemoncode.Soccer.Application.Exceptions
{
    public class ResourceNotFoundException
        : Exception
    {
        public ResourceNotFoundException(string message)
            : base(message)
        {
        }
    }
}