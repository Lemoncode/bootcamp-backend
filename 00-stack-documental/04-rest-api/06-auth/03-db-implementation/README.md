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

We need to implement a function to create a `hashed password` with a secure random `salt` using [randomBytes](https://nodejs.org/dist/latest-v14.x/docs/api/crypto.html#crypto_crypto_randombytes_size_callback) defined in nodejs's `crypto` library:

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

And use a `key derivation function`, in this case, we will use [PBKDF2](https://nodejs.org/dist/latest-v14.x/docs/api/crypto.html#crypto_crypto_pbkdf2_password_salt_iterations_keylen_digest_callback):

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
export * from './hash-password.helpers';

```

Finally, we need to create the `db context`:

_./src/dals/user/user.context.ts_

```typescript
import mongoose, { Schema, SchemaDefinition } from 'mongoose';
import { User } from './user.model';

const userSchema = new Schema({
  email: { type: Schema.Types.String, required: true },
  password: { type: Schema.Types.String, required: true },
  salt: { type: Schema.Types.String, required: true },
  role: { type: Schema.Types.String, required: true },
} as SchemaDefinition<User>);

export const userContext = mongoose.model<User>('User', userSchema);

```

Let's insert some users from `console-runners`:

_./src/console-runners/seed-data.runner.ts_

```diff
import { disconnect } from 'mongoose';
import { connectToDBServer } from 'core/servers';
import { envConstants } from 'core/constants';
import { bookContext } from 'dals/book/book.context';
+ import { userContext } from 'dals/user/user.context';
import { db } from 'dals/mock-data';
+ import { generateSalt, hashPassword } from 'common/helpers';

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);

+ for (const user of db.users) {
+   const salt = await generateSalt();
+   const hashedPassword = await hashPassword(user.password, salt);

+   await userContext.create({
+     ...user,
+     password: hashedPassword,
+     salt,
+   });
+ }

  await bookContext.insertMany(db.books);
  await disconnect();
};

```

> In mongoose doesn't exist `insertOne`.
>
> We could use `insertMany` here too.

Let's run the `seed-data` runner:

```bash
npm run start:console-runners

```

Let's implement the `user db repository`:

_./src/dals/user/repositories/user.db-repository.ts_

```diff
+ import { hashPassword } from 'common/helpers';
+ import { userContext } from '../user.context';
+ import { User } from '../user.model';
import { UserRepository } from './user.repository';

export const dbRepository: UserRepository = {
- getUserByEmailAndPassword: async (email: string, password: string) => null,
+ getUserByEmailAndPassword: async (email: string, password: string) => {
+   const user = await userContext
+     .findOne({
+       email,
+     })
+     .lean();

+   const hashedPassword = await hashPassword(password, user.salt);
+   return user.password === hashedPassword
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

```md
POST http://localhost:3000/api/security/login

### Body
{
	"email": "admin@email.com",
	"password": "test"
}

GET http://localhost:3000/api/books

```

## Appendix

We could try with a modern key derivation function like [Scrypt](https://nodejs.org/dist/latest-v14.x/docs/api/crypto.html#crypto_crypto_scrypt_password_salt_keylen_options_callback):

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
