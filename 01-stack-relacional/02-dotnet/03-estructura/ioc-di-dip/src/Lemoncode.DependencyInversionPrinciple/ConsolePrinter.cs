using System;

namespace Lemoncode.DependencyInversionPrinciple
{
    public class ConsolePrinter
        : Printer
    {
        public override void Print(string text)
        {
            Console.WriteLine(text);
        }
    }
}
