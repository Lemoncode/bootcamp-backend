# 04 COMMON

In this example we are going to structure code in `common` folder.

We will start from `03-pods`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

The `common` folder is related with common functionality that it's not related with the domain and we could move to an external library in a future.

In this case, we could create common `middlewares`:

_./src/common/middlewares/logger.middlewares.ts_

```typescript
import { RequestHandler, ErrorRequestHandler } from "express";

export const logRequestMiddleware: RequestHandler = async (req, res, next) => {
  console.log(req.url);
  next();
};

export const logErrorRequestMiddleware: ErrorRequestHandler = async (
  error,
  req,
  res,
  next
) => {
  console.error(error);
  res.sendStatus(500);
};

```

Add barrel file:

_./src/common/middlewares/index.ts_

```typescript
export * from "./logger.middlewares.js";

```

Update `index` file:

_./src/index.ts_

```diff
import "#core/load-env.js";
import express from "express";
import path from "path";
import url from "url";
+ import {
+   logRequestMiddleware,
+   logErrorRequestMiddleware,
+ } from "#common/middlewares/index.js";
import { createRestApiServer } from "#core/servers/index.js";
import { envConstants } from "#core/constants/index.js";
import { booksApi } from "#pods/book/index.js";

...

- restApiServer.use(async (req, res, next) => {
-   console.log(req.url);
-   next();
- });
+ restApiServer.use(logRequestMiddleware);

restApiServer.use("/api/books", booksApi);

- restApiServer.use(async (error, req, res, next) => {
-   console.error(error);
-   res.sendStatus(500);
- });
+ restApiServer.use(logErrorRequestMiddleware);

...

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
