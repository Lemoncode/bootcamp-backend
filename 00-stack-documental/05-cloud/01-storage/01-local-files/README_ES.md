# 01 Ficheros locales

En este ejemplo, vamos a aprender cómo trabajar con archivos locales.

Arrancamos por `00-boilerplate/back`.

# Pasos

Ejecutamos `npm install` para intalar los paquetes del ejemplo anterior (front):

```bash
cd front
npm install

```

Abrimos otro terminal para instalar los del back:

```bash
cd back
npm install

```

Ejecutamos `npm start` tanto en front como en back para levantar ambos:

_front terminal_

```bash
npm start

```

_back terminal_

```bash
npm start
```

> Acuerdate de que en las _env_ decir que tiramos de IS_API_MOCK `IS_API_MOCK=true`

_./back/.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
+ IS_API_MOCK=true
MONGODB_URL=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET
```

Y como tenemos el contenedor de mongo configurado, vamos a asegurarnos que tenemos la carpeta del volumen creado:

```
back/mongo-data
```

Abrimos el navegador y ponemos esta dirección `http://localhost:8080`

Como tenemos las fotos de los usuarios en la carpeta `public` (`admin-avatar.png` and `user-avatar.png`), vamos a hacer un refactor del proyecto `back`:

> Tenemos las imágenes en esta carpeta: http://localhost:3000/admin-avatar.png or http://localhost:3000/user-avatar.png

Añadimos un nuevo campo a _user.model.ts_ (dal) y a _mock-data_:

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

Actualizamos el `api model`:

_.back/src/pods/user/user.api-model.ts_

```diff
import { Role } from '#core/models/index.js';

export interface User {
  email: string;
  role: Role;
+ avatar: string;
}

```

Y el `mapper`:

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

Las imágenes la tenemos en estas rutas:

- `http://localhost:3000/admin-avatar.png`
- `http://localhost:3000/user-avatar.png`

¿Vamos a tener problemas de `CORS`? (diferentes puertos para front y back), pues... hacemos un truco en local y configuramos un proxy en el front:

_./front/vite.config.js_

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

En local para el front están en el puerto `8080` bajo la carpeta `images` (el proxy hace un redirect al raiz del 3000)

- `http://localhost:8080/images/admin-avatar.png`
- `http://localhost:8080/images/user-avatar.png`

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
