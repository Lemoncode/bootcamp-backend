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
API_MOCK=true
MONGODB_URI=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET

```

_./config/test/env.config.js_

```javascript
const { config } = require('dotenv');
config({
  path: './.env.test',
});

```

> NOTE: Jest is still running in CommonJS

Update jest config:

_./config/test/jest.js_

```diff
export default {
  rootDir: '../../',
  verbose: true,
  restoreMocks: true,
+ setupFiles: ['<rootDir>/config/test/env.config.js'],
  ...
};

```

Add `book.rest-api` specs:

_./src/pods/book/book.rest-api.spec.ts_

```typescript
describe('pods/book/book.rest-api specs', () => {
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

_./src/pods/book/book.rest-api.spec.ts_

```diff
+ import supertest from 'supertest';
+ import { createRestApiServer } from '#core/servers/index.js';
+ import { booksApi } from './book.rest-api.js';

+ const app = createRestApiServer();
+ app.use(booksApi);

describe('pods/book/book.rest-api specs', () => {
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
npm run test:watch book.rest-api
```

As we see, we are running the app in `mock` mode. If we like to test our repository implementation with a MongoDB memory database, we need to install [jest-mongodb](https://github.com/shelfio/jest-mongodb) preset:

```bash
npm install @shelf/jest-mongodb --save-dev
```

Add config file:

_./jest-mongodb-config.js_

```javascript
module.exports = {
  mongodbMemoryServerOptions: {
    binary: {
      version: '6',
      skipMD5: true,
    },
    instance: {
      dbName: 'test-book-store',
      port: 27017,
    },
    autoStart: false,
  },
};

```

> [MongoDB In-Memory server](https://github.com/nodkz/mongodb-memory-server#available-options)

Update jest config:

_./config/test/jest.js_

```diff
export default {
  rootDir: '../../',
  verbose: true,
  restoreMocks: true,
  setupFiles: ['<rootDir>/config/test/env.config.js'],
+ preset: '@shelf/jest-mongodb',
+ watchPathIgnorePatterns: ['<rootDir>/globalConfig'],
  ...
};

```

> [Ignore globalConfig](https://github.com/shelfio/jest-mongodb#6-jest-watch-mode-gotcha)

Ignore `globalConfig`:

_./.gitignore_

```diff
node_modules
dist
.env
mongo-data
+ globalConfig.json

```

Update env variable:

_./.env.test_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
- API_MOCK=true
+ API_MOCK=false
- MONGODB_URI=mongodb://localhost:27017/book-store
+ MONGODB_URI=mongodb://localhost:27017/test-book-store
AUTH_SECRET=MY_AUTH_SECRET

```

Update spec to init MongoDB connection:

_./src/pods/book/book.rest-api.spec.ts_

```diff
+ import { ObjectId } from 'mongodb';
import supertest from 'supertest';
import {
  createRestApiServer,
+ connectToDBServer,
+ disconnectFromDBServer,
} from '#core/servers/index.js';
+ import { envConstants } from '#core/constants/index.js';
+ import { getBookContext } from '#dals/book/book.context.js';
import { booksApi } from './book.rest-api.js';

const app = createRestApiServer();
app.use(booksApi);

describe('pods/book/book.rest-api specs', () => {
+ beforeAll(async () => {
+   await connectToDBServer(envConstants.MONGODB_URI);
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
+   await disconnectFromDBServer();
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

If we want insert new book:

_./src/pods/book/book.rest-api.spec.ts_

```diff
import { ObjectId } from 'mongodb';
import supertest from 'supertest';
import {
  createRestApiServer,
  connectToDBServer,
  disconnectFromDBServer,
} from '#core/servers/index.js';
import { envConstants } from '#core/constants/index.js';
import { getBookContext } from '#dals/book/book.context.js';
+ import { Book } from './book.api-model.js';
import { booksApi } from './book.rest-api.js';

const app = createRestApiServer();
+ app.use((req, res, next) => {
+   req.userSession = {
+     id: '1',
+     role: 'admin',
+   };
+   next();
+ });
app.use(booksApi);

...

+   it('should return return 201 when it inserts new book', async () => {
+     // Arrange
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
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
