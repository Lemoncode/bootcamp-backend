# 01 Core

In this example we are going to structure code in `core` folder.

We will start from `00-boilerplate`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

We will use `core` folder for cross code or app configurations like `servers`, `clients` or even configuration `constants` like env variables, routes, etc.

Let's migrate some code from `index.ts`:

_./src/core/servers/rest-api.server.ts_

```typescript
import express from "express";
import cors from "cors";

export const createRestApiServer = () => {
  const restApiServer = express();
  restApiServer.use(express.json());
  restApiServer.use(
    cors({
      methods: "GET",
      origin: "http://localhost:8080",
      credentials: true,
    })
  );

  return restApiServer;
};

```

Add barrel file:

_./src/core/servers/index.ts_

```typescript
export * from "./rest-api.server.js";

```

Update `index` file:

_./src/index.ts_

```diff
import express from "express";
- import cors from "cors";
import path from "path";
import url from "url";
+ import { createRestApiServer } from "./core/servers/index.js";
import { booksApi } from "./books.api.js";

- const app = express();
- app.use(express.json());
- app.use(
-   cors({
-     methods: "GET",
-     origin: "http://localhost:8080",
-     credentials: true,
-   })
- );
+ const restApiServer = createRestApiServer();

// TODO: Feed env variable in production
const __dirname = path.dirname(url.fileURLToPath(import.meta.url));
- app.use("/", express.static(path.resolve(__dirname, "../public")));
+ restApiServer.use("/", express.static(path.resolve(__dirname, "../public")));

- app.use(async (req, res, next) => {
+ restApiServer.use(async (req, res, next) => {
  console.log(req.url);
  next();
});

- app.use("/api/books", booksApi);
+ restApiServer.use("/api/books", booksApi);

- app.use(async (error, req, res, next) => {
+ restApiServer.use(async (error, req, res, next) => {
  console.error(error);
  res.sendStatus(500);
});

- app.listen(3000, () => {
+ restApiServer.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

Another important improvement, it's use environment variables in our code. These are a common used mechanism to provide different values in different environment like `production`, `development`, etc.
In a `nodejs` process, we are getting all env variables from [process.env](https://nodejs.org/dist/latest-v8.x/docs/api/process.html#process_process_env):

_./src/core/constants/env.constants.ts_

```typescript
export const envConstants = {
  isProduction: process.env.NODE_ENV === 'production',
  PORT: process.env.PORT,
  STATIC_FILES_PATH: process.env.STATIC_FILES_PATH, 
  CORS_ORIGIN: process.env.CORS_ORIGIN,
  CORS_METHODS: process.env.CORS_METHODS,
};

```

Add barrel file:

_./src/core/constants/index.ts_

```typescript
export * from "./env.constants.js";

```

Update rest-api server:

_./src/core/servers/rest-api.server.ts_

```diff
import express from "express";
import cors from "cors";
+ import { envConstants } from "../constants/index.js";

export const createRestApiServer = () => {
  const restApiServer = express();
  restApiServer.use(express.json());
  restApiServer.use(
    cors({
-     methods: "GET",
+     methods: envConstants.CORS_METHODS,
-     origin: "http://localhost:8080",
+     origin: envConstants.CORS_ORIGIN,
      credentials: true,
    })
  );

  return restApiServer;
};

```

Update `index` file:

_./src/index.ts_

```diff
import express from "express";
import path from "path";
import url from "url";
import { createRestApiServer } from "./core/servers/index.js";
+ import { envConstants } from "./core/constants/index.js";
import { booksApi } from "./books.api.js";

const restApiServer = createRestApiServer();

- // TODO: Feed env variable in production
const __dirname = path.dirname(url.fileURLToPath(import.meta.url));
- restApiServer.use("/", express.static(path.resolve(__dirname, "../public")));
+ const staticFilesPath = path.resolve(__dirname, envConstants.STATIC_FILES_PATH);
+ restApiServer.use("/", express.static(staticFilesPath));

...

- restApiServer.listen(3000, () => {
+ restApiServer.listen(envConstants.PORT, () => {
- console.log("Server ready at port 3000");
+ console.log(`Server ready at port ${envConstants.PORT}`);
});

```

> NOTE: We assume that we are always getting static files path from `index` location. Thats why we use `../public` in env variable and `path.resolve` from `__dirname`.

How to set this `env variables`? It depends on each OS:

- Windows: `set PORT=3000`
- Linux or Mac: `export PORT=3000`

We will use [dotenv](https://github.com/motdotla/dotenv) library to be OS agnostic:

```bash
npm install dotenv --save
```

Define `.env` file:

_./.env_

```
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
```

We need preload all these env variables before running the app:

_./package.json_

```diff
...
  "scripts": {
    ...
-   "start:dev": "nodemon dist/index",
+   "start:dev": "nodemon -r dotenv/config dist/index",
    ...
  },
```

> [Using preload option](https://github.com/motdotla/dotenv#preload)

```bash
npm start
```

Another approach is load config script from code, it's a nice way to provide [config's options](https://github.com/motdotla/dotenv#options). Move to production dependencies:

Restore script:

_./package.json_

```diff
...
  "scripts": {
    ...
-   "start:dev": "nodemon -r dotenv/config dist/index",
+   "start:dev": "nodemon dist/index",
    ...
  },
```

> [Using from code](https://github.com/motdotla/dotenv#usage)

Add file to load env variables:

_./src/core/load-env.ts_

```typescript
import { config } from "dotenv";
config();

```

Update `index.ts` file, this import should be the first one in your code:

_./src/index.ts_

```diff
+ import "./core/load-env.js";
import express from "express";
import path from "path";
...

```

> NOTE: It does not work if we write the load-env code in the `index.ts` file due to how the ES modules import works.
>
> [More info here](https://github.com/motdotla/dotenv#how-do-i-use-dotenv-with-import)

Finally, we should `ignore` the `.env` file to avoid conflicts with production environments because if we upload this file, we will overwrite production values with these ones.

_./.gitignore_

```diff
node_modules
dist
+ .env

```

Add `.env.example` to keep the same variables shape for developers


_./.env.example_

```
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
