# Utilizando Anotaciones de datos

Vamos a ver cómo crear y configurar nuestro modelo de datos utilizando _DataAnnotations_, o Anotaciones de Datos.

Ya hemos visto cómo configurarlo utilizando la API fluida o _Fluent API_ dentro de la sobreescritura del método _OnModelCreating_. Sin embargo, en el caso de _DataAnnotations_, la configuración reside en las propias entidades. Decorándose dichas clases o las prioridades de esas clases con atributos.

Por ejemplo, abrimos el proyecto _LibraryManagerWeb_, y la clase _AuditEntry_.

<img src="./content/audit-entry.png" style="zoom:80%">

Vamos a decorar la clase dos atributos, _Table_, que va a determinar el nombre de la tabla, y _Comment_ que va a añadir un comentario a esta tabla en base de datos.

```diff
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
+ [Table("AuditEntries")]
+ [Comment("Tabla con las entradas de auditoría de la biblioteca")]
  public class AuditEntry
  {

   public int AuditEntryId { get; set; }

   public DateTime Date { get; set; }

   public required string OPeration { get; set; }

   public required string ExtendedDescription { get; set; }
  
   public required string UserName { get; set; }

   public required string IpAddress { get; set; }
   ... 
 }
}
```

Tenemos otras entidades, por ejemplo, _Author_. Dentro de Author vamos a decorar la clase también con _Table_ y con _Comment_, pero además, introducimos otras _DataAnnotations_ que ya veremos en profundidad más adelante, como pueden ser _Key_, _DatabaseGenerated_, _Required_, _MinLength_ o _MaxLength_, etc.

```csharp
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
 [Table("Authors")]
 [Comment("Tabla para almacenar los autores que tienen libros en la biblioteca.")]
 public class Author
 {

  [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int AuthorId { get; set; }

  [Required]
  [MinLength(3), MaxLength(50)]
  public string Name { get; set; }

  [Required]
  [MinLength(3), MaxLength(100)]
  public string LastName { get; set; }

  public List<Book> Books { get; set; } = new List<Book>();

 }
}
```

Como ves, todas estas configuraciones se están introduciendo dentro de la entidad. Hay muchísimas _DataAnnotations_, que podemos ver en la documentación de Entity Framework Core, sin embargo, la API fluida es bastante más potente, entre otras cosas nos permiten añadir datos o asignar entidades a funciones o vistas.

Una recomendación sería utilizar la API Fluida, porque parece más legible. Y ya que tenemos que incluir alguna lógica normalmente dentro de un _ModelCreating_, ¿por qué no hacerlo todo aquí? Sin embargo, como para gustos, colores, aquí tenemos ambas alternativas.
