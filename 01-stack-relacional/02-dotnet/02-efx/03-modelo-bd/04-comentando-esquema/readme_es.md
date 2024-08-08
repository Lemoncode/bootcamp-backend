# Añadir comentarios en nuestro modelo

Igual que tenemos que añadir comentarios a nuestro código para facilitarle la vida a otros desarrolladores o a nuestro yo del futuro, también es conveniente añadir comentarios en el propio esquema de la base de datos por las mismas razones. Si añadimos comentarios, podremos saber para qué sirve una determinada tabla o una determinada columna. ¿Cómo podemos hacer esto con Entity Framework Core? Pues de dos formas, como viene siendo habitual, utilizando _Data Annotations_ o _Fluent API_.

Vamos a abrir la clase de entidad _Books_ y utilizamos el atributo _Comment_ para añadir un comentario diciendo que esta tabla sirve para añadir los libros de la biblioteca.

```diff
+ using Microsoft.EntityFrameworkCore;

namespace LibraryManagerWeb.DataAccess
{
+   [Comment("Tabla para almacenar los libros existentes en esta biblioteca")]
    public class Book
    {

        public int BookId { get; set; }

        public int AuthorId { get; set; }

        public required Author Author { get; set; }

        public required string Title { get; set; }

        public string? Sinopsis { get; set; }

        public required List<BookFile> BookFiles { get; set; }

        public int PublisherId { get; set; }

        public required Publisher Publisher { get; set; }

        public List<BookRating> Ratings { get; set; }
    }
}
```

También como alternativa podemos usar la _Fluent API_. Vamos abrir la clase _LibraryContext_, utilizando la sobrescritura de OnModelCreating.

```diff
...
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
+               .HasComment("Tabla para almacenar los autores que tienen libros en la biblioteca.")
                .HasData(new[]
            {
                new Author { AuthorId = 1, Name = "Stephen", LastName = "King" },
                new Author { AuthorId = 2, Name = "Isaac", LastName = "Asimov" }
            });
...
```

En el método _modelBuilder.Entity_ de tipo Author utilizamos _.HasComment_ y añadimos el comentario de nuestra clase. Cuando creemos la migración con estos cambios, se creará una instrucción para añadir el comentario en nuestra base de datos.

```console
Add-Migration AddedCommentsInSchema
```

Ya tenemos la migración creada, en AddedCommentsInSchema, podemos ver que el método _Up_ está alterando la tabla _Publishers_ para añadir el comentario, la tabla _Books_ y la tabla _Authors_.

<img src="./content/add-comments-schema.png" style="zoom:80%" alt="CAptura con la nueva migración en la que se añaden los comentarios a nustro esquema.">