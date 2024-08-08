namespace Lemoncode.DependencyInversionPrinciple
{
    public class Calculator
    {
        private Printer _printer;

        public Calculator(Printer printer)
        {
            _printer = printer;
        }

        public void Sum(int a, int b)
        {
            var result = a + b;
            _printer.Print($"Result: {result}");
        }

        public void Sub(int a, int b)
        {
            var result = a - b;
            _printer.Print($"Result: {result}");
        }

        /// <summary>
        /// Printer can be replaced at runtime
        /// </summary>
        public void ReplacePrinter(Printer printer)
        {
            _printer = printer;
        }
    }
}
