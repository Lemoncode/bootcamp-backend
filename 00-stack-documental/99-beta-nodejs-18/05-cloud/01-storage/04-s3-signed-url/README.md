# 04 S3 signed url

In this example we are going to learn how to work with s3 signed URLs.

We will start from `03-s3-sdk`.

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

We are going to install the official [@aws-sdk/s3-request-presigner](https://www.npmjs.com/package/@aws-sdk/s3-request-presigner):

```bash
npm install @aws-sdk/s3-request-presigner --save

```

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
+   console.log({ url });
  } catch (error) {
    console.error(error);
  }
};

```

> The default `expiresIn` is 900 seconds
>
> The max value in signed v4 is 7 days.
>
> [More info](https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-query-string-auth.html)

Run with `debug terminal` in back project:

```bash
npm run start:console-runners

```

We could update the `mapper` to retrieve signed urls from S3:

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

Update `mock-data`:

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

> We need the exact name stored in S3 bucket without slash `/`.
>
> If we store images inside a folder, we should include it, for example, `images/user-avatar-in-s3.png`.

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

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
