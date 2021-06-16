# 01 Http Server

In this example we are going to implement a basic HTTP server with Nodejs.

We will start from `00-boilerplate`.

# Steps to build it

Let's create our first http server with nodejs`:

_./index.js_

```javascript
const http = require("http");

const server = http.createServer((req, res) => {
  console.log(req.url);
  res.write('Hello from nodejs');
  res.end();
});

server.listen(3000);
```

Run app:

```bash
node index

```

> Check with Chrome
> Check with Firefox to see raw http request / response

Let's add an `index.html` file to serve it:

_./index.html_

```html
<!DOCTYPE html>

<html lang="en">
  <head>
    <meta charset="utf-8" />
    <title>Hello</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
  </head>
  <body>
    <h1>Hello from nodejs</h1>
    <img src="logo.png" />
  </body>
</html>

```

_./index.js_

```diff
const http = require("http");
+ const fs = require("fs");

- const server = http.createServer((req, res) => {
-   console.log(req.url);
-   res.write('Hello from nodejs');
-   res.end();
- });

+ const handleRequest = (req, res) => {
+   if (req.url === "/") {
+     res.writeHead(200, { "Content-Type": "text/html" });
+     fs.createReadStream("index.html").pipe(res);
+   }
+ };

+ const server = http.createServer(handleRequest);

server.listen(3000);
```

Finally, we need to resolve the image query:

_./index.js_

```diff
const http = require("http");
const fs = require("fs");

const handleRequest = (req, res) => {
  if (req.url === "/") {
    res.writeHead(200, { "Content-Type": "text/html" });
    fs.createReadStream("index.html").pipe(res);
- }
+ } else if (req.url.match(".png$")) {
+   res.writeHead(200, { "Content-Type": "image/png" });
+   fs.createReadStream("logo.png").pipe(res);
+ }
};

...

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
