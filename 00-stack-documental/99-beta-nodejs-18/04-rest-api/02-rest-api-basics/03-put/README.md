# 03 Put method

In this example we are going to implement a `put` method using http server in Nodejs.

We will start from `02-post`.

# Steps to build it

Now, we can implement the route for update existing book. We could apply same concepts previously learned in `POST` and use it in `PUT` method:

_./index.mjs_

```diff
import http from "http";
- import { getBookList, getBook, insertBook } from "./mock-db.mjs";
+ import { getBookList, getBook, insertBook, updateBook } from "./mock-db.mjs";

const handleRequest = (req, res) => {
...
- } else if (/\/api\/books\/\d$/.test(url) && method === "GET") {
-   const [, bookId] = url.match(/\/api\/books\/(\d)$/);
+ } else if (/\/api\/books\/\d$/.test(url)) {
+   const [, bookIdString] = url.match(/\/api\/books\/(\d)$/);
+   const bookId = Number(bookIdString);
+   if (method === "GET") {
      res.setHeader("Content-Type", "application/json");
      res.statusCode = 200;
-     const book = getBook(Number(bookId));
+     const book = getBook(bookId);
      res.write(JSON.stringify(book));
      res.end();
+   } else if (method === "PUT") {
+     let data = [];
+     req.on("data", (chunk) => {
+       console.log({ chunk });
+       data += chunk;
+     });

+     req.on("end", () => {
+       const book = JSON.parse(data.toString("utf8"));
+       updateBook(bookId, book);
+       res.statusCode = 204;
+       res.end();
+     });
+   }
  } else {
    res.write("My awesome books portal");
    res.end();
  }
};
...

```

The request:

```
URL: http://localhost:3000/api/books/1
METHOD: PUT
BODY:
{
    "title": "Choque de reyes Actualizado",
    "releaseDate": "21/07/2022",
    "author": "Otro autor"
}
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
