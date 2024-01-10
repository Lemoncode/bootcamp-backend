using System;

namespace Lemoncode.Soccer.Application.Services
{
    public class DateTimeService
        : IDateTimeService
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}