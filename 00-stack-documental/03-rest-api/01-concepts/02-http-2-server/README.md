# 02 Http2 Server

In this example we are going to implement a basic HTTP version 2 server with Nodejs.

We will start from `01-http-server`.

# Steps to build it

Let's use `http2` built-in package from nodejs:

_./index.js_

```diff
- const http = require("http");
+ const http2 = require("http2");
const fs = require("fs");

const handleRequest = (req, res) => {
  if (req.url === "/") {
    res.writeHead(200, { "Content-Type": "text/html" });
    fs.createReadStream("index.html").pipe(res);
  } else if (req.url.match(".png$")) {
    res.writeHead(200, { "Content-Type": "image/png" });
    fs.createReadStream("logo.png").pipe(res);
  }
};

- const server = http.createServer(handleRequest);
+ const server = http2.createServer(handleRequest);

server.listen(3000);

```

Run app:

```bash
node index

```

> Check with Chrome
> Check with Firefox to see raw http request / response

Why does not work? It's because [HTTP2 has to works with https for browser connections](https://nodejs.org/api/http2.html#http2_http2_createserver_options_onrequesthandler).

It means that we need to create a certificate, for example, we will create a self signed for develop:

```bash
openssl req -x509 -newkey rsa:2048 -nodes -sha256 -subj '//CN=localhost' -keyout key.pem -out cert.pem

```

> [Docs](https://www.openssl.org/docs/manmaster/man1/openssl-req.html)

> Double `/` on windows

Copy into `certificate` folder.

Update server:

_./index.js_

```diff
const http2 = require("http2");
const fs = require("fs");

+ const serverOptions = {
+   key: fs.readFileSync("certificate/key.pem"),
+   cert: fs.readFileSync("certificate/cert.pem"),
+ };

const handleRequest = (req, res) => {
  if (req.url === "/") {
    res.writeHead(200, { "Content-Type": "text/html" });
    fs.createReadStream("index.html").pipe(res);
  } else if (req.url.match(".png$")) {
    res.writeHead(200, { "Content-Type": "image/png" });
    fs.createReadStream("logo.png").pipe(res);
  }
};

- const server = http2.createServer(handleRequest);
+ const server = http2.createSecureServer(serverOptions, handleRequest);

server.listen(3000);

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
