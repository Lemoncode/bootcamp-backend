using DependencyInversionPrinciple.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionPrinciple.Managers;

internal class BuyerConsoleManager
{

    private readonly BuyerService _buyerService;

    public BuyerConsoleManager(BuyerService buyerService)
    {
        _buyerService = buyerService;
    }

    public async Task Run()
    {
        var exit = false;
        while (!exit)
        {
            Console.WriteLine("Elige una opción:");
            Console.WriteLine("1. Listar todos los productos");
            Console.WriteLine("2. Obtener información de un producto");
            Console.WriteLine("3. Volver a la pantalla anterior");

            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    await ListAllProducts();
                    break;
                case "2":
                    await GetProductInfo();
                    break;
                case "3":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    private async Task ListAllProducts()
    {
        var products = await _buyerService.GetAllProducts();
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.Id}, Nombre: {product.Name}, Precio: {product.Price}");
        }
    }

    private async Task GetProductInfo()
    {
        Console.WriteLine("Ingrese el ID del producto:");
        var input = Console.ReadLine();

        if (Guid.TryParse(input, out Guid productId))
        {
            var product = await _buyerService.GetProduct(productId);
            if (product != null)
            {
                Console.WriteLine($"Nombre: {product.Name}, Descripción: {product.Description}, Precio: {product.Price}");
            }
            else
            {
                Console.WriteLine("Producto no encontrado.");
            }
        }
        else
        {
            Console.WriteLine("ID inválido.");
        }
    }
}