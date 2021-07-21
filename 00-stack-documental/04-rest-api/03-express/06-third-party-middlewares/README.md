# 06 Third party middlewares

In this example we are going to use a third party middleware.

We will start from `05-custom-middleware`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

Middlewares allow us to extend express functionality as a unique global point of app's configuration and we could install existing third party middlewares for concepts like enable CORS, JWT tokens, Cookie parser, etc.

We will start creating a simple front-end application that it will fetch a book detail. Create `front` folder and init a new package.json:

```bash
cd front
npm init -y
```

Add `index.html` file:

_./front/index.html_

```html
<!DOCTYPE html>

<html lang="en">
  <head>
    <meta charset="utf-8" />
    <title>Front app</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
  </head>
  <body>
    <h1>Hello from Front App</h1>
    <script src="./app.js"></script>
  </body>
</html>

```

Add js file:

_./front/app.js_

```javascript
console.log("Running front app");

fetch("http://localhost:3000/api/books/2")
  .then((response) => {
    return response.json();
  })
  .then((book) => {
    console.log({ book });
  });

```

We could create another express app to serve this files in another process, but to be simple we will install [lite-server](https://github.com/johnpapa/lite-server) to serve this front app.

```bash
npm install lite-server --save-dev
```

Add config file:

_./front/lite-server.config.json_

```json
{
  "port": 8080
}

```

Finally, create the start command:

_./front/package.json_

```diff
...
  "scripts": {
-   "test": "echo \"Error: no test specified\" && exit 1"
+   "start": "lite-server -c lite-server.config.json"
  },
```

Let's runs both servers:

```bash
cd back/
npm start

cd front/
npm start

```

This kind of request is a cross-origin request and we need enable [CORS](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS) (Cross-Origin Resource Sharing) in backend side to allow sharing resources between different domains.

We will install [cors](https://github.com/expressjs/cors) middleware from `express` team:

```bash
cd back/
npm install cors --save
npm install @types/cors --save-dev

```

Let's configure it:

_./back/src/index.ts_

```diff
import express from "express";
+ import cors from 'cors';
import path from "path";
import { booksApi } from "./books.api";

const app = express();
app.use(express.json());
+ app.use(
+   cors({
+     methods: "GET",
+     origin: "http://localhost:8080",
+   })
+ );

...

```

> [Options](https://github.com/expressjs/cors#configuration-options)

Let's runs both servers:

```bash
cd back/
npm start

cd front/
npm start

```

If we need for example send some headers or cookies with authorization token, we need to enable `credentials` mode:

_./front/app.js_

```diff
console.log("Running front app");

- fetch("http://localhost:3000/api/books/2")
+ fetch("http://localhost:3000/api/books/2", {
+   credentials: "include",
+ })
  .then((response) => {
    return response.json();
  })
  .then((book) => {
    console.log({ book });
  });

```

Let's runs both servers:

```bash
cd back/
npm start

cd front/
npm start

```

Enable on backend:

_./back/src/index.ts_

```diff
...

const app = express();
app.use(express.json());
app.use(
  cors({
    methods: "GET",
    origin: "http://localhost:8080",
+   credentials: true,
  })
);

...

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
