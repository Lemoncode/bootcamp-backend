# LINQ

**LINQ (Language Integrated Query)** es una sintaxis de consulta estructurada construida en C# y VB.NET que se utiliza para guardar y recuperar datos de diferentes tipos de fuentes de datos: una colección de objetos, una base de datos, XML o un servicio web.

¿Qué ventaja tenemos al usar LINQ?

- Uniformidad en lenguajes de consulta.
- Reducción de código.
- Código más legible.
- Integración en C# (Intellisense disponible)

Vamos a empezar con un ejemplo muy simple para ver las ventajas que nos ofrece LINQ. Nos planteamos un pequeño ejercicio: Partiendo de un array de enteros vamos a calcular la suma y la creación de un nuevo array con los números pares.

```csharp
var valores = new List<int> {1,2,3,4,5,6,7,8,9};
var pares = new List<int>();
var suma = 0;
foreach (var valor in valores)
{
    suma += valor;
    if (valor % 2 == 0)
    {
        pares.Add(valor);
    }
}
```

Ahora vamos a plantearlo con LINQ:

```csharp
var valores = new List<int> {1,2,3,4,5,6,7,8,9};
var suma = valores.Sum();
var pares = valores.Where(x => x % 2 == 0).ToList();
```

¿Veis la mejora en el código? Con LINQ utilizamos un lenguaje más natural y nos ayuda a eliminar bucles que nos "ensucian" el código. Ahora que ya conocemos las ventajas que nos aporta LINQ, vamos a ver algunos de los métodos que podemos utilizar. Para ello vamos a partir de la siguiente colección de objetos:

```csharp

    public class Student
    {
        public string Name { get; set; }
        public int Average { get; set; }
        public string Classroom { get; set; }
    }

    static void Main(string[] args)
        {
            var students = new List<Student>
            {
                new Student { Name = "Juan", Average = 4, Classroom = "1A" },
                new Student { Name = "Maria", Average = 8, Classroom = "1A" },
                new Student { Name = "Carlos", Average = 6, Classroom = "1A" },
                new Student { Name = "Pedro", Average = 3, Classroom = "1B" },
                new Student { Name = "Sandra", Average = 9, Classroom = "1B" }
            };
        }
.....
```

## Api LINQ

Vamos a ver los métodos de extensión que nos proporciona LINQ. Lo primero que tenemos que hacer en nuestro código es añadir el *using* al *namespace*:

```csharp
using System.Linq;
```

- **Select**
    Este método nos va a permitir quedarnos con una propiedad concreta.

    En nuestro ejemplo vamos a crear un nuevo array solo con los nombre de los todos los alumnos.

    ```csharp
        var studentsName = students.Select(s => s.Name);
        
        foreach (var name in studentsName)
            Console.WriteLine(name);
    ```
    
- **Where**
    Con este método vamos a filtrar nuestra colección.

    En nuestro ejemplo vamos a quedarnos con los alumnos que tengan la media igual o mayor a cinco.

    ```csharp
        var studentsName = students.Where(s => s.Average >= 5);
        
        foreach (var student in studentsName)
            Console.WriteLine(student.Name);
    ```
    
    Otra solución que podemos hacer es concatenar los dos métodos _Where_ y _Select_ para directamente obtener el nombre del alumno con la media aprobada. 

    ```csharp
        var studentsName = students.Where(s => s.Average >= 5).Select(s => s.Name);
        
        foreach (var name in studentsName)
            Console.WriteLine(name);
    ```
    
- **OrderBy**
    Este método nos permite ordenar una colección por cualquiera de sus campos.

    En nuestro ejemplo queremos ordenar los alumnos por su nota media.

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

- **First/Last**
    Con estos métodos obtenemos el primer o último elemento.

    En nuestro ejemplo, vamos a obtener el alumno con más y menos nota media.

    ```csharp
    var studentsWithBestAverage = students.OrderByDescending(x => x.Average).Select(x => x.Name);
    Console.WriteLine($"Best: {studentsWithBestAverage.First()}");
    Console.WriteLine($"Worst: {studentsWithBestAverage.Last()}");
    ```
    
- **Sum**
    Como ya vimos en el primer ejemplo, este método nos permite sumar el campo deseado de todos los elementos de una colección.

    Para nuestro ejemplo, vamos a sumar las notas medias de todos los alumnos.

    ```csharp
    var sum = students.Sum(x => x.Average);
    Console.WriteLine(sum);
    ```
    
- **Count**
    Con este métodos podemos contar los elementos de una colección.

    Para nuestro ejemplo, vamos a ver cuántos alumnos tenemos matriculados.

    ```csharp
    var count = students.Count();
    Console.WriteLine($"El número de alumnos matriculados es: {count}");
    ```
    
- **Group**
    Bueno llegamos a un método más interesante, con este método obtenemos una nueva colección con los elementos agrupados por el criterio indicado.

    Para nuestro ejemplo, nos interesaría conocer cuantas clases hay y el número de alumnos de cada unas de ellas.

    ```csharp
    var classrooms = students.GroupBy(x => x.Classroom);
    
    foreach(var classroom in classrooms)
        Console.WriteLine($"{classroom.Key} => {classroom.Count()}");
    ```

    Analizando el resultado del método GroupBy vemos nos devuelve una colección de tipo _IGrouping<string,Student>_, es decir, cada uno de los elementos de la nueva colección es una subcolección de _Student_ agrupada por una clave (key). La clave será cada uno de los valores diferentes del campo que hemos indicado.

- **Max/Min**
    Este método nos calcula el mayor o menor campo de una colección.

    En nuestro ejemplo, vamos a ver la nota media más alta y la más baja por cada una de las clases.

    ```csharp
    var classrooms = students.GroupBy(x => x.Classroom);
    
    foreach(var classroom in classrooms)
        Console.WriteLine($"{classroom.Key} => Best: {classroom.Max(x=>x.Average)}, Worst: {classroom.Min(x => x.Average)}");
    
    Console.ReadLine();
    ```

- **Average**
    Este método nos da la media de un campo determinado para la colección.

    En nuestro ejemplo, vamos a calcular la media por clase.

    ```csharp
    var classrooms = students.GroupBy(x => x.Classroom);
    
    foreach(var classroom in classrooms)
        Console.WriteLine($"{classroom.Key} => {classroom.Average(x=>x.Average)}");
    
    Console.ReadLine();
    ```

- **All/Any**
    Estos métodos nos permiten evaluar si todos o algunos de los elementos cumplen con una determinada condición.

    En nuestro ejemplo, queremos saber han aprobado todos o si ha aprobado algún alumno por cada una de las clases. 

    ```csharp
    var classrooms = students.GroupBy(x => x.Classroom);
    
    foreach(var classroom in classrooms)
        Console.WriteLine($"{classroom.Key} => All approved: {classroom.All(x=>x.Average >= 5)},  Any approved: {classroom.Any(x => x.Average >= 5)}");
    
    Console.ReadLine();
    ```

> Como ejercicio de clase se podría pedir que se obtuviese el nombre del alumno con mejor y peor nota de cada una de las clases.

## Sintaxis integrada

Aunque en los ejemplos anteriores hemos visto el uso directo de los métodos de extensión, otra de las grandes ventajas que tiene LINQ es que permite crear expresiones directamente en el código, de manera similar a si escribiésemos SQL directamente en C#. Por ejemplo:

```csharp
    var estudiantesAprobados = from student in students
                           where student.Average >= 5
                           orderby student.Average
                           select student.Name;

            foreach (var name in estudiantesAprobados)
                Console.WriteLine($"{name}");

            Console.ReadLine();
```

Esta consulta nos devolverá la lista de alumnos que tienen una nota superior a o igual a 5, ordenados por nota ascendentemente.

¿Cuál de las dos formas es mejor? Pues son exactamente igual, utiliza con la que te sientas más cómodo.

## Recomendaciones

Bueno, ya que hemos visto la gran ventaja que nos aporta LINQ, es hora de hablar de una desventaja: La optimización. Es cierto que si estamos en entorno donde tenemos que optimizar a nivel de milisegundos puede que LINQ salga mal parado por inserta una capa más de abstracción, pero por regla general la comparativa es prácticamente igual que realizando la operación con un bucle.

Eso sí, hay que tener siempre en cuenta los métodos de extension que ejecutan nuestras consulta. Vamos a ver esto con un ejemplo:

```csharp
    var approvedStudents = students.Where(x => x.Average >= 5);
    var approvedStudentsClassroom1A = approvedStudents.Where(x => x.Classroom == "1A");
    var approvedStudentsClassroom1AWithA = approvedStudentsClassroom1A.Where(x => x.Name.Contains("a"));
    var approvedStudentsClassroom1AWithASorted = approvedStudentsClassroom1AWithA.OrderBy(x => x.Average);

    var list = approvedStudentsClassroom1AWithASorted.ToList();

    foreach (var name in list)
        Console.WriteLine($"{name}");

    Console.ReadLine();
```

Si vemos los tipos devueltos en los métodos de extensión _Where_ son todos _IEnumerable_ o cuando llamamos al método _OrderBy_ son _IOrderedEnumerable_. Hasta este momento la consulta **NO SE HA HECHO**, por tanto podemos añadir cuantas condiciones queremos sin que se ejecute nada. Es cuando necesitamos el resultado en memoria cual se lanza la consulta, es decir con el método _ToList_, Sum, Average, ... Por tanto es recomendable tener claro cuando estás trabajando con datos en memoria o con una consulta no realizada, sobre todo cuando estemos atacando a una base de datos como origen.
