using System;

namespace Lemoncode.Soccer.Application.Services
{
    public interface IDateTimeService
    {
        DateTime GetUtcNow();
    }
}