# 03 DB implementation

In this example we are going to implement a login workflow in DB with mongoose/mongo.

We will start from `02-login-cookies`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

Now, let's run the app with real data base:

_./.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
- API_MOCK=true
+ API_MOCK=false
MONGODB_URI=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET

```

We will add `salt` field to user model:

_./src/dals/user/user.model.ts_

```diff
...

export interface User {
  _id: ObjectId;
  email: string;
  password: string;
+ salt: string;
  role: Role;
}

```

Update `mock-data`:

_./src/dals/mock-data.ts_

```diff
...
export const db: DB = {
  users: [
    {
      _id: new ObjectId(),
      email: 'admin@email.com',
      password: 'test',
+     salt: '',
      role: 'admin',
    },
    {
      _id: new ObjectId(),
      email: 'user@email.com',
      password: 'test',
+     salt: '',
      role: 'standard-user',
    },
  ],

```

We need to implement a function to create a `hashed password` with a secure random `salt` using [randomBytes](https://nodejs.org/dist/latest/docs/api/crypto.html#cryptorandombytessize-callback) defined in nodejs's `crypto` library:

_./src/common/helpers/hash-password.helpers.ts_

```typescript
import crypto from 'crypto';
import { promisify } from 'util';
const randomBytes = promisify(crypto.randomBytes);

const saltLength = 16;
export const generateSalt = async (): Promise<string> => {
  const salt = await randomBytes(saltLength);
  return salt.toString('hex');
};
```

> 64 bits min recommended -> 8 bytes. We are using 16 bytes.

And use a `key derivation function`, in this case, we will use [PBKDF2](https://nodejs.org/dist/latest/docs/api/crypto.html#cryptopbkdf2password-salt-iterations-keylen-digest-callback):

_./src/common/helpers/hash-password.helpers.ts_

```diff
import crypto from 'crypto';
import { promisify } from 'util';
const randomBytes = promisify(crypto.randomBytes);
+ const pbkdf2 = promisify(crypto.pbkdf2);

const saltLength = 16;
export const generateSalt = async (): Promise<string> => {
  const salt = await randomBytes(saltLength);
  return salt.toString('hex');
};

+ const passwordLength = 64; // 64 bytes = 512 bits
+ const digestAlgorithm = 'sha512';
+ const iterations = 100000;

+ export const hashPassword = async (
+   password: string,
+   salt: string
+ ): Promise<string> => {
+   const hashedPassword = await pbkdf2(
+     password,
+     salt,
+     iterations,
+     passwordLength,
+     digestAlgorithm
+   );

+   return hashedPassword.toString('hex');
+ };

```

Add barrel file:

_./src/common/helpers/index.ts_

```typescript
export * from './hash-password.helpers.js';

```

Finally, we need to create the `db context`:

_./src/dals/user/user.context.ts_

```typescript
import { db } from '#core/servers/index.js';
import { User } from './user.model.js';

export const getUserContext = () => db?.collection<User>('users');

```

Let's insert some users from `console-runners`:

_./src/console-runners/seed-data.runner.ts_

```diff
+ import { generateSalt, hashPassword } from '#common/helpers/index.js';
import {
  connectToDBServer,
  disconnectFromDBServer,
} from '#core/servers/index.js';
import { envConstants } from '#core/constants/index.js';
import { getBookContext } from '#dals/book/book.context.js';
+ import { getUserContext } from '#dals/user/user.context.js';
import { db } from '#dals/mock-data.js';

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);

+ for (const user of db.users) {
+   const salt = await generateSalt();
+   const hashedPassword = await hashPassword(user.password, salt);

+   await getUserContext().insertOne({
+     ...user,
+     password: hashedPassword,
+     salt,
+   });
+ }

  await getBookContext().insertMany(db.books);
  await disconnect();
};

```

Let's run the `seed-data` runner:

```bash
npm run start:console-runners

```

> Run in Javascript Debug Terminal

Let's implement the `user db repository`:

_./src/dals/user/repositories/user.db-repository.ts_

```diff
+ import { hashPassword } from '#common/helpers/index.js';
+ import { getUserContext } from '../user.context.js';
+ import { User } from '../user.model.js';
import { UserRepository } from './user.repository.js';

export const dbRepository: UserRepository = {
- getUserByEmailAndPassword: async (email: string, password: string) => null,
+ getUserByEmailAndPassword: async (email: string, password: string) => {
+   const user = await getUserContext().findOne({
+     email,
+   });

+   const hashedPassword = await hashPassword(password, user?.salt);
+   return user?.password === hashedPassword
+     ? ({
+         _id: user._id,
+         email: user.email,
+         role: user.role,
+       } as User)
+     : null;
+ },
};

```

Let's run app using database:

```bash
npm start

```

> Run in Javascript Debug Terminal

```md
URL: http://localhost:3000/api/security/login
METHOD: POST

BODY:
{
  "email": "admin@email.com",
  "password": "test"
}

URL: http://localhost:3000/api/books
METHOD: GET
```

## Appendix

We could try with a modern key derivation function like [Scrypt](https://nodejs.org/dist/latest/docs/api/crypto.html#cryptoscryptpassword-salt-keylen-options-callback):

_./src/common/helpers/hash-password.helpers.ts_

```typescript
import crypto from 'crypto';
import { promisify } from 'util';
const randomBytes = promisify(crypto.randomBytes);

const saltLength = 16;
export const generateSalt = async (): Promise<string> => {
  const salt = await randomBytes(saltLength);
  return salt.toString('hex');
};

const passwordLength = 64; // 64 bytes = 512 bits
const iterations = 16384; // Must be a power of two greater than one (2^x)

// Memory required = 128 * N * r * p (128 * cost * blockSize * parallelization)
// E.g. 128 * 16384 * 8 * 1 = 16 MB

export const hashPassword = async (
  password: string,
  salt: string
): Promise<string> => {
  const promise = new Promise<string>((resolve, reject) => {
    crypto.scrypt(
      password,
      salt,
      passwordLength,
      {
        cost: iterations,
        blockSize: 8,
        parallelization: 1,
        maxmem: 32 * 1024 * 1024,
      },
      (error, hashedPassword) => {
        if (error) {
          reject(error);
        }

        resolve(hashedPassword.toString('hex'));
      }
    );
  });

  return promise;
};
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
