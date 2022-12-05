# 04 Built-in middlewares

In this example we are going to add built-in middlewares from express core.

We will start from `03-debugging`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

Next step could be define the route for "Insert new book":

_./src/index.ts_

```diff
import express from "express";
- import { getBookList, getBook } from "./mock-db.js";
+ import { getBookList, getBook, insertBook } from "./mock-db.js";

...

+ app.post("/api/books", async (req, res) => {
+   const book = req.body;
+   const newBook = await insertBook(book);
+   res.status(201).send(newBook);
+ });

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

Add new book:

```json
URL: http://localhost:3000/api/books
METHOD: POST
BODY:
{
    "title": "El señor de los anillos",
    "releaseDate": "29/07/1954",
    "author": "J. R. R. Tolkien"
}
```

We need to use a middleware for parsing incoming request bodies:

_./src/index.ts_

```diff
import express from "express";
import { getBookList, getBook, insertBook } from "./mock-db.js";

const app = express();
+ app.use(express.json());
...

```

> NOTE: it's using [body-parser.json](https://github.com/expressjs/body-parser) under the hoods.
>
> [Options](https://github.com/expressjs/body-parser#bodyparserjsonoptions)

Now, we could implement "Update existing book":

_./src/index.ts_

```diff
import express from "express";
- import { getBookList, getBook, insertBook } from "./mock-db.js";
+ import { getBookList, getBook, insertBook, updateBook } from "./mock-db.js";
...

+ app.put("/api/books/:id", async (req, res) => {
+   const { id } = req.params;
+   const bookId = Number(id);
+   const book = req.body;
+   await updateBook(bookId, book);
+   res.sendStatus(204);
+ });

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

Update request:

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

And "Delete a discontinued book":

_./src/index.ts_

```diff
import express from "express";
import {
  getBookList,
  getBook,
  insertBook,
  updateBook,
+ deleteBook,
} from "./mock-db.js";

...

+ app.delete("/api/books/:id", async (req, res) => {
+   const { id } = req.params;
+   const bookId = Number(id);
+   await deleteBook(bookId);
+   res.sendStatus(204);
+ });

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

The request:

```
URL: http://localhost:3000/api/books/1
METHOD: DELETE
```

We have improved the legibility with `express` but we still could have a better way to organize the routes, using the [express Router](https://expressjs.com/en/guide/routing.html#express-router) to creare modular route handlers:

_./src/books.api.ts_

```javascript
import { Router } from "express";
import {
  getBookList,
  getBook,
  insertBook,
  updateBook,
  deleteBook,
} from "./mock-db.js";

export const booksApi = Router();

booksApi
  .get("/", async (req, res) => {
    const bookList = await getBookList();
    res.send(bookList);
  })
  .get("/:id", async (req, res) => {
    const { id } = req.params;
    const bookId = Number(id);
    const book = await getBook(bookId);
    res.send(book);
  })
  .post("/", async (req, res) => {
    const book = req.body;
    const newBook = await insertBook(book);
    res.status(201).send(newBook);
  })
  .put("/:id", async (req, res) => {
    const { id } = req.params;
    const bookId = Number(id);
    const book = req.body;
    await updateBook(bookId, book);
    res.sendStatus(204);
  })
  .delete("/:id", async (req, res) => {
    const { id } = req.params;
    const bookId = Number(id);
    await deleteBook(bookId);
    res.sendStatus(204);
  });

```

Update main file:

_./src/index.ts_

```diff
import express from "express";
- import {
-   getBookList,
-   getBook,
-   insertBook,
-   updateBook,
-   deleteBook,
- } from "./mock-db.js";
+ import { booksApi } from "./books.api.js";

const app = express();
app.use(express.json());

app.get("/", (req, res) => {
  res.send("My awesome books portal");
});

+ app.use("/api/books", booksApi);

- app.get("/api/books", async (req, res) => {
-   const bookList = await getBookList();
-   res.send(bookList);
- });

- app.get("/api/books/:id", async (req, res) => {
-   const { id } = req.params;
-   const bookId = Number(id);
-   const book = await getBook(bookId);
-   res.send(book);
- });

- app.post("/api/books", async (req, res) => {
-   const book = req.body;
-   const newBook = await insertBook(book);
-   res.status(201).send(newBook);
- });

- app.put("/api/books/:id", async (req, res) => {
-   const { id } = req.params;
-   const bookId = Number(id);
-   const book = req.body;
-   await updateBook(bookId, book);
-   res.sendStatus(204);
- });

- app.delete("/api/books/:id", async (req, res) => {
-   const { id } = req.params;
-   const bookId = Number(id);
-   await deleteBook(bookId);
-   res.sendStatus(204);
- });

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

Another important middleware is [express.static](https://expressjs.com/en/starter/static-files.html) to serve static files such as images, html files, css files, etc.

Let's add a simple `index.html`:

_./public/index.html_

```html
<!DOCTYPE html>

<html lang="en">
  <head>
    <meta charset="utf-8" />
    <title>Back app</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
  </head>
  <body>
    <h1>Hello from Express</h1>
  </body>
</html>

```

And update express server to serve this file:


_./src/index.ts_

```diff
import express from "express";
+ import path from "path";
+ import url from "url";
import { booksApi } from "./books.api.js";

const app = express();
app.use(express.json());

- app.get("/", (req, res) => {
-   res.send("My awesome books portal");
- });
+ // TODO: Feed env variable in production
+ const __dirname = path.dirname(url.fileURLToPath(import.meta.url));
+ app.use("/", express.static(path.resolve(__dirname, "../public")));

app.use("/api/books", booksApi);

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

> NOTE: `__dirname` was a Global NodeJS using CommonJS. The new way is using [import.meta.url](https://nodejs.org/api/all.html#all_esm_importmetaurl)
>
> NOTE: `__dirname` is equals to the full path to `dist` folder

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
