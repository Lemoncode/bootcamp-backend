// See https://aka.ms/new-console-template for more information
using DependencyInversionPrinciple.Managers;
using DependencyInversionPrinciple.Repositories;
using DependencyInversionPrinciple.Services;

using System.ComponentModel.Design;
var productRepository = new ProductRepository(); ;
var buyerService = new BuyerService(productRepository);
var sellerService = new SellerService(productRepository);
var sellerManager = new SellerConsoleManager(sellerService, buyerService);
var buyerManager = new BuyerConsoleManager(buyerService);

var exit = false;
while (!exit)
{
    Console.WriteLine("¡Hola! ¿Qué eres?");
    Console.WriteLine("1. Vendedor.");
    Console.WriteLine("2. Comprador.");
    Console.WriteLine("3. Salir.");
    var option = Console.ReadLine();
    switch (option)
    {
        case "1":
            await sellerManager.Run();
            break;
        case "2":
            await buyerManager.Run();
            break;
        case "3":
            Console.WriteLine("¡Adiós!");
            exit = true;
            break;
        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
}
