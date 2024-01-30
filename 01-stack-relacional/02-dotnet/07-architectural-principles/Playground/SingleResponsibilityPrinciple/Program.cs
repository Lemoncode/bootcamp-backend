using SingleResponsibilityPrinciple.Entities;

namespace SingleResponsibilityPrinciple;

internal class Program
{
    static async Task Main(string[] args)
    {
        var exit = false;

        while (!exit)
        {
            Console.WriteLine("1. Añadir empleado");
            Console.WriteLine("2. Editar empleado");
            Console.WriteLine("3. Eliminar empleado");
            Console.WriteLine("4. Lista de empleados.");
            Console.WriteLine("5. Salir");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    // Añadir empleado
                    var newEmployee = CreateEmployee(true);
                    if (newEmployee is null)
                    {
                        Console.WriteLine("Imposible crear el empleado.");
                        continue;
                    }

                    await newEmployee.Save();
                    break;

                case "2":
                    // Editar empleado
                    Console.WriteLine("Ingrese ID del empleado a editar:");
                    if (!int.TryParse(Console.ReadLine(), out var editId))
                    {
                        Console.WriteLine("Eso no es un número.");
                        continue;
                    }

                    var employeeToEdit = Employee.Find(editId);
                    if (employeeToEdit != null)
                    {
                        var tempEmployee = CreateEmployee(false);
                        if (tempEmployee is null)
                        {
                            Console.WriteLine("Imposible editar el empleado.");
                            continue;
                        }

                        employeeToEdit.FirstName = tempEmployee.FirstName;
                        employeeToEdit.LastName = tempEmployee.LastName;
                        employeeToEdit.Address = tempEmployee.Address;
                        await employeeToEdit.Update();
                    }
                    else
                    {
                        Console.WriteLine(" no se encontró un empleado con ese identificador.");
                    }
                    break;

                case "3":
                    // Eliminar empleado
                    Console.WriteLine("Ingrese ID del empleado a eliminar:");

                    if (!int.TryParse(Console.ReadLine(), out var deleteId))
                    {
                        Console.WriteLine("Eso no es un número.");
                        continue;
                    }

                    var employeeToDelete = Employee.Find(deleteId);
                    if (employeeToDelete != null)
                    {
                        await employeeToDelete.Delete();
                        Console.WriteLine("Borrado.");
                    }
                    else
                    {
                        Console.WriteLine("No existe un empleado con ese identificador.");
                    }
                    break;

                case "4":
                    foreach (var employee in Employee.GetAllEmployees())
                    {
                        Console.WriteLine(employee.ToString());
                    }
                    break;
                case "5":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    public static Employee? CreateEmployee(bool needIdentifier)
    {
        int employeeId = 0;
        if (needIdentifier)
        {
            Console.WriteLine("Introduce el ID del empleado:");
            if (!int.TryParse(Console.ReadLine(), out employeeId))
            {
                Console.WriteLine("El ID no es numérico.");
                return null;
            }
        }

        Console.WriteLine("Introduce el nombre del empleado:");
        var firstName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(firstName))
        {
            Console.WriteLine("El nombre es obligatorio.");
            return null;
        }

        Console.WriteLine("Introduce el apellido del empleado:");
        var lastName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Los apellidos son obligatorios.");
            return null;
        }

        Console.WriteLine("Introduce la dirección del empleado:");
        var address = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(address))
        {
            Console.WriteLine("La dirección del empleado es obligatoria.");
            return null;
        }

        return new Employee { Id = employeeId, FirstName = firstName, LastName = lastName, Address = address };
    }
}