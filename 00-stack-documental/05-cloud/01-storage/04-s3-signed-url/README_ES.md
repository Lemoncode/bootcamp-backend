# 04 S3 signed url

En este ejemplo vamos a aprender a trabajar con s3 signed URLs.

Tomamos como punto de partida el ejemplo `03-s3-sdk`.

# Pasos

Si no lo tuvieras funcionando, ... toca hacer lo _install_ pertinentes.

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

Nos toca instalar una librerías más de [@aws-sdk/s3-request-presigner](https://www.npmjs.com/package/@aws-sdk/s3-request-presigner) en este caso la que nos va a permitir firmar urls.

** nos ponemos debajo de _back_ **

```bash
cd back
```

```bash
npm install @aws-sdk/s3-request-presigner --save
```

Y vamos ahora a obtener una url firmada para poder acceder a un recurso privado de S3.

_./back/src/console-runners/s3.runner.ts_

```diff
import {
  S3Client,
  ListObjectsCommand,
  GetObjectCommand,
  PutObjectCommand,
} from '@aws-sdk/client-s3';
- import fs from 'fs';
- import path from 'path';
+ import { getSignedUrl } from '@aws-sdk/s3-request-presigner';

export const run = async () => {
  try {
    const client = new S3Client({ region: 'eu-west-3' });
    const bucket = 'bucket-name';
    const fileName = 'user-avatar-in-s3.png';
-   const imageStream = fs.createReadStream(path.resolve('./', fileName));
-   const command = new PutObjectCommand({
+   const command = new GetObjectCommand({
      Bucket: bucket,
      Key: fileName,
-     Body: imageStream,
    });
-   const data = await client.send(command);
-   console.log({ data });
+   const url = await getSignedUrl(client, command);
+   console.log(url);
  } catch (error) {
    console.error(error);
  }
};

```

> La url por defecto expira en 900 segundos `expiresIn`
>
> El tiempo máximo de expiración en una url firmada v4 es de 7 días.
>
> [More info](https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-query-string-auth.html)

Run with `debug terminal` in back project:

```bash
cd back
```

```bash
npm run start:console-runners
```

Probamos y vemos que obtene os la URL firmada, si copiamos y pegamos en el navegador, veremos que podemos acceder a la imagen (ya que está firmada).

Vamos a intentar usar esto en nuestro backend:

Actualizamos el `mapper` para que recupere las urls firmadas de S3:

_./back/src/core/clients/s3.client.ts_

```javascript
import { S3Client } from '@aws-sdk/client-s3';

export const s3Client = new S3Client({ region: 'eu-west-3' });
```

Add barrel file:

_./back/src/core/clients/index.ts_

```javascript
export * from './s3.client.js';
```

Update `mapper`:

_./back/src/pods/user/user.mappers.ts_

```diff
+ import { getSignedUrl } from '@aws-sdk/s3-request-presigner';
+ import { GetObjectCommand } from '@aws-sdk/client-s3';
+ import { s3Client } from '#core/clients/index.js';
import * as model from '#dals/index.js';
import * as apiModel from './user.api-model.js';

+ // TODO: Move to env variable
+ const bucket = 'bucket-name';

+ const mapAvatar = async (avatar: string): Promise<string> => {
+   const command = new GetObjectCommand({
+     Bucket: bucket,
+     Key: avatar,
+   });
+   const expiresIn = 60 * 60 * 24; // 1 day expiration time.
+   return await getSignedUrl(s3Client, command, { expiresIn });
+ };

- export const mapUserFromModelToApi = (user: model.User): apiModel.User => ({
-   email: user.email,
-   role: user.role,
-   avatar: user.avatar,
- });

+ export const mapUserFromModelToApi = async (
+   user: model.User
+ ): Promise<apiModel.User> => {
+   const avatar = await mapAvatar(user.avatar);
+   return {
+     email: user.email,
+     role: user.role,
+     avatar,
+   };
+ };


```

Update `rest-api`:

_./back/src/pods/user/user.rest-api.ts_

```diff
import { Router } from 'express';
import { userRepository } from '#dals/index.js';
import { mapUserFromModelToApi } from './user.mappers.js';

export const userApi = Router();

userApi.get('/', async (req, res, next) => {
  try {
    const user = await userRepository.getUserById(req.userSession.id);
-   res.send(mapUserFromModelToApi(user));
+   const apiUser = await mapUserFromModelToApi(user);
+   res.send(apiUser);
  } catch (error) {
    next(error);
  }
});

```

Actualizamos `mock-data`:

_./back/src/dals/mock-data.ts_

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
-     avatar: 'https://<bucket-name>.s3.<region>.amazonaws.com/admin-avatar-in-s3.png',
+     avatar: 'admin-avatar-in-s3.png',
    },
    {
      _id: new ObjectId(),
      email: 'user@email.com',
      password: 'test',
      salt: '',
      role: 'standard-user',
-     avatar: '/user-avatar.png',
+     avatar: 'user-avatar-in-s3.png',
    },
  ],
...
```

> Necesitamos el nombre exacto almacenado en el bucket de S3 sin la barra `/`.
>
> Si almacenamos imagenes dentro de una carpeta también tendríamos que incluir ese nombre de carpeta, por ejemplo, `images/user-avatar-in-s3.png`.

Vamos a ejecutar `front` y `back` y a ver que tal tira la implementación:

_front terminal_

```bash
npm start

```

_back terminal_

```bash
npm start

```

Abrimos el navegador `http://localhost:8080`

Y al logarnos podemos ver que se ha cargado la imagen desde S3 (inspect y ver _img_ el _src_).

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
