# Temario

    Sesion 1:
        Convenciones x
        Variables
        Condicionales y bucles
        Clases y a base de ejemplos que vean los pilares.
    Sesion 2: 
        LINQ

## CONVENCIONES

Las convenciones en el código nos ayudan a:

- Tener un código consistente, con lo que un desarrollador que lea el código se pueda centrar en el contenido del código.
- Ayuda al desarrollador a entender de manera más fácil y rápida el código ya que todos seguimos las mismas convenciones.
- Facilita copiar, cambiar y mantener el código.
- En general, son buenas prácticas que todos hemos de seguir

### CONVENCIONES DE NOMENCLATURA
En .Net como en cualquier otro lenguaje existen unas normas básicas para el uso de mayúsculas y minúsculas.

Dependiendo del identificador podemos distinguir dos nomenclaturas:

1. **Pascal Case**: se usa para la mayoría de identificadores:

    - Espacios de nombres:  `namespace Vehiculos.Contratos`

    - Clases:  `public class Coche {}`

    - Interfaces: `public interface IVehiculo{}`

    - Propiedades: `public int NumeroRuedas {get; set;}`

    - Métodos: `public int ObtenerNumeroRuedas(){}`

    - Valor de enumerados:
        ``` csharp
        public enum Marca {
            Mercedes,
            Audi,
            Ford,
            Seat
        }
        ```

2. **camelCasing**: se utiliza para variables y parámetros
    ``` csharp
    public class Coche {
        public int ObtenerNumeroRuedas(Marca marcaVehiculo){
            int numeroRuedas = 0;
            ...
        }
    }
    ```

## CONVENCIONES GENERALES
1. Los nombres de los identificadores han de ser fáciles de leer
`NumeroRuedas` es más legible que `RuedasNumero`

2. No utilizar abreviaturas para mejorar la legilibidad. Por ejemplo, el método `ObtenerNumeroRuedas` es más legible que si se llamara simplemente `Ruedas`

3. Evitar el uso de acrónimos

4. Escribe sólo una sentencia o declaración por línea

5. Para los comentarios, este ha de ir en una línea a parte.

6. Empieza el comentario con mayúsculas.

7. Deja un espacio en blanco entre el delimitador de comentario // y el texto
    ```csharp
    // Esto es un comentario
    ```

#  VARIABLES
C# es un lenguaje fuertemente tipado.  Todas las variables y constantes tienen un tipo. 

## Tipos integrados vs. tipos personalizados
Los tipos de datos se pueden clasificar en:
- **Tipos integrados**: se usan para representar valores numéricos, expresiones booleanas, caracteres de texto, cadenas de texto.
    - int
    - float
    - double
    - decimal
    - char
    - bool
    - string

Los tipos numéricos flotantes son los indicados en la tabla, siendo el `decimal` el tipo más preciso, pero también el que más espacio en  memoria ocupa.

| Tipo | Precisión | Tamaño |
| ---- | --------- | ------ |
| `float` | de 6 a 9 dígitos | 4 bytes |
| `double` | de 15 a 17 dígitos | 8 bytes |
| `decimal` | 28-29 dígitos | 16 bytes |

   
- **Tipos personalizados**: `struct`, `class`, `interface`, `enum` y `record`. *Se verán más adelante en el apartado de clases*

    - **Struct**: engloban pequeños grupos de variables relacionadas entre sí
    ```csharp
    public struct Vehiculo 
    {
        public string Matricula;
        public string Marca;
    }     
    ```

    - **Class**: es una encapsulación de propiedades y métodos que se usan para representar una entidad
    ```csharp
    public class Persona
    {
        private string Nombre;
        private string Apellidos;
        private string DNI;

        public Persona(string nombre, string apellidos)
        {
            Nombre = nombre;
            Apellidos = apellidos;
        }
    }
    ```
    - **Interface**: define un contrato. Cualquier clase que implemente una interfaz debe de contener una implementación de los miembros definidos en esa interfaz
    ```csharp
    public interface IPersona
    {
        string NombreCompleto(string nombre, string apellidos);
    }
    ```
    - **Enum**: se usa para asignar nombres constantes a valores numéricos enteros
    ```csharp
    public enum DiasSemana
    {
        Lunes, //0
        Martes, //1
        Miercoles, //2
        Jueves, //3
        Viernes, //4
        Sabado, //5
        Domingo //6
    }
    ```

    - **Record**: es un nuevo tipo de referencia. Se diferencia de las clases en que los tipos de registros usan igualdad basada en valores, es decir, dos variables de tipo record son iguales si las definiciones de tipo son iguales y si en cada campo los valores de ambos records son iguales. En una clase, dos clases son iguales si los objetos a los que se hace referencia son el mismo tipo y las variables hacen referencia al mismo objeto. Los records se usan para admitir tipos de datos inmutables

    ```csharp
    public record Person
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
    };
    ```

## Tipos de valor vs. tipos de referencia
Los tipos integrados y personalizados pueden ser a su vez: 
- **Tipos de valor**: almacenan directamente los datos en su propia asignación de memoria. En este grupo tenemos: 
    - Todos los datos numéricos
    - Booleanos
    - Char
    - Date
    - Todas las estructuras
    - Enumeraciones
- **Tipos de referencia**: almacenan en memoria la referencia a sus datos. En este grupo están incluidos:
    - String
    - Las matrices
    - Clases
    - Delegados

Veamos unos ejemplos para entender mejor estos conceptos.
Demo:
Crear una aplicación básica de consola:
```csharp
public class Program
{
    public static int CalculaElDoble(int numero)
    {
        numero = numero * 2;
        return numero;
    }
    public static void Main(string[] args)
    {
        int i = 2;
        Console.WriteLine(CalculaElDoble(i)); // Devuelve 4
        Console.WriteLine(i); // Devuelve 2
        Console.ReadLine();
    }
}
```
Vemos que aunque en la función `SumaNumero` se ha cambiado el valor de la variable `numero`, la variable en sí no ha cambiado, y esto es porque realmente a la función se ha pasado una copia de la variable.

Veamos un ejemplo con un tipo por referencia:
```csharp
public class Persona
{
    public string Nombre;
    public int Edad;
}

public class Program
{
    public static void CambiarNombre(Persona persona)
    {
        persona.Nombre = "Manolo";
    }

    public static void Main(string[] args)
    {
        Persona persona = new Persona
        {
            Nombre = "Antonio",
            Edad = 40
        };
        Console.WriteLine(persona.Nombre);
        CambiarNombre(persona);
        Console.WriteLine(persona.Nombre);
        Console.ReadLine();
    }
}
```
En este caso sí cambia el valor de la variable, y esto es porque aquí le pasamos a la función una copia de la referencia en memoria al dato.

# Condicionales y bucles

Como en todo lenguaje de programación tenemos condicionales y bucles. 

## Condicionales
 - `if `
 ```csharp
 public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Introduce tu edad:");
        var edadUsuario = Console.ReadLine();
        var edad = int.TryParse(edadUsuario);

        if(edad > 18)
        {
            Console.WriteLine("Eres mayor de edad");
        }

        Console.ReadLine();
    }
}
 ```
 - `if-else`
 ```csharp
 public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Introduce tu edad:");
        var edadUsuario = Console.ReadLine();
        var edad = int.TryParse(edadUsuario);
        
        if(edad > 18)
        {
            Console.WriteLine("Eres mayor de edad");
        }
        else
        {
            Console.WriteLine("Eres menor de edad");
        }

        Console.ReadLine();
    }
}
 ```
 - `if-elseif`
 ```csharp
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Introduce tu edad:");
        var edadUsuario = Console.ReadLine();
        var edad = int.TryParse(edadUsuario);

        if(edad >= 18)
        {
            Console.WriteLine("Eres adulto");
        }
        else if (12 < edad && edad < 18)
        {
            Console.WriteLine("Eres adolescente");
        }
        else
        {
            Console.WriteLine("Eres un niño");
        }

        Console.ReadLine();
    }
}
 ```

 - `switch`
```csharp
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Introduce día de la semana:");
        var diaSemana = Console.ReadLine().ToLower();

        switch (diaSemana)
        {
            case "lunes":
            case "martes":
            case "miercoles":
            case "jueves":
            case "viernes":
                Console.WriteLine("Es día laboral, a trabajar!");
                break;
            case "sabado":
            case "domingo":
                Console.WriteLine("Es fin de semana, a descansar que te lo has ganado");
                break;
            default:
                Console.WriteLine("Introduce un día de la semana");
                break;
        }

        Console.ReadLine();
    }
}
 ```
 - Operador ternario
```csharp
 public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Introduce tu edad:");
        var edad = int.TryParse(Console.ReadLine());

        var resultado = edad >= 18 ? "Eres mayor de edad" : "Eres menor de edad";

        Console.WriteLine(resultado);
        Console.ReadLine();
    }
}
```
- Operador de valor nulo: recibe dos operandos y devuelve el primero que no sea nulo
```csharp
public class Program
{
    public static void Main(string[] args)
    {
        string nombre = null;
        string persona = nombre ?? "persona sin identificar";
        Console.WriteLine(persona);

        nombre = "Pepe";

        persona = nombre ?? "persona sin identificar";
        
        Console.WriteLine(persona);
        
        Console.ReadLine();
    }
}
```
## Bucles
- **while**: el código se ejecutará hasta que la condición sea falsa. La variable que esté en la condición ha de variar para no provocar un bucle infinito.
```csharp
public class Program
{
    public static void Main(string[] args)
    {
        int numero = 0;

        while (numero <= 5)
        {
            Console.WriteLine(numero);
            numero += 1;
        }

        Console.ReadLine();
    }
}
```

- **do-while**: parecida al while, pero en este caso se va a ejecutar al menos 1 vez, mientras que el while se puede ejecutar 0 o N veces.
```csharp
public class Program
{
    public static void Main(string[] args)
    {
        int numero;

        do
        {
            Console.WriteLine("Introduce un número entre 0 y 5:");
            numero = int.TryParse(Console.ReadLine());
        } while (numero >= 5 || numero < 0);

        Console.WriteLine($"El número introducido es: {numero}");
        Console.ReadLine();
    }
}
```

- **for**: en este caso, además de la condición se incluye la inicialización de la variable, así como el incremento o decremento de la misma. Si no se incluye el incremento, toma el valor por defecto de 1.
```csharp
public class Program
{
    public static void Main(string[] args)
    {
        for (var numero = 1; numero <= 5; numero++)
        {
            Console.WriteLine(numero);
        }

        Console.ReadLine();
    }
}
```

- **foreach**: se utiliza para recorrer colecciones de datos.
```csharp
public class Program
{
    public static void Main(string[] args)
    {
        List<string> nombres = new List<string>
        {
            "Ana",
            "Pedro",
            "Pepe",
            "Maria"
        };
        
        foreach(var nombre in nombres)
        {
            Console.WriteLine($"El nombre es: {nombre}");
        }

        Console.ReadLine();
  }
}
```

# Clases

Una clase es un conjunto de campos, propiedades, métodos y eventos. El objeto es la instancia de una clase para poder acceder a esas propiedades y métodos.
Ejemplo: una clase podría ser el plano de un motor, y un objeto son los distintos motores que se pueden construir usando dicho plano.

.Net es un lenguaje orientado a objetos, y por ello tiene las siguientes 3 propiedades: 

   - **Encapsulación**: Agrupa datos y código en una misma clase
   - **Herencia**: se crea una nueva clase a partir de otra ya existente,de la cual se heredan todos los miembros y métodos, y puede modificarse agregando o modificando propiedades y métodos heredados. Las clases creadas a partir de otras existentes se llaman **clases derivadas**
  - **Polimorfismo** permite utilizarse diferentes clases de forma intercambiable.

### Modificadores de accesso
- **public** una clase pública es accesible desde cualquier código en el mismo ensamblado o desde otro ensamblado que haga referencia a éste.
- **private** sólamente el código de la misma clase puede acceder al tipo o miembro
- **protected** sólamente el código de la misma clase, o de una clase que herede de esa clase puede acceder al tipo o miembro.
- **internal** tiene acceso el código del mismo ensamblado pero no de otro ensamblado distinto
- **protected internal** cualquier código del mismo ensamblado o desde una clase derivada de otro ensamblado, puede acceder al tipo o miembro
- **abstract** no permite crear instancias de esta clase, sólo sirve para ser heredada como clase base. Suele tener métodos definidos pero sin ninguna funcionalidad, ya que esos métodos se suelen escribir en las clases derivadas.
- **sealed** cuando una clase es la última de una jerarquía, por lo que no se puede usar como clase base de otra clase.

Ejemplo de **public**: 
1. Creamos un fichero a parte para la clase Person
```csharp
    public class Person
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public string FullName()
        {
            return $"{Name} {LastName}";
        }
    }
```
2. En el main del Program añadimos
```csharp
    public static void Main(string[] args)
    {
        var person = new Person
        {
            Name = "Pepe",
            LastName = "Garcia"
        };

        Console.WriteLine(person.FullName());

        Console.ReadLine();
    }
```
Como vemos el método `FullName` es accesible desde la clase Program

Ejemplo de **private**
1. En la clase anterior de `Person`cambiamos el método `FullName` a privado.
2. Comprobamos que ahora no es accesible desde la clase `Program`
3. Creamos un nuevo método publico en `Person`
```csharp
    public class Person
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public void PrintFullName()
        {
            Console.WriteLine(FullName());
        }
        private string FullName()
        {
            return $"{Name} {LastName}";
        }
    }
```
4. Accedemos a este nuevo método desde la clase `Program`
```csharp
    public static void Main(string[] args)
    {
        var person = new Person
        {
            Name = "Pepe",
            LastName = "Garcia"
        };

        person.PrintFullName();

        Console.ReadLine();
    }
```

Ejemplo de **protected**
1. Ahora cambiamos el método `PrintFullName`a protected.
2. Vemos que deja de ser accesible desde la clase `Program`
3. Creamos una nueva clase `Employee` que herede de `Person`
```csharp
    public class Employee : Person
    {
        public int NumEmployee { get; set; }

        public void PrintEmployee()
        {
            Console.WriteLine($"Name of Employee id: {NumEmployee}");
            PrintFullName();
        }
    }
``` 
4. Comprobamos que el método `PrintFullName`sí es accesible pero no el `FullName`. Ahora desde el `Program`: 
```csharp
    public static void Main(string[] args)
    {
        var employee = new Employee
        {
            Name = "Pepe",
            LastName = "Garcia",
            NumEmployee = 2
        };

        employee.PrintEmployee();

        Console.ReadLine();
    }
```

Ejemplo de **internal**
1. Creamos un nuevo proyecto de consola
2. Copiamos el fichero `Employee`en este nuevo proyecto.
3. Cambiamos el método `PrintFullName` de la clase  `Person` a internal
4. Vemos que ya no es accesible pero que en el proyecto original sigue siendolo

Ejemplo de **abstract**
1. Volvemos a dejar la clase `Person` como pública
```csharp
    public class Person
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public void PrintFullName()
        {
            Console.WriteLine(FullName());
        }
        private string FullName()
        {
            return $"{Name} {LastName}";
        }
    }
```
2. Desde la clase `Program` instanciamos de nuevo a la clase `Person`:
```csharp
    public static void Main(string[] args)
    {
        var person = new Person
        {
            Name = "Pepe",
            LastName = "Garcia"
        };
        person.PrintFullName();

        Console.ReadLine();
    }
```
3. Ahora cambiamos la clase `Person` a abstracta:
4. Comprobamos en la clase `Program` que no se puede instanciar.
5. Comprobamos que desde la clase `Employee` podemos acceder a la clase `Person` ya que hereda de ella.

Ejemplo de **sealed**
1. Volvemos a dejar las clases `Person` y `Employee` así:

```csharp
public abstract  class Person
{
    public string Name { get; set; }
    public string LastName { get; set; }

    public void PrintFullName()
    {
        Console.WriteLine(FullName());
    }
    public virtual string FullName()
    {
        return $"{Name} {LastName}";
    }
}

public class Employee : Person
{
    public int NumEmployee { get; set; }
    
    public void PrintEmployee()
    {
        Console.WriteLine($"Name of Employee id: {NumEmployee}");
        PrintFullName();
    }
}
```

2. Creamos una nueva clase `Employee2` que herede de `Employee`:
```csharp
    public class Employee2 : Employee
    {
        public int DNIEmployee { get; set; }
      
        public void PrintEmployee2()
        {
            PrintEmployee();
        }
    }
```

3. Cambiamos la clase `Employee` a sealed:
4. Comprobamos que ahora la clase `Employee2` no puede heredar de `Employee`

## Polimorfismo
Como ya hemos visto es la capacidad que tiene una clase de convertirse en un objeto nuevo y luego volver al objeto original del que salió.
Hay tres tipos de polimorfismos:
- **Polimorfismo por herencia**: se hereda de una clase normal y puedo convertirme en ella

```csharp
    public class Animal 
    {
        public string QuienSoy()
        {
            return "Soy un animal";
        }
    }

    public class Perro : Animal
    {
    }

    public class Gato: Animal
    {
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var perro = new Perro();
            var gato = new Gato();

            Animal[] animales = { perro, gato };

            foreach(var animal in animales)
            {
                Console.WriteLine(animal.QuienSoy());
            }
            
            
            Console.ReadLine();
        }
    }
```
En este ejemplo vemos como sea un perro o un gato el método QuienSoy siempre devuelve un animal.

Se puede cambiar el comportamiento del método QuienSoy e implementarlo para cada animal. Para ello el método QuienSoy pasa a ser virtual y se sobreescribe en cada una de las clases derivadas.

```csharp
    public class Animal 
    {
        public virtual string QuienSoy()
        {
            return "Soy un animal";
        }
    }

    public class Perro : Animal
    {
        public override string QuienSoy()
        {
            return "Soy un perro";
        }
    }

    public class Gato: Animal
    {
        public override string QuienSoy()
        {
            return "Soy un gato";
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var perro = new Perro();
            var gato = new Gato();

            Animal[] animales = { perro, gato };

            foreach(var animal in animales)
            {
                Console.WriteLine(animal.QuienSoy());
            }
                        
            Console.ReadLine();
        }
    }
```

- **Polimorfismo por abstracción**: se hereda de una clase abstracta.
En el siguiente ejemplo vamos a crear un método para saber como es cada animal, y el problema es que cada animal es de una manera diferente. Por ello la clase  Animal pasa a ser abstracta y creamos un método también abstracto, que será implementado en cada clase derivada.

```csharp
    public abstract class Animal 
    {
        public virtual string QuienSoy()
        {
            return "Soy un animal";
        }

        public abstract string ComoSoy();
    }

    public class Perro : Animal
    {
        public override string QuienSoy()
        {
            return "Soy un perro";
        }

        public override string ComoSoy()
        {
            return "Soy tu mejor amigo";
        }
    }

    public class Gato: Animal
    {
        public override string QuienSoy()
        {
            return "Soy un gato";
        }

        public override string ComoSoy()
        {
            return "Soy el rey de la casa";
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var perro = new Perro();
            var gato = new Gato();

            Animal[] animales = { perro, gato };

            foreach(var animal in animales)
            {
                Console.WriteLine(animal.QuienSoy());
                Console.WriteLine(animal.ComoSoy());
            }

            Console.ReadLine();
        }
    }
```
- **Polimorfismo por interfaz**: se implementa una interfaz.
El interfaz o contrato nos indica el comportamiento que ha de tener la clase que implemente la interfaz y la clase contendrá la implementación.
 En este caso de polimorfismo, se oculta la implementación del comportamiento.
```csharp
   public interface IAnimal 
    {
        string QuienSoy();

        string ComoSoy();
    }

    public class Perro : IAnimal
    {
        public string QuienSoy()
        {
            return "Soy un perro";
        }

        public string ComoSoy()
        {
            return "Soy tu mejor amigo";
        }
    }

    public class Gato: IAnimal
    {
        public string QuienSoy()
        {
            return "Soy un gato";
        }

        public string ComoSoy()
        {
            return "Soy el rey de la casa";
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var perro = new Perro();
            var gato = new Gato();

            IAnimal[] animales = { perro, gato };

            foreach(var animal in animales)
            {
                Console.WriteLine(animal.QuienSoy());
                Console.WriteLine(animal.ComoSoy());
            }

            Console.ReadLine();
        }
    }
```
# LINQ

**LINQ (Language Integrated Query)** es una sintaxis de consulta estructurada construida en C# y VB.NET que se utiliza para guardar y recuperar datos de diferentes tipos de fuentes de datos: una colección de objetos, una base de datos, XML o un servicio web.

Ejemplos. Creamos una clase `Students` y una colección de los mismos: 
```csharp
 public class Student
 {
    public string Name { get; set; }
    public int Average { get; set; }
 }

  var students = new List<Student>
  {
    new Student {Name = "Juan", Average = 4},
    new Student{Name = "Maria", Average = 8},
    new Student {Name = "Carlos", Average = 6}
  };
```
Ejemplo 1 - Select. 
```csharp
    var studentsName = students.Select(s => s.Name);

    foreach (var name in studentsName)
        Console.WriteLine(name);
    
    Console.ReadLine();
```
Vemos en este ejemplo que la variable `studentsName` es de tipo `IEnumerable`.

Hay un método que es el `ToList()` o el `ToArray()` que muchos desarrolladores usan tras cada consulta Linq, y esto no es una buena práctica ya que afecta al rendimiento de la aplicación, pues estas almacenando en memoria. Asi que usalo solo cuando sea necesario!

Ejemplo 2 - Where
```csharp
var approvedStudents = students.Where(x => x.Average >=5);
```
En este caso obtenemos un listado de todos los estudiantes que han aprobado, si queremos que nos devuelva sólo los nombres hacemos un select sobre el resultado:
```csharp
var approvedStudents = students.Where(x => x.Average >=5).Select(x => x.Name);
```
Ejemplo 3 - OrderBy
```csharp
var studentsWithWorstAverage = students.OrderBy(x => x.Average).Select(x => x.Name);            
Console.WriteLine("Ascending Ordered");
foreach (var name in studentsWithWorstAverage)
    Console.WriteLine(name);

Console.WriteLine("Descending Ordered");
var studentsWithBestAverage = students.OrderByDescending(x => x.Average).Select(x => x.Name);
foreach (var name in studentsWithBestAverage)
    Console.WriteLine(name);
```

Ejemplo 4 - First/Last
Podemos obtener el primer elemento y el último de la colección
```csharp
var firstStudent = students.First();
Console.WriteLine($"First student {firstStudent.Name}");
var lastStudent = students.Last();
Console.WriteLine($"Last student {lastStudent.Name}");
```

Ejemplo 5 - Sum
Podemos sumar la colección:
```csharp
var sumAverage = students.Sum(x => x.Average);
Console.WriteLine(sumAverage);
```

Ejemplo 6 - Max/Min
Nos permite obtener el mayor y el menor valor de la colección
```csharp
var maxAverage = students.Max(x => x.Average);
Console.WriteLine(maxAverage);
var minAverage = students.Min(x => x.Average);
Console.WriteLine(minAverage);
```

Ejemplo 7 - Average
Nos devuelve la media de la colección
```csharp
var average = students.Average(x => x.Average);
Console.WriteLine(average);
```

Ejemplo 8 - All/Any
Sirve para comprobar si todos o algunos de los valores de la colección cumplen una condición
```csharp
var allApproved = students.All(x => x.Average >= 5);
Console.WriteLine(allApproved);
var anyApproved = students.Any(x => x.Average >= 5);
Console.WriteLine(anyApproved);
```
