using System;

namespace Lemoncode.InversionOfControl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Inversion of Control with a C# delegate (callback)");
            var sampleDelegate = new SampleDelegate();
            sampleDelegate.RunForCurrentDate(date => Console.WriteLine(date.ToLongDateString()));

            Console.WriteLine("Inversion of Control with a factory");
            var date = SampleFactory.CreateDateTime();
            Console.WriteLine(date);
        }
    }
}
