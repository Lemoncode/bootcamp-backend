# 01 Login headers

In this example we are going to implement a login workflow using headers.

We will start from `00-boilerplate`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

In this example, we want to implement a security module for book store, we will need:

- Login method: providing email and password.
- Secure all book's endpoints.
- Add role to user because some book's endpoints will be available for admin users.
- Logout method.

First, let's create the user model:

_./src/dals/user/user.model.ts_

```typescript
import { ObjectId } from 'mongodb';

export interface User {
  _id: ObjectId;
  email: string;
  password: string;
}

```

Add barrel file:

_./src/dals/user/index.ts_

```typescript
export * from './user.model.js';

```

We will start working on `mock` mode, so let's create mock data:

_./src/dals/mock-data.ts_

```diff
import { ObjectId } from "mongodb";
import { Book } from "./book/index.js";
+ import { User } from './user/index.js';

export interface DB {
+ users: User[];
  books: Book[];
}

export const db: DB = {
+ users: [
+   {
+     _id: new ObjectId(),
+     email: 'admin@email.com',
+     password: 'test',
+   },
+   {
+     _id: new ObjectId(),
+     email: 'user@email.com',
+     password: 'test',
+   },
+ ],
  books: [
    {
  ...

```

Update barrel file:

_./src/dals/index.ts_

```diff
export * from "./book/index.js";
+ export * from './user/index.js';

```

And create `repository structrure`:

_./src/dals/user/repositories/user.repository.ts_

```typescript
import { User } from '../user.model.js';

export interface UserRepository {

}

```

_./src/dals/user/repositories/user.mock-repository.ts_

```typescript
import { UserRepository } from './user.repository.js';

export const mockRepository: UserRepository = {

};

```

_./src/dals/user/repositories/user.db-repository.ts_

```typescript
import { UserRepository } from './user.repository.js';

export const dbRepository: UserRepository = {

};

```

_./src/dals/user/repositories/index.ts_

```typescript
import { mockRepository } from './user.mock-repository.js';
import { dbRepository } from './user.db-repository.js';
import { envConstants } from '#core/constants/index.js';

export const userRepository = envConstants.isApiMock
  ? mockRepository
  : dbRepository;

```

Update barrel file:

_./src/dals/user/index.ts_

```diff
export * from './user.model.js';
+ export * from './repositories/index.js';

```

Let's create the `login` method:

_./src/pods/security/security.rest-api.ts_

```typescript
import { Router } from 'express';

export const securityApi = Router();

securityApi.post('/login', async (req, res, next) => {
  try {
    const { email, password } = req.body;
    // Check is valid user
    // Create token with user info
    // Send token
  } catch (error) {
    next(error);
  }
});

```

> We cannot use `GET` method because browsers could save URL/Query Parameters as plain text in the history.
>
> POST vs PUT: `PUT` method is idempotent (same input same output) and a login method could return different token value for same inputs.

Add barrel file:

_./src/pods/security/index.ts_

```typescript
export * from './security.rest-api.js';

```

Let's implement `mock` method to check user's credentials:

_./src/dals/user/repositories/user.repository.ts_

```diff
import { User } from '../user.model';

export interface UserRepository {
+ getUserByEmailAndPassword: (email: string, password: string) => Promise<User>;
}

```

_./src/dals/user/repositories/user.mock-repository.ts_

```diff
import { UserRepository } from './user.repository.js';
+ import { db } from '../../mock-data.js';

export const mockRepository: UserRepository = {
+ getUserByEmailAndPassword: async (email: string, password: string) =>
+   db.users.find((u) => u.email === email && u.password === password),
};

```

_./src/dals/user/repositories/user.db-repository.ts_

```diff
import { UserRepository } from './user.repository.js';

export const dbRepository: UserRepository = {
+ getUserByEmailAndPassword: async (email: string, password: string) => null,
};

```

> NOTE: We can throw a not implemented error too.

Getting valid user:

_./src/pods/security/security.rest-api.ts_

```diff
import { Router } from 'express';
+ import { userRepository } from '#dals/index.js';

export const securityApi = Router();

securityApi.post('/login', async (req, res, next) => {
  try {
    const { email, password } = req.body;
-   // Check is valid user
+   const user = await userRepository.getUserByEmailAndPassword(
+     email,
+     password
+   );

+   if (user) {
      // Create token with user info
      // Send token
+   } else {
+     res.sendStatus(401);
+   }
  } catch (error) {
    next(error);
  }
});

```
> [401 Unauthorized](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/401)

Let's install [jsonwebtoken](https://github.com/auth0/node-jsonwebtoken) library to create JWT tokens in Nodejs:

```bash
npm install jsonwebtoken --save
npm install @types/jsonwebtoken --save-dev

```

Generate JWT token with user info:

_./src/pods/security/security.rest-api.ts_

```diff
import { Router } from 'express';
+ import jwt from 'jsonwebtoken';
import { userRepository } from '#dals/index.js';

...

    if (user) {
-     // Create token with user info
+     const token = jwt.sign();
      // Send token
    } else {
...
```

Create `UserSession` model:

_./src/common-app/models/user-session.ts_

```typescript
export interface UserSession {
  id: string;
}

```

Add barrel file:

_./src/common-app/models/index.ts_

```typescript
export * from './user-session.js';

```

Update endpoint:

_./src/pods/security/security.rest-api.ts_

```diff
import { Router } from 'express';
import jwt from 'jsonwebtoken';
+ import { UserSession } from '#common-app/models/index.js';
import { userRepository } from '#dals/index.js';

...

    if (user) {
+     const userSession: UserSession = { id: user._id.toHexString() };
+     const secret = 'my-secret'; // TODO: Move to env variable
-     const token = jwt.sign();
+     const token = jwt.sign(userSession, secret, {
+       expiresIn: '1d',
+       algorithm: 'HS256',
+     });
-     // Send token
+     res.send(`Bearer ${token}`);
    } else {
      res.sendStatus(401);
    }
...
```

> [Options](https://github.com/auth0/node-jsonwebtoken#usage)
>
> We could create a certificate with [OpenSSL tool](https://www.openssl.org/docs/manmaster/man1/openssl-req.html)
>
> [Authentication schemes](https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication#authentication_schemes)

Let's update the index with this new API:

_./src/index.ts_

```diff
...
import { booksApi } from '#pods/book/index.js';
+ import { securityApi } from '#pods/security/index.js';

...
restApiServer.use(logRequestMiddleware);

+ restApiServer.use('/api/security', securityApi);
restApiServer.use('/api/books', booksApi);

...

```

Let's run app in `mock` mode:

```bash
npm start
```

```md
URL:http://localhost:3000/api/security/login
METHOD: POST
BODY:
{
	"email": "admin@email.com",
	"password": "test"
}
```

> [Check jwt info](https://jwt.io/)

On the other hand, we will secure the `book` api:

_./src/pods/book/book.rest-api.ts_

```diff
import { Router } from 'express';
+ import jwt from 'jsonwebtoken';
import { bookRepository } from '#dals/index.js';
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from './book.mappers.js';

...

booksApi
  .get('/', async (req, res, next) => {
    try {
+     const [, token] = req.headers.authorization?.split(' ') || [];
+     const secret = 'my-secret'; // TODO: Move to env variable
+     jwt.verify(token, secret, async (error, userSession) => {
+       if (userSession) {
          const page = Number(req.query.page);
          const pageSize = Number(req.query.pageSize);
          const bookList = await bookRepository.getBookList(page, pageSize);
          res.send(mapBookListFromModelToApi(bookList));
+       } else {
+         res.sendStatus(401);
+       }
+     });
    } catch (error) {
      next(error);
    }
  })
...
```

> [Authorization header](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Authorization)
>
> `Authorization: Bearer my-token`

```md
URL:http://localhost:3000/api/books
METHOD: GET

HEADERS:
Authorization: Bearer my-token
```

Let's refactor this code

_./.env.example_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
API_MOCK=true
MONGODB_URI=mongodb://localhost:27017/book-store
+ AUTH_SECRET=MY_AUTH_SECRET

```

_./.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
API_MOCK=true
MONGODB_URI=mongodb://localhost:27017/book-store
+ AUTH_SECRET=MY_AUTH_SECRET

```

Create `env constant`:

_./src/core/constants/env.constants.ts_

```diff
export const envConstants = {
  isProduction: process.env.NODE_ENV === 'production',
  PORT: process.env.PORT,
  STATIC_FILES_PATH: process.env.STATIC_FILES_PATH,
  CORS_ORIGIN: process.env.CORS_ORIGIN,
  CORS_METHODS: process.env.CORS_METHODS,
  isApiMock: process.env.API_MOCK === 'true',
  MONGODB_URI: process.env.MONGODB_URI,
+ AUTH_SECRET: process.env.AUTH_SECRET,
};

```

Create `security.middleware`:

_./src/pods/security/security.middlewares.ts_

```typescript
import { RequestHandler } from 'express';
import jwt from 'jsonwebtoken';
import { envConstants } from '#core/constants/index.js';
import { UserSession } from '#common-app/models/index.js';

const verify = (token: string, secret: string): Promise<UserSession> =>
  new Promise((resolve, reject) => {
    jwt.verify(token, secret, (error, userSession: UserSession) => {
      if (error) {
        reject(error);
      }

      if (userSession) {
        resolve(userSession);
      } else {
        reject();
      }
    });
  });

export const authenticationMiddleware: RequestHandler = async (
  req,
  res,
  next
) => {
  try {
    const [, token] = req.headers.authorization?.split(' ') || [];
    const userSession = await verify(token, envConstants.AUTH_SECRET);
    req.userSession = userSession;
    next();
  } catch (error) {
    res.sendStatus(401);
  }
};

```

> We will have Typescript issues if we try to use `util.promisify` from NodeJS.
>
> import { promisify } from 'util';
> const verify = promisify(jwt.verify);

We could extend Express's Request types:

_./global-types.d.ts_

```typescript
declare namespace Express {
  export interface UserSession {
    id: string;
  }

  export interface Request {
    userSession?: UserSession;
  }
}

```

_./tsconfig.json_

```diff
{
  "compilerOptions": {
    ...
  },
- "include": ["src/**/*"]
+ "include": ["src/**/*", "./global-types.d.ts"]
}

```

Update `security` rest-api:

_./src/pods/security/security.rest-api.ts_

```diff
import { Router } from 'express';
import jwt from 'jsonwebtoken';
import { UserSession } from '#common-app/models/index.js';
+ import { envConstants } from '#core/constants/index.js';
import { userRepository } from '#dals/index.js';

...
    if (user) {
      const userSession: UserSession = { id: user._id.toHexString() };
-     const secret = 'my-secret'; // TODO: Move to env variable
-     const token = jwt.sign(userSession, secret, {
+     const token = jwt.sign(userSession, envConstants.AUTH_SECRET, {
        expiresIn: '1d',
        algorithm: 'HS256',
      });
      res.send(`Bearer ${token}`);
...

```

Update barrel file:

_./src/pods/security/index.ts_

```diff
export * from './security.rest-api.js';
+ export * from './security.middlewares.js';

```

Update `book` rest-api:

_./src/pods/book/book.rest-api.ts_

```diff
import { Router } from 'express';
- import jwt from 'jsonwebtoken';
...

booksApi
  .get('/', async (req, res, next) => {
    try {
-     const [, token] = req.headers.authorization?.split(' ') || [];
-     const secret = 'my-secret'; // TODO: Move to env variable
-     jwt.verify(token, secret, async (error, userSession) => {
-       if (userSession) {
          const page = Number(req.query.page);
          const pageSize = Number(req.query.pageSize);
          const bookList = await bookRepository.getBookList(page, pageSize);
          res.send(mapBookListFromModelToApi(bookList));
-       } else {
-         res.sendStatus(401);
-       }
-     });
    } catch (error) {
      next(error);
    }
  })
  ...

```

Secure all book API:

_./src/index.ts_

```diff
...
import { booksApi } from '#pods/book/index.js';
- import { securityApi } from '#pods/security/index.js';
+ import { securityApi, authenticationMiddleware } from '#pods/security/index.js';

...

restApiServer.use('/api/security', securityApi);
- restApiServer.use('/api/books', booksApi);
+ restApiServer.use('/api/books', authenticationMiddleware, booksApi);

...

```

Let's try now `/api/books`:

> Stop and run again to read new env variable

```md
URL: http://localhost:3000/api/security/login
METHOD: POST

BODY:
{
	"email": "admin@email.com",
	"password": "test"
}
```

> Sign jwt with new env variable

```md
URL: http://localhost:3000/api/books
METHOD: GET

HEADERS:
Authorization: Bearer my-token
```

Let's add a role for each user:

_./src/common-app/models/role.ts_

```typescript
export type Role = 'admin' | 'standard-user';

```

_./src/common-app/models/user-session.ts_

```diff
+ import { Role } from './role.js';

export interface UserSession {
  id: string;
+ role: Role;
}

```

Update barrel file:

_./src/common-app/models/index.ts_

```diff
export * from './user-session.js';
+ export * from './role.js';

```

Update dals model:

_./src/dals/user/user.model.ts_

```diff
import { ObjectId } from 'mongodb';
+ import { Role } from '#common-app/models/index.js';

export interface User {
  _id: ObjectId;
  email: string;
  password: string;
+ role: Role;
}

```

And mock data:

_./src/dals/mock-data.ts_

```diff
...
export const db: DB = {
  users: [
    {
      _id: new ObjectId(),
      email: 'admin@email.com',
      password: 'test',
+     role: 'admin',
    },
    {
      _id: new ObjectId(),
      email: 'user@email.com',
      password: 'test',
+     role: 'standard-user',
    },
  ],
  books: [
...

```

Update Express's Request:

_./global-types.d.ts_

```diff
declare namespace Express {
+ export type Role = 'admin' | 'standard-user';

  export interface UserSession {
    id: string;
+   role: Role;
  }

  export interface Request {
    userSession?: UserSession;
  }
}

```

Update `login` method:

_./src/pods/security/security.rest-api.ts_

```diff
...

    if (user) {
      const userSession: UserSession = {
        id: user._id.toHexString(),
+       role: user.role,
      };
      const token = jwt.sign(userSession, envConstants.AUTH_SECRET, {
        expiresIn: '1d',
...
```

Let's run app and [check new jwt](https://jwt.io/):

```bash
npm start

```

```md
URL: http://localhost:3000/api/security/login
METHOD: POST

BODY:
{
	"email": "admin@email.com",
	"password": "test"
}
```

Now, we could create a new `authorization middleware`:

_./src/pods/security/security.middlewares.ts_

```diff
import { RequestHandler } from 'express';
import jwt from 'jsonwebtoken';
import { envConstants } from '#core/constants/index.js';
- import { UserSession } from '#common-app/models/index.js';
+ import { UserSession, Role } from '#common-app/models/index.js';

...

+ const isAuthorized = (currentRole: Role, allowedRoles: Role[]) =>
+   (Boolean(currentRole) && allowedRoles?.some((role) => currentRole === role));

+ export const authorizationMiddleware =
+   (allowedRoles: Role[]): RequestHandler =>
+   async (req, res, next) => {
+     if (isAuthorized(req.userSession?.role, allowedRoles)) {
+       next();
+     } else {
+       res.sendStatus(403);
+     }
+   };

```

Let's use this new `middleware` in book rest-api:

_./src/pods/book/book.rest-api.ts_

```diff
import { Router } from 'express';
import { bookRepository } from '#dals/index.js';
+ import { authorizationMiddleware } from '#pods/security/index.js';
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from './book.mappers.js';

export const booksApi = Router();

...
- .post('/', async (req, res, next) => {
+ .post('/', authorizationMiddleware(['admin']), async (req, res, next) => {
    try {
      const book = mapBookFromApiToModel(req.body);
      const newBook = await bookRepository.saveBook(book);
      res.status(201).send(mapBookFromModelToApi(newBook));
    } catch (error) {
      next(error);
    }
  })
- .put('/:id', async (req, res, next) => {
+ .put('/:id', authorizationMiddleware(['admin']), async (req, res, next) => {
    try {
      const { id } = req.params;
      if (await bookRepository.getBook(id)) {
        const book = mapBookFromApiToModel({ ...req.body, id });
        await bookRepository.saveBook(book);
        res.sendStatus(204);
      } else {
        res.sendStatus(404);
      }
    } catch (error) {
      next(error);
    }
  })
- .delete('/:id', async (req, res, next) => {
+ .delete(
+   '/:id',
+   authorizationMiddleware(['admin']),
+   async (req, res, next) => {
      try {
        const { id } = req.params;
        const isDeleted = await bookRepository.deleteBook(id);
        res.sendStatus(isDeleted ? 204 : 404);
      } catch (error) {
        next(error);
      }
    }
  );

```

Let's try this:

```
npm start

```

```md
URL: http://localhost:3000/api/security/login
METHOD: POST

BODY:
{
	"email": "user@email.com",
	"password": "test"
}
```

Finally, we will implement the `logout` method:

_./src/pods/security/security.rest-api.ts_

```diff
import { Router } from 'express';
import jwt from 'jsonwebtoken';
import { UserSession } from '#common-app/models/index.js';
import { envConstants } from '#core/constants/index.js';
import { userRepository } from '#dals/index.js';
+ import { authenticationMiddleware } from './security.middlewares.js';

...

  } catch (error) {
    next(error);
  }
- });
+ })
+ .post('/logout', authenticationMiddleware, async (req, res) => {
+   // NOTE: We cannot invalidate token using jwt libraries.
+   // Different approaches:
+   // - Short expiration times in token
+   // - Black list tokens on DB
+   res.sendStatus(200);
+ });

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
