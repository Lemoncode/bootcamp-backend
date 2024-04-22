# 01 Get method

In this example we are going to implement a `get` method using http server in Nodejs.

We will start from `00-boilerplate`.

# Steps to build it

We have a bookstore and want to give users access to the following resources:

- [1] Get available book list.
- [2] Get book details.

As administrator we want:

- [3] Insert new book.
- [4] Update existing book.
- [5] Delete a discontinued book.

So, we need mix the minimum `URLs` and `HTTP methods` to identify the resource and the action, that is:

- `/books`: for [1] and [3]
- `/books/:id`: for [2], [4] and [5]

Create simple server as start up point:

_./index.mjs_

```javascript
import http from "node:http";

const handleRequest = (req, res) => {
  res.write("My awesome books portal");
  res.end();
};

const server = http.createServer(handleRequest);

server.listen(3000);
```

> Important: res.end to finish the response.

Run app:

```bash
node index.mjs

```

Normally, the same server that provides data, it's the same that expose frontend files like html, css, images, etc. And for example, we could split routes in `/api` as a prefix for data endpoints and `/` for static files.

Let's define the first route to expose the current book list:

_./index.mjs_

```diff
import http from "node:http";
+ import { getBookList } from "./mock-db.mjs";

const handleRequest = (req, res) => {
+ const { url, method } = req;
+ if (url === "/api/books" && method === "GET") {
+   res.write(getBookList());
+   res.statusCode = 200;
+   res.end();
+ } else {
    res.write("My awesome books portal");
    res.end();
+ }
};
...

```

> Now the [file extension is mandatory](https://nodejs.org/docs/latest-v18.x/api/esm.html#mandatory-file-extensions) on imports

Run app:

```bash
node index.mjs

```

Why it fails? The payloads are always send it like [`string` or `Buffer`](https://nodejs.org/dist/latest/docs/api/http.html#responsewritechunk-encoding-callback). How could a frontend application parse to the desired format if we always send it by `string`?

_./index.mjs_

```diff
...
const handleRequest = (req, res) => {
  const { url, method } = req;
  if (url === "/api/books" && method === "GET") {
+   res.setHeader("Content-Type", "application/json");
-   res.write(getBookList());
+   res.write(JSON.stringify(getBookList()));
    res.statusCode = 200;
    res.end();
  } else {
    res.write("My awesome books portal");
    res.end();
  }
};
...

```

Run app:

```bash
node index.mjs

```

Let's implement now the next query to get the book details:

_./index.mjs_

```diff
import http from "node:http";
- import { getBookList } from "./mock-db.mjs";
+ import { getBookList, getBook } from "./mock-db.mjs";

const handleRequest = (req, res) => {
  const { url, method } = req;
  if (url === "/api/books" && method === "GET") {
    res.setHeader("Content-Type", "application/json");
    res.statusCode = 200;
    res.write(JSON.stringify(getBookList()));
    res.end();
+ } else if (url.startsWith("/api/books/") && method === "GET") {
+   const bookId = url.split("/")[3];
+   res.setHeader("Content-Type", "application/json");
+   res.statusCode = 200;
+   const book = getBook(Number(bookId));
+   res.write(JSON.stringify(book));
+   res.end();
  } else {
    res.write("My awesome books portal");
    res.end();
  }
};
...

```

> [startsWith method](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/startsWith)

Run app:

```bash
node index.mjs

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
