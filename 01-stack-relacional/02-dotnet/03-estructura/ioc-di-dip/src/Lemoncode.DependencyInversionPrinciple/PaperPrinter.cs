using System;

namespace Lemoncode.DependencyInversionPrinciple
{
    public class PaperPrinter
        : Printer
    {
        public override void Print(string text)
        {
            // I use some OS printing drivers to connect to a device and send documents
            Console.WriteLine($"A4 paper says: {text}");
        }
    }
}
