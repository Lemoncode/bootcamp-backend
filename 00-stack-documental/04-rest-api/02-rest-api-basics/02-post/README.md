# 02 Post method

In this example we are going to implement a `post` method using http server in Nodejs.

We will start from `01-get`.

# Steps to build it

Let's implement the admin side now, we will insert a new book:

> To create the requests for next methods we could use [Postman](https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop?hl=en) or similar app.

Add a new book in body:

```
{
    "title": "El señor de los anillos",
    "releaseDate": "29/07/1954",
    "author": "J. R. R. Tolkien"
},

```

> See request headers in Postman

_./index.js_

```diff
const http = require("http");
const { getBookList, getBook } = require("./mock-db");

const handleRequest = (req, res) => {
  const { url, method } = req;
- if (url === "/api/books" && method === "GET") {
+ if (url === "/api/books") {
+   if (method === "GET") {
      res.setHeader("Content-Type", "application/json");
      res.statusCode = 200;
      res.write(JSON.stringify(getBookList()));
      res.end();
+   } else if (method === "POST") {
+     console.log({ req });
+     res.write(url);
+     res.end();
+   }
  } else if (/\/api\/books\/\d$/.test(url) && method === "GET") {
    const [, bookId] = url.match(/\/api\/books\/(\d)$/);
...

```

How we could get the body data? Remember that Nodejs has a single thread, that is, we can't retrieve all data in request because it could be a heavy process if data file has too much size.

_./index.js_

```diff
const http = require("http");
- const { getBookList, getBook } = require("./mock-db");
+ const { getBookList, getBook, insertBook } = require("./mock-db");

const handleRequest = (req, res) => {
  const { url, method } = req;
  if (url === "/api/books") {
    if (method === "GET") {
      res.setHeader("Content-Type", "application/json");
      res.statusCode = 200;
      res.write(JSON.stringify(getBookList()));
      res.end();
    } else if (method === "POST") {
-     console.log({ req });
-     res.write(url);
-     res.end();
+     let data = [];
+     req.on("data", (chunk) => {
+       console.log({ chunk });
+       data += chunk;
+     });

+     req.on("end", () => {
+       const book = JSON.parse(data.toString("utf8"));
+       const newBook = insertBook(book);
+       res.setHeader("Content-Type", "application/json");
+       res.statusCode = 201;
+       res.write(JSON.stringify(newBook));
+       res.end();
+     });
    }
  } else if (/\/api\/books\/\d$/.test(url) && method === "GET") {
    const [, bookId] = url.match(/\/api\/books\/(\d)$/);
...

```

> The default data limit size is 5mb

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
