using System;

namespace Lemoncode.Books.FunctionalTests.TestSupport.Extensions
{
    public static class IntExtensions
    {
        public static DateTime ToUtcDate(this int secondsOffset)
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var result = dateTime.AddSeconds(secondsOffset);
            return result;
        }
    }
}
