namespace Lemoncode.DependencyInjection
{
    public class Calculator
    {
        private readonly Printer _printer;

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
    }
}
