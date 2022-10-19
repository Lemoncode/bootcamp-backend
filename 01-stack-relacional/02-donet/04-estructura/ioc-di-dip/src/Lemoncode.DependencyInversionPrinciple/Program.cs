﻿using System;

namespace Lemoncode.DependencyInversionPrinciple
{
    class Program
    {
        static void Main(string[] args)
        {
            Printer printer = new ConsolePrinter(); // soy una impresora de consola
            var calculator = new Calculator(printer);
            calculator.Sum(10, 5);
            calculator.Sub(30, 20);

            Console.WriteLine("Now I replace printer with a paper one");
            calculator.ReplacePrinter(new PaperPrinter());
            calculator.Sum(10, 5);
            calculator.Sub(30, 20);
        }
    }
}
