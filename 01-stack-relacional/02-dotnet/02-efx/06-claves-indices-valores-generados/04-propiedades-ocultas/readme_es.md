# Propiedades ocultas

Vamos a qué son exactamente las _shadow properties_ en Entity Framework Core. Una _shadow property_, o propiedad oculta, es una columna que existe en la base de datos pero que no tiene una equivalencia en nuestro objeto .NET. De forma automática, esto ocurre, por ejemplo, con las relaciones.

Vamos a verlo con un ejemplo, estamos en la aplicación _LibraryManagerWeb_, y abrimos la entidad _AuditEntry_.

<img src="./content/audit-entry.png" style="zoom:80%">

Tenemos _CountryId_ y a continuación _Country_, por convención, esto significa que nuestra entidad _AuditEntry_ tiene una relación con un país y se está utilizando este campo _CountryId_ para establecer dicha relación. ¿Pero qué ocurre si nosotros ahora guardamos otra relación? Decimos que nuestra _AuditEntry_ puede tener una relación con un libro.

***./DataAccess/AuditEntry.cs***

```diff
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
 public class AuditEntry
 {

  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int AuditEntryId { get; set; }

  public DateTime Date { get; set; }

  [Required]
  public string OPeration { get; set; }

  public decimal TimeSpent { get; set; }

  public string ExtendedDescription { get; set; }
  
  public string UserName { get; set; }

  public string IpAddress { get; set; }

  public int CountryId { get; set; }

  public Country Country { get; set; }

  public string City { get; set; }

  public double Latitude { get; set; }

  public double Longitude { get; set; }

  public string ISP { get; set; }

  public string UserAgent { get; set; }

  public string OperatingSystem { get; set; }

+ public Book Book { get; set; }
 }
}
```

Lo hemos añadido, pero no estamos añadiendo ningún campo para _BookId_. ¿Qué ocurre? Que Entity Framework Core automáticamente va a crear esta propiedad porque la necesita como _foreign key_, en SQL Server llamada _BookId_.

También se establece por convención el tipo de la entidad relacionada y el identificador, en este caso, _BookId_. Por tanto, cuando creemos esta entidad _AuditEntry_ en la base de datos se generará una columna llamada _BookId_, aunque este _BookId_ no esté disponible como propiedad en nuestra entidad.

Aparte también podemos crear nosotros propiedades ocultas personalizadas, por ejemplo, vamos a nuestro contexto de base de datos. Estamos en _LibraryContext_, y en _AuditEntry_ vamos a añadir una propiedad oculta llamada ResearchTicketId, por ejemplo, un ticket abierto para investigar algo que ha ocurrido en nuestra auditoría.

***./DataAccess/LibraryContext.cs***

```diff
   var auditEntryEntity = modelBuilder.Entity<AuditEntry>();

   auditEntryEntity.Property(p => p.TimeSpent)
    .HasPrecision(20);
   auditEntryEntity.Property(p => p.IpAddress).IsRequired();
+  auditEntryEntity.Property<string>("ResearchTicketId").HasMaxLength(20);
```

Esto va a hacer que se añada una propiedad adicional en _AuditEntry_ llamada _ResearchTicketId_, pero no va a tener una representación como tal en nuestra entidad.

Y entonces, ¿cómo actualizamos esa propiedad? Cuando añadamos una nueva auditoría en nuestra base de datos. Vamos a verlo en nuestro _HomeController_.

***./Controllers/HomeController.cs***

```diff
  public async Task<IActionResult> Index()
  {
-  var mostHighlyRatedBooks = await _context.MostHighlyRatedBooks.ToListAsync();
-  var proliphicAuthors = await _context.ProliphicAuthors.ToListAsync();

+  var firstBook = await _context.Books.FirstOrDefaultAsync();
+  var newAuditEntry = new AuditEntry
+  {
+   Book = firstBook,
+   Date = DateTime.UtcNow,
+   CountryId = 1,
+   ExtendedDescription = "Prueba de entrada de auditoría",
+   OPeration = "Reseñar un libro",
+   IpAddress = "83.22.121.44",
+   UserName = "JuanjoMontiel"
+  };
+  await _context.AuditEntries.AddAsync(newAuditEntry);
+  _context.Entry(newAuditEntry).Property("ResearchTicketId").CurrentValue = "abcdefghijklmnopqrst";
+  await _context.SaveChangesAsync();

   return View();
  }
```

Y si queremos sacar la _AuditEntry_ cuyo _ResearchTicketId_ es el que hemos añadido. ¿Cómo lo hacemos?

***./Controllers/HomeController.cs***

```diff
  public async Task<IActionResult> Index()
  {
   var firstBook = await _context.Books.FirstOrDefaultAsync();
   var newAuditEntry = new AuditEntry
   {
    Book = firstBook,
    Date = DateTime.UtcNow,
    CountryId = 1,
    ExtendedDescription = "Prueba de entrada de auditoría",
    OPeration = "Reseñar un libro",
    IpAddress = "83.22.121.44",
    UserName = "JuanjoMontiel"
   };
   await _context.AuditEntries.AddAsync(newAuditEntry);
   _context.Entry(newAuditEntry).Property("ResearchTicketId").CurrentValue = "abcdefghijklmnopqrst";
   await _context.SaveChangesAsync();
+  var entry = await _context.AuditEntries.SingleOrDefaultAsync(e => EF.Property<string>(e, "ResearchTicketId") == "abcdefghijklmnopqrst");

   return View();
  }
```
