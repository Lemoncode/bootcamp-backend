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
  const app = express();
  app.use(express.json());
  app.use(
    cors({
      methods: ["GET"],
      origin: "http://localhost:8080",
    })
  );

  return app;
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
import path from "node:path";
+ import { createRestApiServer } from "./core/servers/index.js";
import { booksApi } from "./books.api.js";

- const app = express();
+ const app = createRestApiServer();
- app.use(express.json());
- app.use(
-   cors({
-     methods: ["GET"],
-     origin: "http://localhost:8080",
-   })
- );

...

```

Another important improvement, it's use environment variables in our code. These are a common used mechanism to provide different values in different environment like `production`, `development`, etc.
In a `nodejs` process, we are getting all env variables from [process.env](https://nodejs.org/docs/latest/api/all.html#all_process_processenv):

_./src/core/constants/env.constants.ts_

```typescript
export const ENV = {
  IS_PRODUCTION: process.env.NODE_ENV === "production",
  PORT: Number(process.env.PORT),
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
+ import { ENV } from "../constants/index.js";

export const createRestApiServer = () => {
  const app = express();
  app.use(express.json());
  app.use(
    cors({
-     methods: ["GET"],
+     methods: ENV.CORS_METHODS,
-     origin: "http://localhost:8080",
+     origin: ENV.CORS_ORIGIN,
    })
  );

  return app;
};

```

Update `index` file:

_./src/index.ts_

```diff
import express from "express";
import path from "node:path";
import { createRestApiServer } from "./core/servers/index.js";
+ import { ENV } from "./core/constants/index.js";
import { booksApi } from "./books.api.js";

const app = createRestApiServer();

app.use(
  "/",
- express.static(path.resolve(import.meta.dirname, "../public")
+ express.static(path.resolve(import.meta.dirname, ENV.STATIC_FILES_PATH))
);

...

- app.listen(3000, () => {
+ app.listen(ENV.PORT, () => {
-   console.log("Server ready at port 3000");
+   console.log(`Server ready at port ${ENV.PORT}`);
  });

```

> NOTE: We assume that we are always getting static files path from `index` location. Thats why we use `../public` in env variable and `path.resolve` from `__dirname`.

Define `.env` file:

_./.env_

```
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
```

How to read this `env` file? `Node.js` has an experimental flag [--env-file](https://nodejs.org/docs/latest/api/all.html#all_cli_--env-fileconfig) to consume env variables from a file. Meanwhile, we can use [dotenv](https://www.npmjs.com/package/dotenv) library that is OS agnostic:

```bash
npm install dotenv --save-dev
```

We need preload all these env variables before running the app:

_./package.json_

```diff
...
  "scripts": {
    "start": "run-p -l type-check:watch start:dev",
-   "start:dev": "tsx --watch src/index.ts",
+   "start:dev": "tsx --require dotenv/config --watch src/index.ts",
   ...

```

> [Using preload option](https://github.com/motdotla/dotenv#preload)

```bash
npm start
```

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
