using System;

namespace Lemoncode.DependencyInversionPrincipleWithInterface
{
    public class ConsolePrinter
        : IPrinter
    {
        public void Print(string text)
        {
            Console.WriteLine(text);
        }
    }
}
