using System;

namespace Lemoncode.DependencyInversionPrincipleWithInterface
{
    public class PaperPrinter
        : IPrinter
    {
        public void Print(string text)
        {
            // I use some OS printing drivers to connect to a device and send documents
            Console.WriteLine($"A4 paper says: {text}");
        }
    }
}
