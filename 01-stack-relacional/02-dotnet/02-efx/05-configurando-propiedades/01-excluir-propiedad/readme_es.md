# Incluyendo y excluyendo propiedades

Al añadir una nueva entidad a nuestro modelo, cualquier propiedad pública que tenga un _get_ y un _set_ será añadida a dicha entidad. Sin embargo, podemos sobrescribir este comportamiento utilizando tanto API fluida como anotaciones de datos. Vamos a verlo con un ejemplo.

Abrimos el proyecto _LibraryManagerWeb_, en la entidad Author. Imagina, por ejemplo, que queremos añadir un campo que se va a llamar _LoadedDate_, donde vamos a almacenar la fecha en la que se ha cargado la información del autor, pero solo queremos guardar esta información en memoria, no nos interesa persistir esto en base de datos, ya que es un valor totalmente dinámico y que no tiene sentido guardar. 

Si directamente hacemos:

```diff
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
 public class Author
 {

  public int AuthorId { get; set; }

  public required string Name { get; set; }

  public required string LastName { get; set; }

  public List<Book> Books { get; set; } = new List<Book>();

+ public DateTime LoadedDate { get; set; }

 }
}
```

Por convención esta propiedad se va a crear en nuestra tabla de base de datos. ¿Cómo puedo indicar a Entity Framework Core que yo no quiero esto en mi base de datos? Pues primero vamos a hacerlo con anotaciones, con el atributo _NotMapped_.

```diff
using System;
using System.Collections.Generic;
+using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
 public class Author
 {

  public int AuthorId { get; set; }

  public required string Name { get; set; }

  public required string LastName { get; set; }

  public List<Book> Books { get; set; } = new List<Book>();

+ [NotMapped]
  public DateTime LoadedDate { get; set; }

 }
}
```

Y ahora, en lugar de hacerlo por _Data Annotations_ vamos a hacerlo por _Fluent API_. Nos vamos al contexto, y dentro del método _OnModelCreating_, añadimos el siguiente código:

```diff
   modelBuilder.Entity<Book>()
+   .Ignore(p => p.LoadedDate)
    .HasData(new[]
    {
        new Book { BookId = 1, AuthorId = 1, Title = "Los ojos del dragón", Sinopsis = "El libro \"Los ojos del dragón\".", PublisherId = 1 },
        new Book { BookId = 2, AuthorId = 1, Title = "La torre oscura I", Sinopsis = "Es el libro \"La torre oscura I\"." , PublisherId = 1 },
        new Book { BookId = 3, AuthorId = 2, Title = "Yo, robot", Sinopsis = "Es el libro \"Yo, robot\".\"." , PublisherId = 1 }
    });
```

Compilamos, y vámonos a la consola para ver qué pasa si creamos una migración nueva. Estamos en el directorio de CSPROJ y ejecutamos el siguiente comando

```shell
dotnet ef migrations add AddedLoadedDate
```

Si esto funciona como espero que funcione, la migración debería estar vacía, puesto que los dos campos han sido ignorados, uno con _Fluent API_ y el otro con _Data Annotations_. Como hemos dicho antes, las mezclas nunca son buenas. Lo ponemos solo como ejemplo, pero en el mundo real o usas _Data Annotations_ o _Fluent API_ para la configuración del modelo.

Vamos a Visual Studio, Migrations, y en la migración _AddedLoadedDate_ comprobamos que está completamente vacía. Podremos usar el campo para añadir información en memoria, pero no se va a guardar en base de datos ni el campo va a existir en la tabla.

<img src="./content/no-property-add.png" style="zoom:80%">