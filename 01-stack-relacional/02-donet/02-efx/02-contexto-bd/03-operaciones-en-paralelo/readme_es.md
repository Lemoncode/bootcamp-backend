# Evitando errores con múltiples operaciones en paralelo

Los contextos de base de datos de Entity Framework Core no son seguros para subprocesos. Eso significa que un solo contexto no se puede utilizar con diferentes hilos en operaciones paralelas. Vamos a ver esto con un ejemplo.
Vamos a abrir la aplicación de consola _FirstEFCoreConsoleApp_ y modificamos _Program.cs_

***./Program.cs***

```diff
using FirstEFCoreConsoleApp;
using FirstEFCoreConsoleApp.Model;

- using var context = new LibraryContext();
- context.Database.EnsureCreated();
- var author = new Author { Name = "Stephen", LastName = "King" };
- var book = new Book {Title = "Los ojos del dragón", Sinopsis = "Prueba de un libro", Author = author };
- context.Authors.Add(author);
- context.Books.Add(book);
- await context.SaveChangesAsync();
- var bookFromDb = context.Books.FirstOrDefault(b => b.Title == "Los ojos del dragón");
- Console.WriteLine(bookFromDb?.Title);

+ try
+ {
+    using var context = new LibraryContext();
+    var authors = new[]
+    {
+        new Author { Name = "Robert", LastName = "Cecil Martin" },
+        new Author { Name = "Martin", LastName = "Fowler" }
+    };

+    var tasks = new List<Task>();
+    for (var i = 0; i < authors.Length; i++)
+    {
+        var author = authors[i];
+        tasks.Add(Task.Run(async () =>
+        {
+            await context.AddRangeAsync(author);
+            await context.SaveChangesAsync();
+            context.Authors.Remove(author);
+            await context.SaveChangesAsync();
+        }));
+    }
+    await Task.WhenAll(tasks);
+ }
+ catch (Exception ex)
+ {
+    Console.WriteLine(ex.Message);
+ }
```

Partimos de la aplicación de consola _LibraryManager_ y vamos a hacer algo prohibido. Creamos el contexto con  _using var context_, y luego creamos un array de autores. Vamos a crear también una lista de tareas para hacer la simulación de ejecución en paralelo.

Empezamos a recorrer la lista de autores y por cada autor añadimos una tarea a nuestra lista de tareas usando un _Task.Run_ y una lambda que lo que va a hacer es añadir el autor al contexto, persistir los cambios, luego eliminarlo y volver a persistir los cambios.

Finalmente, al acabar el for, esperamos, con un await _Task.WhenAll_, que todas las tareas finalicen. ¿Qué está ocurriendo aquí? Pues aquí esta ocurriendo que estamos usando un solo contexto para ejecutar ene operaciones en paralelo. En este caso dos operaciones, porque nuestro array tiene solo dos autores. Vamos a meter todo nuestro código en un bloque _try&catch_, porque posiblemente la ejecución nos va a lanzar una excepción.

Bien, vamos a poner un punto de parada en el bloque _catch_ e intentamos ejecutar el código con _F5_. Bueno, se ha parado en el _catch_, vamos a ver la excepción. El mensaje nos está diciendo que una operación que estaba en curso no se ha finalizado antes de que empiece otra. Bien, ¿cómo podríamos arreglar esto? De dos formas. O bien eliminando el paralelismo o bien utilizando un contexto por cada una de las tareas. Vamos a hacer lo segundo:

***./Program.cs***

```diff
using FirstEFCoreConsoleApp;
using FirstEFCoreConsoleApp.Model;

try
{
-   using var context = new LibraryContext();
    var authors = new[]
    {
        new Author { Name = "Robert", LastName = "Cecil Martin" },
        new Author { Name = "Martin", LastName = "Fowler" }
    };

    var tasks = new List<Task>();
    for (var i = 0; i < authors.Length; i)
    {
        var author = authors[i];
        tasks.Add(Task.Run(async () =>
        {
+           using var context = new LibraryContext();            
            await context.AddRangeAsync(author);
            await context.SaveChangesAsync();
            context.Authors.Remove(author);
            await context.SaveChangesAsync();
        }));
    }
    await Task.WhenAll(tasks);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
```

Ejecutamos la aplicación con _F5_ y ahí lo tenemos. Ha finalizado sin ningún tipo de error. ¿Por qué? Porque pese a que estamos usuando paralelismo, hemos creado un contexto para cada una de las tareas. Por tanto, un solo contexto no puede ser utilizado por más de un hilo al mismo tiempo.
