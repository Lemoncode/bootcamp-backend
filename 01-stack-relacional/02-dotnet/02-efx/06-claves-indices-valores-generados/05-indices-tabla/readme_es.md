# Índices en las tablas

En base de datos o realmente en casi cualquier tipo de sistema de almacenamiento existe el concepto de _Index_ o Índice. Los índices nos permiten buscar de manera más eficiente por una determinada propiedad. Por poner un símil en el mundo físico, imagínate el índice de un libro, en el que tenemos una página donde nos aparece el título de un capítulo y la página en la que se encuentra.

Con Entity Framework Core también podemos crear índices. Vamos a ver cómo se hace. Abrimos el proyecto _LibraryManagementWeb_ y vamos a la entidad _Author_. Los índices pueden hacer referencia a una propiedad o a varias. En este caso, vamos a usar dos campos para nuestro índice, la propiedad _Nombre_ y la propiedad _Apellidos_.

***./DataAccess/Author.cs***

```diff
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
 [Comment("Tabla para almacenar los autores que tienen libros en la biblioteca.")]
+[Index(nameof(Name), nameof(LastName))]
 public class Author
 {

  [Key]
  public int AuthorId { get; set; }

  [MaxLength(100)]
  public string Name { get; set; }

  [MaxLength(200)]
  public string LastName { get; set; }

  public string DisplayName { get; set; }

  public string AuthorUrl { get; set; }

  public List<Book> Books { get; set; } = new List<Book>();

  [NotMapped]
  public DateTime LoadedDate { get; set; }

 }
}
```

Normalmente, es una buena técnica crear índices en las propiedades que se van a usar más frecuentemente para realizar consultas.

Además, también podríamos indicarle si queremos que el índice sea único o no, es decir, que no pudiera haber valores repetidos de autores con el mismo nombre y apellidos. Lo indicar añadiendo después de los nombres coma _IsUnique = true_. En este caso, no vamos a hacerlo, puesto seguro que hay diferentes autores con los mismos nombres y apellidos.

¿Y cómo se crean índices utilizando Fluent API? Vamos a verlo, vamos a nuestro contexto en _LibraryContext_. Y en _onModelCreating_ vamos a ver cómo crear un índice para la propiedad _ResearchTicketId_ de _AuditEntry_, que recordemos además era una _Shadow Property_.

***./DataAccess/LibraryContext.cs***

```diff
   var auditEntryEntity = modelBuilder.Entity<AuditEntry>();

   auditEntryEntity.Property(p => p.TimeSpent)
    .HasPrecision(20);
   auditEntryEntity.Property(p => p.IpAddress).IsRequired();
   auditEntryEntity.Property<string>("ResearchTicketId")
    .HasMaxLength(20);
+  auditEntryEntity.HasIndex("ResearchTicketId")
+   .IsUnique(true)
+   .HasName("UX_AuditEntry_ResearchTicketId");
```

Hemos creado un índice en la propiedad _ResearchTicketId_, indicándole que es único y cambiando el nombre por efecto a _UX_AuditEntry_ResearchTicketId_
