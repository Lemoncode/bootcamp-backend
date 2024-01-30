using DependencyInversionPrinciple.Entities;
using DependencyInversionPrinciple.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionPrinciple.Managers;

internal class SellerConsoleManager
{
    private readonly SellerService _sellerService;

    private readonly BuyerService _buyerService;

    public SellerConsoleManager(SellerService sellerService, BuyerService buyerService)
    {
        _sellerService = sellerService;
        _buyerService = buyerService;
    }

    public async Task Run()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Elige una opción:");
            Console.WriteLine("1. Agregar un producto");
            Console.WriteLine("2. Editar un producto");
            Console.WriteLine("3. Eliminar un producto");
            Console.WriteLine("4. Volver a la pantalla anterior.");

            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    await AddProduct();
                    break;
                case "2":
                    await EditProduct();
                    break;
                case "3":
                    await DeleteProduct();
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    private async Task AddProduct()
    {
        Product product = CreateProduct();
        if (product is null)
        {
            Console.WriteLine("No se pudo añadir el producto.");
        }
        else
        {
            await _sellerService.AddProduct(product);
            Console.WriteLine("Producto agregado exitosamente.");
        }
    }

    private async Task EditProduct()
    {
        Console.WriteLine("Ingrese el ID del producto a editar:");
        var input = Console.ReadLine();

        if (Guid.TryParse(input, out Guid productId))
        {
            var existingProduct = await _buyerService.GetProduct(productId);
            if (existingProduct is null)
            {
                Console.WriteLine("Ese producto no existe.");
                return;
            }

            var product = CreateProduct(productId);
if (product is null)
            {
                Console.WriteLine("Imposible obtener los datos del nuevo producto.");
                return;
            }

            await _sellerService.EditProduct(product);
            Console.WriteLine("Producto editado exitosamente.");
        }
        else
        {
            Console.WriteLine("ID inválido.");
        }
    }

    private async Task DeleteProduct()
    {
        Console.WriteLine("Ingrese el ID del producto a eliminar:");
        var input = Console.ReadLine();

        if (Guid.TryParse(input, out Guid productId))
        {
            await _sellerService.DeleteProduct(productId);
            Console.WriteLine("Producto eliminado.");
        }
        else
        {
            Console.WriteLine("ID inválido.");
        }
    }

    private Product? CreateProduct(Guid? id = null)
    {
        Guid productId = id ?? Guid.NewGuid();
        Console.WriteLine("Ingrese el nombre del producto:");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("El nombre es obligatorio.");
            return null;
        }

        Console.WriteLine("Ingrese la descripción del producto:");
        var description = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("La descripción es obligatoria.");
            return null;
        }

        Console.WriteLine("Ingrese el precio del producto:");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("Precio inválido. Se establecerá en 0.");
            return null;
        }

        return new Product
        {
            Id = productId,
            Name = name,
            Description = description,
            Price = price
        };
    }

}
