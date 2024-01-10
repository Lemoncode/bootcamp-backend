# Utilizando la API Fluida

Anteriormente, hemos visto que a la hora de crear nuestro modelo podemos utilizar dos enfoques diferentes, o bien utilizar la API fluida de Entity Framework Core, o bien utilizar las Data Annotations en las propias clases de entidad.

Vamos a ver exactamente qué es la API fluida. Abrimos el proyecto _LibraryManagerWeb_ y dentro de nuestro contexto, que hereda de DbContext, tenemos el método _OnModelCreating_. Este método recibe un parámetro que se llama _modelBuilder_ que es de tipo _ModelBuilder_.

<img src="./content/model-builder.png" style="zoom:80%" alt="Vista del contexto con el método OnModelCreating.">

Con este _modelBuilder_ podemos configurar todo nuestro modelo utilizando código C#. ¿Pero por qué se llama esto API fluida? Pues API fluida es un concepto que nos permite configurar algo, en este caso una entidad, concatenando llamadas a diferentes métodos. Vamos a configurar la entidad Autor.

Si nos fijamos en el código, se está asociando a la variable _Author_ la llamada al método _Entity_ de _modelBuilder_. _Entity_, que es un método genérico al cual le pasamos el tipo de entidad que estamos configurando, en este caso, _Author_.

```csharp
...
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   modelBuilder.Entity<Author>()
    .HasComment("Tabla para almacenar los autores que tienen libros en la biblioteca.")
    ...
```

Una vez tenemos guardada en la variable la devolución de este _Entity_, que es de tipo _EntityTypeBuilder_, comenzamos a configurar la entidad. Decimos _author.HasComment_ y decimos el comentario que asignamos a esta tabla. Este _HasComment_ está devolviendo el propio _EntityTypeBuilder_ y ahora sobre este _EntityTypeBuilder_ que ya tiene su comentario, seguimos aplicando configuraciones. Por ejemplo, vamos a utilizar _.ToTable_ para indicarle que la tabla se va a llamar _Authors_.

```diff
...
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   modelBuilder.Entity<Author>()
    .HasComment("Tabla para almacenar los autores que tienen libros en la biblioteca.")
+   .ToTable("Authors")
    ...
```

Ahora sobre este _EntityTypeBuilder_ que ya tiene el comentario y el nombre de la tabla, seguimos añadiendo configuraciones.

```diff
...
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   modelBuilder.Entity<Author>()
    .HasComment("Tabla para almacenar los autores que tienen libros en la biblioteca.")
    .ToTable("Authors")
+   .HasMany(p => p.Books)
+   .WithOne(b => b.Author)
    ...
```

_HasMany_ y _WithOne_, vamos a configurar las relaciones, esto ya lo veremos más adelante, le decimos que la propiedad _Books_ tiene una relación de _Many_ con _Authors_, es decir, un autor tiene muchos libros, y por otro lado, que cada libro tiene un solo autor.

```diff
...
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   modelBuilder.Entity<Author>()
    .HasComment("Tabla para almacenar los autores que tienen libros en la biblioteca.")
    .ToTable("Authors")
    .HasMany(p => p.Books)
    .WithOne(b => b.Author)
+   .IsRequired(true)
    ...
```

Además, esta relación es requerida.

En conclusión, la API fluida es una forma muy elegante de configurar nuestras entidades. De hecho, con la API fluida se pueden hacer algunas cosas más que con las Data Annotations. Por ejemplo, asociar una entidad a una vista o a una función o añadir datos iniciales, con el método _HasData_.
