using TaxCalculator.Services;

Console.WriteLine("Salario bruto anual:");
var grossStr = Console.ReadLine();
if (!Decimal.TryParse(grossStr, out decimal gross))
{
    Console.WriteLine("No es un valor válido. Pulsa enter para salir.");
    Console.ReadLine();
    return;
}
var taxService = new TaxService();
Console.WriteLine($"Vas a pagar {taxService.GetTax(gross)}.");
Console.ReadLine();