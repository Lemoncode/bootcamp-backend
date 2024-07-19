# 06 Integration tests

In this example we are going to learn how to implement integration tests.

We will start from `05-async`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install
```

This time, we will implement `integration tests`, in this case, the `book api`. To keep simple as possible, we will install [supertest](https://github.com/visionmedia/supertest):

```bash
npm install supertest @types/supertest --save-dev

```

An important configuration is read the `env variables` for specs too. Let's create a custom env file for tests:

_./.env.test_

```
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
IS_API_MOCK=true
MONGODB_URL=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET

```

_./config/test/env.config.ts_

```javascript
import { config } from 'dotenv';

export function setup() {
  config({
    path: './.env.test',
  });
}
```

Update config:

_./config/test/config.ts_

```diff
import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    globals: true,
    restoreMocks: true,
+   globalSetup: ['./config/test/env.config.ts'],
  },
});

```

> [globalSetup](https://vitest.dev/config/#globalsetup)

Add `book.api` specs:

_./src/pods/book/book.api.spec.ts_

```typescript
describe('pods/book/book.api specs', () => {
  describe('get book list', () => {
    it('', () => {
      // Arrange
      // Act
      // Assert
    });
  });
});
```

`Supertest` needs the app express instance to do a mock request:

_./src/pods/book/book.api.spec.ts_

```diff
+ import supertest from 'supertest';
+ import { createRestApiServer } from '#core/servers/index.js';
+ import { bookApi } from './book.api.js';

+ const app = createRestApiServer();
+ app.use(bookApi);

describe('pods/book/book.api specs', () => {
  describe('get book list', () => {
-   it('', () => {
+   it('should return the whole bookList with values when it request "/" endpoint without query params', async () => {
      // Arrange
+     const route = '/';

      // Act
+     const response = await supertest(app).get(route);

      // Assert
+     expect(response.statusCode).toEqual(200);
+     expect(response.body).toHaveLength(7);
    });
  });
});

```

Run specs:

```bash
npm run test:watch book.api
```

As we see, we are running the app in `mock` mode. If we like to test our repository implementation with a MongoDB memory database, we need to install [mongodb-memory-server](https://github.com/nodkz/mongodb-memory-server):

```bash
npm install mongodb-memory-server --save-dev
```

Add config file:

_./config/test/db-server.config.ts_

```typescript
import { MongoMemoryServer } from 'mongodb-memory-server';

export async function setup() {
  const dbServer = await MongoMemoryServer.create({
    instance: {
      dbName: 'test-book-store',
    },
  });

  process.env.MONGODB_URL = dbServer.getUri();

  return async () => {
    await dbServer.stop();
  };
}
```

> [MongoDB In-Memory server](https://github.com/nodkz/mongodb-memory-server?tab=readme-ov-file#available-options-for-mongomemoryserver)

Update `test/config.ts`:

_./config/test/config.ts_

```diff
import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    globals: true,
    restoreMocks: true,
    globalSetup: [
      './config/test/env.config.ts',
+     './config/test/db-server.config.ts',
    ],
  },
});

```

Update env variable:

_./.env.test_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
- IS_API_MOCK=true
+ IS_API_MOCK=false
- MONGODB_URL=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET

```

Update spec to init MongoDB connection:

_./src/pods/book/book.api.spec.ts_

```diff
+ import { ObjectId } from 'mongodb';
import supertest from 'supertest';
- import { createRestApiServer } from '#core/servers/index.js';
+ import { createRestApiServer, dbServer } from '#core/servers/index.js';
+ import { ENV } from '#core/constants/index.js';
+ import { getBookContext } from '#dals/book/book.context.js';
import { bookApi } from './book.api.js';

...

describe('pods/book/book.api specs', () => {
+ beforeAll(async () => {
+   await dbServer.connect(ENV.MONGODB_URL);
+ });

+ beforeEach(async () => {
+   await getBookContext().insertOne({
+     _id: new ObjectId(),
+     title: 'book-1',
+     author: 'author-1',
+     releaseDate: new Date('2021-07-28'),
+   });
+ });

+ afterEach(async () => {
+   await getBookContext().deleteMany({});
+ });

+ afterAll(async () => {
+   await dbServer.disconnect();
+ });

  describe('get book list', () => {
    it('should return the whole bookList with values when it request "/" endpoint without query params', async () => {
      // Arrange
      const route = '/';

      // Act
      const response = await supertest(app).get(route);

      // Assert
      expect(response.statusCode).toEqual(200);
-     expect(response.body).toHaveLength(7);
+     expect(response.body).toHaveLength(1);
    });
  });
});

```

Run specs:

```bash
npm run test:watch book.api
```

If we want insert new book:

_./src/pods/book/book.api.spec.ts_

```diff
import { ObjectId } from 'mongodb';
import supertest from 'supertest';
import { createRestApiServer, dbServer } from '#core/servers/index.js';
import { ENV } from '#core/constants/index.js';
import { getBookContext } from '#dals/book/book.context.js';
+ import { Book } from './book.api-model.js';
import { bookApi } from './book.api.js';

- const app = createRestApiServer();
- app.use(bookApi);

...
  describe('get book list', () => {
+   const app = createRestApiServer();

+   beforeAll(() => {
+     app.use(bookApi);
+   });

    it('should return the whole bookList with values when it request "/" endpoint without query params', async () => {
      ...
    });
  });

+ describe('insert book', () => {
+   it('should return 201 when an admin user inserts new book', async () => {
+     // Arrange
+     const app = createRestApiServer();
+     app.use((req, res, next) => {
+       req.userSession = {
+         id: '1',
+         role: 'admin',
+       };
+       next();
+     });
+     app.use(bookApi);

+     const route = '/';
+     const newBook: Book = {
+       id: undefined,
+       title: 'book-2',
+       author: 'author-2',
+       releaseDate: '2021-07-29T00:00:00.000Z',
+     };

+     // Act
+     const response = await supertest(app).post(route).send(newBook);

+     // Assert
+     expect(response.statusCode).toEqual(201);
+     expect(response.body.id).toEqual(expect.any(String));
+     expect(response.body.title).toEqual(newBook.title);
+     expect(response.body.author).toEqual(newBook.author);
+     expect(response.body.releaseDate).toEqual(newBook.releaseDate);
+   });
+ });

```

Check `standard-user`:

_./src/pods/book/book.api.spec.ts_

```diff
...

+   it('should return 403 when a standard user try to insert new book', async () => {
+     // Arrange
+     const app = createRestApiServer();
+     app.use((req, res, next) => {
+       req.userSession = {
+         id: '1',
+         role: 'standard-user',
+       };
+       next();
+     });
+     app.use(bookApi);

+     const route = '/';
+     const newBook: Book = {
+       id: undefined,
+       title: 'book-2',
+       author: 'author-2',
+       releaseDate: '2021-07-29T00:00:00.000Z',
+     };

+     // Act
+     const response = await supertest(app).post(route).send(newBook);

+     // Assert
+     expect(response.statusCode).toEqual(403);
+   });
  });
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
