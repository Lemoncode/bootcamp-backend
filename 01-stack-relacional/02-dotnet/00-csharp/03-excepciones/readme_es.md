# Excepciones

El control de excepciones es muy necesario para poder manejar errores que se producen en tiempos de ejecución de un programa.
En .Net usamos las palabras clave `try` `catch` y `finally` para ello.

- `try`: encapsula el código que puede causar excepciones.
- `catch`: encapsula el manejo de la excepción
- `finally`: código que se ejecuta haya o no excepciones. No se usa siempre, un ejemplo de uso es para liberar objetos de memoria, o cerrar alguna cadena de conexión a la base de datos.

Las excepciones pueden ser generadas por el CLR (Common Language Runtime), .NET, librerías de terceros o el propio código de la aplicación que estemos desarrollando.

Veamos un ejemplo de excepción no controlada:

```csharp
Console.WriteLine("Inserta un número: ");
string valor = Console.ReadLine();

if (valor is not null)
{
    var numero = int.Parse(valor);
    var doble = 2 * numero;
    Console.WriteLine($"El doble de {numero} es {doble}");
    Console.ReadLine();
}
```

En este caso al ejecutar el código nos devuelve una excepción del tipo `System.FormatException`

Vamos a capturar y manejar la excepción

```diff
+   try
+   {
        Console.WriteLine("Inserta un número: ");
        string valor = Console.ReadLine();
        if (valor is not null)
        {
            var numero = int.Parse(valor);
            var doble = 2 * numero;
            Console.WriteLine($"El doble de {numero} es {doble}");
-           Console.ReadLine();
        }
+   }
+   catch (Exception ex)
+   {
+       Console.WriteLine("Por favor, ingrese un número");
+   }
+   Console.ReadLine();
```

En este ejemplo como la línea `var numero = int.Parse(valor)` da una excepción, la excepción es capturada y el código sigue ejecutandose por el `catch`

## Tipos de Excepciones

En C# todas la excepciones derivan de `System.Exception`,
Con el bloque `catch` podemos capturar este `System.Exception` o podemos capturar excepciones de forma más específica.
Algunas excepciones más comunes:

- ArgumentException: se lanza cuando uno de los argumentos que se pasan a un método no es válido.
  Ejemplo:

```csharp
 Console.WriteLine("División de 100 entre 2 = {0}", DividirPorDos(100));

try
{
    // Aquí la excepción ArgumentException es lanzada debido a que el
    // dividendo es un número impar:
    Console.WriteLine("13 dividido por 2 = {0}", DividirPorDos(13));
}
catch (ArgumentException ae)
{
    Console.WriteLine("Mensaje de error: `{0}`", ae.Message);
}

static int DividirPorDos(int numero)
{
    // Si el número no es par, entonces
    // se lanzará la excepción `ArgumentException`:
    if ((numero % 2) == 1)
    {
        throw new ArgumentException("El número debe ser par.", "numero");
    }

    return numero / 2;
}
```

De esta excepción derivan otras dos:

- ArgumentNullException: se lanza cuando una referencia `null` es pasada a un método que no acepta este valor:

  ```csharp
   try
    {
        string mensaje = "Hola mundo";
        ImprimirMensaje(mensaje);
    
        string mensaje2 = null;
        ImprimirMensaje(mensaje2);
    }
    catch (ArgumentNullException ae)
    {
        Console.WriteLine("Mensaje de error: `{0}`", ae.Message);
    }
    
    static void ImprimirMensaje(string mensaje)
    {
        if (mensaje == null)
        {
            throw new ArgumentNullException("mensaje", "El mensaje no puede ser nulo");
        }
    
        Console.WriteLine(mensaje);
    }
  ```

- ArgumentOutOfRangeException: se lanza cuando el valor de un argumento está fuera de los límites inferior y superior:

```csharp
    try
    {
        var votante1 = new Votante("Maria", 25);
        Console.WriteLine($"Votante 1: Maria");
        var votante2 = new Votante("Antonio", 7);
        Console.WriteLine($"Votante 2: Antonio");
    }
    catch (ArgumentOutOfRangeException ae)
    {
        Console.WriteLine("Mensaje de error: `{0}`", ae.Message);
    }
    
    class Votante
    {
        private string _nombre;
        private int _edad;
    
        public Votante(string nombre, int edad)
        {
            _nombre = nombre;
    
            if (edad <= 18)
            {
                throw new ArgumentOutOfRangeException("edad", "El votante no puede ser menor de edad");
            }
            else
            {
                _edad = edad;
            }
        }
    }
```

- DivideByZeroException: se lanza cuando intentamos dividir por cero

```csharp
    try
    {
        Console.WriteLine("Añade el primer número: ");
        var numero1 = int.Parse(Console.ReadLine());
        Console.WriteLine("Añade el segundo número: ");
        var numero2 = int.Parse(Console.ReadLine());
    
        var resultado = numero1 / numero2;
    }
    catch (FormatException ce)
    {
        Console.WriteLine("Mensaje de error: `{0}`", ce.Message);
    
    }
    catch (DivideByZeroException ae)
    {
        Console.WriteLine("Mensaje de error: `{0}`", ae.Message);
    }
```

- NullReferenceException: se lanza cuando se intenta acceder o manipular el estado de una variable que tiene asignada la referencia null:

```csharp
  
    try
    {
        ArrayList array = null;

        array.Add("hola");
    }
    catch(Exception ex)
    {
        Console.WriteLine("Mensaje de error: `{0}`", ex.Message);
    }
      
```

- OverflowException: se lanza cuando operaciones aritméticas o de conversiones sobrepasan los límites de memoria de tipos de datos:

```csharp
   
    try
    {
        checked
        {
            int suma = Int32.MaxValue + Int32.Parse("1");
            Console.WriteLine($"El resultado de sumar {Int32.MaxValue} más 1 es: {suma}");
        }
    }
    catch(OverflowException ex)
    {
        Console.WriteLine("Mensaje de error: `{0}`", ex.Message);
    }

```

Para que la operación aritmética o de conversión produzca una `OverflowException` la operación tiene que producirse en un contexto comprobado: `checked`. En caso contrario el resultado se trunca descartando los bits mayores que no caben el tipo de destino.

```csharp

    try
    {
        int suma = Int32.MaxValue + Int32.Parse("1");
        Console.WriteLine($"El resultado de sumar {Int32.MaxValue} más 1 es: {suma}");
    
    }
    catch(OverflowException ex)
    {
        Console.WriteLine("Mensaje de error: `{0}`", ex.Message);
    }
  
```

También se pueden crear excepciones personalizadas. Para ello sólo hay que crear una clase que herede de la clase `Exception` y añadir un constructor. Ejemplo:

```csharp
public class DniInvalidoException: Exception {
    public DniInvalidoException(string message): base(message)
    {
        Console.WriteLine(message);
    }
}
```

Para utilizar esta excepción que hemos creado hay que instanciarla manualmente:

```diff
+ try
+ {
+     Console.WriteLine("Añada DNI:");
+     var dni = Console.ReadLine();+ 
+     if (!string.IsNullOrEmpty(dni) && ValidateDNI(dni))
+     {
+         Console.WriteLine($"DNI: {dni} es válido");
+     }
+     else
+     {
+         throw new DniInvalidoException($"Error!! DNI: {dni} no es válido");
+     }
+ }
+ catch (DniInvalidoException ex)
+ {
+     Console.ReadLine();
+ }
+ static bool ValidateDNI(string dni)
+ {
+     string pattern = @"^((\d{8})|(\d{8}([A-Z]|[a-z])))$";

+     Regex r = new Regex(pattern);

+     if ((r.IsMatch(dni)))
+     {
+         return true;
+     }
+     else
+     {
+         return false;
+     }
+ }
+ public class DniInvalidoException : Exception
{
    public DniInvalidoException(string message) : base(message)
    {
        Console.WriteLine(message);
    }
}
```
