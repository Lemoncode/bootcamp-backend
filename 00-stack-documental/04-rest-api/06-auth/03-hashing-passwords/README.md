# 03 Hashing passwords

In this example we are going to implement the hashing password function.

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
- IS_API_MOCK=true
+ IS_API_MOCK=false
MONGODB_URL=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET

```

We need to implement a function to create a `hashed password` with a secure random `salt` using [randomBytes](https://nodejs.org/dist/latest/docs/api/crypto.html#cryptorandombytessize-callback) defined in nodejs's `crypto` library and we will use a `key derivation function`, in this case [scrypt](https://nodejs.org/dist/latest/docs/api/crypto.html#cryptoscryptpassword-salt-keylen-options-callback):

_./src/common/helpers/hash.helpers.ts_

```typescript
import crypto from 'node:crypto';

const saltLenght = 32; // 16 bytes min recommended
const passwordLength = 64; // 64 bytes = 512 bits

const hashSaltAndPassword = (salt: string, password: string): Promise<string> =>
  new Promise((resolve, reject) => {
    // The default options values are using 2^14 = 16384 iterations and 16 MB of memory.
    crypto.scrypt(password, salt, passwordLength, (error, hashedPassword) => {
      if (error) {
        reject(error);
      }

      // salt:hash
      resolve(`${salt}:${hashedPassword.toString('hex')}`);
    });
  });

export const hash = async (password: string): Promise<string> => {
  const salt = crypto.randomBytes(saltLenght).toString('hex');

  return await hashSaltAndPassword(salt, password);
};
```

Add barrel file:

_./src/common/helpers/index.ts_

```typescript
export * from './hash.helpers.js';
```

Finally, we need to create the `db context`:

_./src/dals/user/user.context.ts_

```typescript
import { dbServer } from '#core/servers/index.js';
import { User } from './user.model.js';

export const getUserContext = () => dbServer.db?.collection<User>('users');
```

Let's insert some users from `console-runners`:

_./src/console-runners/seed-data.runner.ts_

```diff
+ import { hash } from '#common/helpers/index.js';
+ import { getUserContext } from '#dals/user/user.context.js';
import { getBookContext } from '#dals/book/book.context.js';
import { db } from '#dals/mock-data.js';

export const run = async () => {
+ for (const user of db.users) {
+   const hashedPassword = await hash(user.password);

+   await getUserContext().insertOne({
+     ...user,
+     password: hashedPassword,
+   });
+ }

  await getBookContext().insertMany(db.books);
};

```

Let's run the `seed-data` runner:

```bash
npm run start:console-runners

```

> Run in Javascript Debug Terminal

Let's implement the `verifyHash` function to compare plain password with hashed one:

- We will use [timingSafeEqual](https://nodejs.org/dist/latest/docs/api/crypto.html#cryptotimingsafeequala-b) to compare two values because this function does not leak timing information that would allow an attacker to guess one of the values.

_./src/common/helpers/hash.helpers.ts_

```diff
import crypto from 'node:crypto';

const saltLenght = 32; // 16 bytes min recommended
const passwordLength = 64; // 64 bytes = 512 bits

const hashSaltAndPassword = (salt: string, password: string): Promise<string> =>
  new Promise((resolve, reject) => {
    // The default options values are using 2^14 = 16384 iterations and 16 MB of memory.
    crypto.scrypt(password, salt, passwordLength, (error, hashedPassword) => {
      if (error) {
        reject(error);
      }

      // salt:hash
      resolve(`${salt}:${hashedPassword.toString('hex')}`);
    });
  });

export const hash = async (password: string): Promise<string> => {
  const salt = crypto.randomBytes(saltLenght).toString('hex');

  return await hashSaltAndPassword(salt, password);
};

+ // Recommended comparison to protect against [timing attacks](https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html#compare-password-hashes-using-safe-functions)
+ const areEquals = (hashA: string, hashB: string): boolean =>
+   crypto.timingSafeEqual(Buffer.from(hashA, 'hex'), Buffer.from(hashB, 'hex'));

+ export const verifyHash = async (
+   password: string,
+   hashedPassword: string
+ ): Promise<boolean> => {
+   const [salt] = hashedPassword.split(':');

+   const newHashedPassword = await hashSaltAndPassword(salt, password);
+   return areEquals(newHashedPassword, hashedPassword);
+ };


```

Let's implement the `user db repository`:

_./src/dals/user/repositories/user.mongodb-repository.ts_

```diff
+ import { verifyHash } from '#common/helpers/index.js';
+ import { getUserContext } from '../user.context.js';
+ import { User } from '../user.model.js';
import { UserRepository } from './user.repository.js';

export const mongoDBRepository: UserRepository = {
  getUserByEmailAndPassword: async (email: string, password: string) => {
-   throw new Error('Not implemented');
+   const user = await getUserContext().findOne({
+     email,
+   });

+   return verifyHash(password, user?.password)
+     ? ({
+         _id: user._id,
+         email: user.email,
+         role: user.role,
+       } as User)
+     : null;
  },
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

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
