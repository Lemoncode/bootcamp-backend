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

Open browser in `http://localhost:8080`

Since we have users's images in `public` folder: `admin-avatar.png` and `user-avatar.png`, we will refactor `back` project like:

_.back/src/dals/user/user.model.ts_

```diff
import { ObjectId } from 'mongodb';
import { Role } from 'common-app/models';

export interface User {
  _id: ObjectId;
  email: string;
  password: string;
  salt: string;
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
      salt: '',
      role: 'admin',
+     avatar: '/admin-avatar.png',
    },
    {
      _id: new ObjectId(),
      email: 'user@email.com',
      password: 'test',
      salt: '',
      role: 'standard-user',
+     avatar: '/user-avatar.png',
    },
  ],

...

```

Update `api model`:

_.back/src/pods/user/user.api-model.ts_

```diff
import { Role } from 'common-app/models';

export interface User {
  email: string;
  role: Role;
+ avatar: string;
}

```

Update `mapper`:

_.back/src/pods/user/user.mappers.ts_

```diff
import * as model from 'dals/user';
import * as apiModel from './user.api-model';

export const mapUserFromModelToApi = (user: model.User): apiModel.User => ({
  email: user.email,
  role: user.role,
+ avatar: user.avatar,
});

```

It means we have published the users's images in:

- `http://localhost:3000/admin-avatar.png`
- `http://localhost:3000/user-avatar.png`

Since, we have configured the webpack proxy to avoid `CORS` configuration:

_./front/config/webpack/dev.js_

```
  proxy: {
    '/api': 'http://localhost:3000',
    '/': 'http://localhost:3000',
  },
```

We have published the images in `8080` port, because webpack redirect it to `3000` one:

- `http://localhost:8080/admin-avatar.png`
- `http://localhost:8080/user-avatar.png`

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
