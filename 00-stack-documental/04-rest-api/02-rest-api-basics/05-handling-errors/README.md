# 05 Handling errors

In this example we are going to handling errors using http server in Nodejs.

We will start from `04-delete`.

# Steps to build it

When we develop real applications, we need answer to client with the right response, for example if any there is any error in client request (4xx) or any server error (5xx).

So, we will analyze the current app and check which errors we could have to answer with the right status code.

Let's start with [status code 404](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/404):

_./index.js_

```diff
...
    }
  } else {
-   res.write("My awesome books portal");
+   res.statusCode = 404;
    res.end();
  }
};
...

```

If there is any error to send a request, for example, there is a file too large, we could send [status code 400](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400):

_./index.js_

```diff
...
const handleRequest = (req, res) => {
  const { url, method } = req;
+ req.on("error", (error) => {
+   res.statusCode = 400;
+   res.end();
+ });

  if (url === "/api/books") {
    if (method === "GET") {
...

```

> [Available errors](https://nodejs.org/api/errors.html)

For server errors, we could see the [status code 500](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500) which it's for not handled errors

_./index.js_

```diff
...
const handleRequest = (req, res) => {
...
  if (url === "/api/books") {
    if (method === "GET") {
+     try {
        res.setHeader("Content-Type", "application/json");
        res.statusCode = 200;
        res.write(JSON.stringify(getBookList()));
        res.end();
+     } catch (error) {
+       res.statusCode = 500;
+       res.end();
+     }
    } else if (method === "POST") {
      let data = [];
      req.on("data", (chunk) => {
        console.log({ chunk });
        data += chunk;
      });
...

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
