# Tiempo de vida de un contexto

Nuestro contexto de base de datos hereda de la clase DbContext, que implementa, entre otras muchas, dos interfaces (dentro de la interfaz _IdbContextPoolable_): _IDisposable_ y _IAsyncDisposable_. Estas dos interfaces nos van a asegurar de que cuando finalicemos el contexto y hagamos un dispoise, todos los objetos relacionados con el contexto, como por ejemplo la conexión con la base de datos, se van a cerrar adecuadamente.

Esto viene muy relacionado con la duración de nuestro contexto, que normalmente es muy breve. La duración del contexto suele asociarse a una unidad de trabajo, o _unit of work_, es decir, a una transacción de negocio con nuestra base de datos. 

Por ejemplo, vamos a eliminar el código que borraba los datos en nuestro _program.cs_ y se nos quedará el siguiente código:

***./Program.cs***
```csharp
using EFCoreReverseEngineering.Model;

using var context = new LibraryContext();
var author = new Author { Name = "Stephen", LastName = "King" };
var book = new Book { Title = "Los ojos del dragón", Author = author, Sinopsis = "Esta es la sonopsis del libro." };
context.Authors.Add(author);
context.Books.Add(book);
await context.SaveChangesAsync();
```

Analizando este código, lo primero que vemos es que creamos el contexto con la palabra reservada _using_. A continuación estamos creando un autor y un libro y los estamos añadiendo al contexto. El siguiente paso es persistir los cambios, y de forma implicita, puesto que hemos usado _using_, el contexto se va a eliminar completamente.

La duración del contexto dependerá del tipo de aplicación que estemos usando. A continuación, vamos a ver la duración del contexto para una aplicación ASP.NET Core, pero puede haber otras aproximaciones para una aplicación de consola o una en Blazor.
