# Ejercicios

A partir del ejemplo anterior `06-more-queries` se quiere montar un portal de administración de las películas.

Para ello, necesitamos las siguientes características:

- Ver listado de películas con los campos: `id`, `title`, `released`, `plot`, `poster`.
- Al navegar al detalle de una de ellas, se quiere poder editar los campos: `id`, `title`, `released`, `plot`, `poster`, `cast`, `directors`, `imdbRating`, `tomatoesRating`.
- Además, se quiere poder insertar y borrar películas.
- Por último, se quiere un apartado de comentarios donde listarlos con los campos: `id`, `name`, `email`, `movie`, `text`, `date`
- E insertar un comentario.

*Donde `movie` es un objeto con los campos:

```typescript
interface Movie {
  id: string;
  title: string;
  imdbRating: number;
  tomatoesRating: number;
}
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
