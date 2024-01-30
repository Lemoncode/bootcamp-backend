using System;
using System.Text.RegularExpressions;

namespace DRY
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var exit = false;

            while (!exit)
            {
                Console.WriteLine("Elije una opción:");
                Console.WriteLine("1. Iniciar sesión");
                Console.WriteLine("2. Crear cuenta");
                Console.WriteLine("3. Olvidé mi contraseña!");
                Console.WriteLine("4. Salir");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        SignIn();
                        break;
                    case "2":
                        SignUp();
                        break;
                    case "3":
                        LostPassword();
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

        static void SignIn()
        {
            Console.WriteLine("Introduce tu correo electrónico:");
            var email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("El e-mail está vacío.");
                return;
            }

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern))
            {
                Console.WriteLine("Correo electrónico no válido.");
            }
            else
            {
                // Lógica de inicio de sesión
                Console.WriteLine("Inicio de sesión exitoso.");
            }
        }

        static void SignUp()
        {
            Console.WriteLine("Ingrese su correo electrónico para la cuenta:");
            var email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("El e-mail está vacío.");
                return;
            }

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern))
            {
                Console.WriteLine("Correo electrónico no válido.");
            }
            else
            {
                // Lógica de creación de cuenta
                Console.WriteLine("Cuenta creada exitosamente.");
            }
        }

        static void LostPassword()
        {
            Console.WriteLine("Ingrese su correo electrónico para recuperar su contraseña:");
            var email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("El e-mail está vacío.");
                return;
            }

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern))
            {
                Console.WriteLine("Correo electrónico no válido.");
            }
            else
            {
                // Lógica de recuperación de contraseña
                Console.WriteLine("Instrucciones para recuperar la contraseña enviadas.");
            }
        }
    }
}
