# 02 Config

In this example we are going to configure MongoDB in Nodejs App

We will start from `01-install`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

Let's install the official [MongoDB's driver](https://github.com/mongodb/node-mongodb-native) for Nodejs:

```bash
npm install mongodb --save
npm install @types/mongodb --save-dev

```

Create the connection URI as env variable:

_./.env.example_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
API_MOCK=true
+ MONGODB_URI=mongodb://localhost:27017/book-store

```

_./.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
- API_MOCK=true
+ API_MOCK=false
+ MONGODB_URI=mongodb://localhost:27017/book-store

```

Update `env.constants`:

_./src/core/constants/env.constants.ts_

```diff
export const envConstants = {
  isProduction: process.env.NODE_ENV === 'production',
  PORT: process.env.PORT,
  STATIC_FILES_PATH: process.env.STATIC_FILES_PATH,
  CORS_ORIGIN: process.env.CORS_ORIGIN,
  CORS_METHODS: process.env.CORS_METHODS,
  isApiMock: process.env.API_MOCK === 'true',
+ MONGODB_URI: process.env.MONGODB_URI,
};

```

Configure `mongo server` instance:

_./src/core/servers/db.server.ts_

```typescript
import { MongoClient, Db } from "mongodb";

let dbInstance: Db;

export const connectToDBServer = async (connectionURI: string) => {
  const client = new MongoClient(connectionURI);
  await client.connect();

  dbInstance = client.db();
};

export const getDBInstance = (): Db => dbInstance;
```

Update barrel file:

_./src/core/servers/index.ts_

```diff
export * from "./rest-api.server";
+ export * from "./db.server";

```

Let's update `app` and connect it to db:

_./src/app.ts_

```diff
import express from "express";
import path from "path";
- import { createRestApiServer } from "core/servers";
+ import { createRestApiServer, connectToDBServer, getDBInstance } from "core/servers";
import { envConstants } from "core/constants";
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from "common/middlewares";
import { booksApi } from "pods/book";
...

- restApiServer.listen(envConstants.PORT, () => {
+ restApiServer.listen(envConstants.PORT, async () => {
+ if (!envConstants.isApiMock) {
+   await connectToDBServer(envConstants.MONGODB_URI);
+   console.log("Connected to DB");
+   const db = getDBInstance();
+   await db.collection("books").insertOne({ name: "Book 1" });
+ } else {
+   console.log("Running API mock");
+ }
  console.log(`Server ready at port ${envConstants.PORT}`);
});

```

> Check results in Mongo Compass

Now we can execute same methods like in mongo console, for example, the find method:

_./src/app.ts_

```diff
...
restApiServer.listen(envConstants.PORT, async () => {
  if (!envConstants.isApiMock) {
    await connectToDBServer(envConstants.MONGODB_URI);
    console.log("Connected to DB");
    const db = getDBInstance();
-   await db.collection("books").insertOne({ name: "Book 1" });
+   const books = await db.collection("books").find().toArray();
+   console.log({ books });
  } else {
    console.log("Running API mock");
  }
  console.log(`Server ready at port ${envConstants.PORT}`);
});

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
