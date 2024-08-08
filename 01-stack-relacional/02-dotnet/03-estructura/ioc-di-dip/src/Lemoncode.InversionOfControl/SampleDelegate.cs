using System;

namespace Lemoncode.InversionOfControl
{
    public class SampleDelegate
    {
        public void RunForCurrentDate(Action<DateTime> callback)
        {
            var currentDate = DateTime.UtcNow;
            callback.Invoke(currentDate);
        }
    }
}
