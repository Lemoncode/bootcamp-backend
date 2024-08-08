namespace Lemoncode.DependencyInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            var printer = new Printer();
            var calculator = new Calculator(printer);
           
            calculator.Sum(10, 5);
            calculator.Sub(30, 20);
        }
    }
}
