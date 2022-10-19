namespace Lemoncode.DependencyInversionPrincipleWithInterface
{
    public class Calculator
    {
        private IPrinter _printer;

        public Calculator(IPrinter printer)
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
        public void ReplacePrinter(IPrinter printer)
        {
            _printer = printer;
        }
    }
}
