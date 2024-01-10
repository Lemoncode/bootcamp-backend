# Precisión y escala

Desde la version 5.0 de Entity Framework Core ahora también podemos modificar la precisión de algunos tipos de columna, por ejemplo, de _DateTime_ o _Decimal_.

Estamos aquí en el proyecto _LibraryManagerWeb_. Vamos a la clase _AuditEntry_ donde guardamos todas la auditoría de las operaciones que se realizan en nuestro sitio web. Aquí vamos a añadir una nueva propiedad.

```diff
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
 public class AuditEntry
 {

  public int AuditEntryId { get; set; }

  public DateTime Date { get; set; }

  public string OPeration { get; set; }

+ public decimal TimeSpent { get; set; }

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
 }
}

```

Donde se va a guardar el tiempo que ha tardado la operación en finalizar. Ahora vamos a cambiar la precisión de este campo. En SQL Server ese campo se traducirá en una columna de tipo _decimal_ y mediante la API fluida, (aún no se puede hacer esto con _Data Annotations_) podemos cambiar esa precisión. ¿Cómo lo hacemos? Pues en _OnModelCreating_ vamos a añadir este cambio:

```diff

+  modelBuilder.Entity<AuditEntry>()
+   .Property(p => p.TimeSpent)
+   .HasPrecision(20);

   base.OnModelCreating(modelBuilder);
```

Con este cambio indicamos que el campo _TimeSpent_ va a poder guardar 20 dígitos. Guardamos (Control+Shift+S) y compilamos. Y ahora vamos a la consola a crear la migración.

```shell
dotnet ef migrations add ChangedPrecission
```

Vamos a Visual Studio para ver qué es lo que ha generado en _Migrations_ y _ChangedPrecission_. Podemos ver como añade la columna e indica que la precisión va a ser 20.

 <img src="./content/change-precision.png" style="zoom:80%" alt="Migración con las instrucciones para crear el nuevo campo.">