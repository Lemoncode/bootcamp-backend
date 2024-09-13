# 01 Local files

In this example we are going to learn how to work with stored local files.

We will start from `00-boilerplate/back`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
cd front
npm install

```

In a second terminal:

```bash
cd back
npm install

```

Run `front` and `back` projects to check current implementation:

_front terminal_

```bash
npm start

```

_back terminal_

```bash
npm start

```

> Remember run backend server with `IS_API_MOCK=true`

Open browser in `http://localhost:8080`

Since we have users's images in `public` folder: `admin-avatar.png` and `user-avatar.png`, we will refactor `back` project like:

> Check: http://localhost:3000/admin-avatar.png or http://localhost:3000/user-avatar.png

_.back/src/dals/user/user.model.ts_

```diff
import { ObjectId } from 'mongodb';
import { Role } from '#core/models/index.js';

export interface User {
  _id: ObjectId;
  email: string;
  password: string;
  role: Role;
+ avatar: string;
}

```

_.back/src/dals/mock-data.ts_

```diff
...

export const db: DB = {
  users: [
    {
      _id: new ObjectId(),
      email: 'admin@email.com',
      password: 'test',
      role: 'admin',
+     avatar: '/admin-avatar.png',
    },
    {
      _id: new ObjectId(),
      email: 'user@email.com',
      password: 'test',
      role: 'standard-user',
+     avatar: '/user-avatar.png',
    },
  ],

...

```

Update `api model`:

_.back/src/pods/user/user.api-model.ts_

```diff
import { Role } from '#core/models/index.js';

export interface User {
  email: string;
  role: Role;
+ avatar: string;
}

```

Update `mapper`:

_.back/src/pods/user/user.mappers.ts_

```diff
import * as model from '#dals/index.js';
import * as apiModel from './user.api-model.js';

export const mapUserFromModelToApi = (user: model.User): apiModel.User => ({
  email: user.email,
  role: user.role,
+ avatar: user.avatar,
});

```

It means we have published the users's images in:

- `http://localhost:3000/admin-avatar.png`
- `http://localhost:3000/user-avatar.png`

Since, we have configured a proxy to avoid `CORS` configuration:

_./front/vite.config.ts_

```javascript
...
  server: {
    proxy: {
      '/api': 'http://localhost:3000',
      '/images': {
        target: 'http://localhost:3000',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/images/, ''),
      },
    },
  },
```

We have published the images in `8080` port, because the proxy redirects it to `3000` one:

- `http://localhost:8080/images/admin-avatar.png`
- `http://localhost:8080/images/user-avatar.png`

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
