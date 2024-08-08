# Tipos de datos de columnas

En Entity Framework Core, cuando actualicemos una base de datos en función de nuestro modelo en .NET, el tipo de dato de las columnas generadas dependerá del proveedor de base de datos y también del tipo de dato en .NET. Por ejemplo, en SQL Server, una propiedad de tipo _string_ se convertirá en una columna de tipo _nvarchar(max)_, aunque esto dependerá de si la propiedad tiene algún tipo de restricción de tamaño, en cuyo caso, en lugar de ser _nvarchar(max)_, será _nvarchar_ y el tamaño máximo.

También podría depender de si esa propiedad pertenece a una clave principal, en cuyo caso, por ejemplo, sería _nvarchar(450)_. Sin embargo, este comportamiento se puede sobrescribir. Vamos a verlo con un ejemplo. 

Abrimos el proyecto _LibraryManagerWeb_. Queremos que el nombre de mi autor, en lugar de ser un _nvarchar(max)_, que es el tipo de dato que generará por defecto, que sea un _nvarchar(200).

Como casi todo, en Entity Framework Core, se puede hacer de dos formas, con la _Fluent API_ o con _Data Annotations_. Comencemos con _Data Annotations_.

```diff
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
 [Comment("Tabla para almacenar los autores que tienen libros en la biblioteca.")]
 public class Author
 {

  public int AuthorId { get; set; }

+ [Column(TypeName = "nvarchar(200")]
  public required string Name { get; set; }

  public required string LastName { get; set; }

  public List<Book> Books { get; set; } = new();

  [NotMapped]
  public DateTime LoadedDate { get; set; }

 }
}
```

Ahora imagina que queremos hacer lo mismo, pero en otra clase, por ejemplo, en la clase _BookRating_. Aquí queremos forzar que la propiedad _Stars_ sea decimal, de escala 4 y precisión 2. Vamos a hacerlo con _Fluent API_. ¿Dónde lo hacemos? Pues como siempre, en el contexto. Abrimos _LibraryContext_ y vamos al método _OnModelCreating_

```diff
-   modelBuilder.Entity<BookRating>()
-  .HasData(new[]
+  var bookRatingEntity = modelBuilder.Entity<BookRating>();
+  bookRatingEntity.Property(p => p.Stars).HasColumnType("decimal(2,4");
+  bookRatingEntity.HasData(new[]
    {
        new BookRating { BookRatingId = 1, BookId = 1, Username = "juanjo", Stars = 5 },
        new BookRating { BookRatingId = 2, BookId = 1, Username = "Lola", Stars = 3 },
        new BookRating { BookRatingId = 3, BookId = 1, Username = "Silvia", Stars = 4 },
        new BookRating { BookRatingId = 4, BookId = 1, Username = "Diego", Stars = 2 },
        new BookRating { BookRatingId = 5, BookId = 2, Username = "juanjo", Stars = 4 },
        new BookRating { BookRatingId = 6, BookId = 2, Username = "Lola", Stars = 2 },
        new BookRating { BookRatingId = 7, BookId = 2, Username = "Silvia", Stars = 5 },
        new BookRating { BookRatingId = 8, BookId = 2, Username = "Diego", Stars = 5 }
    });
```

¿Por qué no se puede utilizar aquí la concatenación de métodos? Porque _Property_ no devuelve un _EntityTypeBuilder_, por lo cual no podríamos aplicar directamente el _HasData_ a la devolución de _Property_, por eso he utilizado una variable, para no tener que llamar más veces a _modelBuilder.Entity_ de _BookRating_.

Vamos a compilar y ahora vamos a la consola a crear nuestra migración. Abrimos una consola en el directorio del proyecto, y ejecutamos:

```shell
dotnet ef migrations add ChangedColumnTypes
```

Volvemos a Visual Studio, vamos a _Migrations_ y buscamos _ChangedColumnTypes_. En el Up tenemos _AlterColumn Stars_, en _BookRatings_, indicando el tipo como _decimal(2,4)_ y la versión anterior era _int_. Y hay otra alteración de columna, _Name_ en _Authors_, el tipo es _nvarchar(200)_,  y la versión anterior era _nvarchar(max)_. Y aquí lo tenemos, hemos cambiado el tipo de dos columnas mediante Fluent API y mediante Data Annotations.

<img src="./content/change-columns-type.png" style="zoom:80%">