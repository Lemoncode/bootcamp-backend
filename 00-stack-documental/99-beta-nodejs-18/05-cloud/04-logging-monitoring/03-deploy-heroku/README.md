# 03 Deploy Render

In this example we are going to deploy app to `Render`.

We will start from `02-custom-transport`.

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

Let's update the code for production:

_./back/src/core/logger/logger.ts_

```diff
import { createLogger } from 'winston';
+ import Transport from 'winston-transport';
+ import { envConstants } from '#core/constants/index.js';
import { console, file, rollbar } from './transports/index.js';

+ let transports: Transport[] = [console, file];
+ if (envConstants.isProduction) {
+   transports = [...transports, rollbar];
+ }

export const logger = createLogger({
- transports: [console, file, rollbar],
+ transports,
});

```

Update Dockerfile to works with api mock, only for demo purpose:

_./Dockerfile_

```diff
...

+ ENV NODE_ENV=production
ENV STATIC_FILES_PATH=./public
- ENV API_MOCK=false
+ ENV API_MOCK=true
ENV CORS_ORIGIN=false

...
```

Create new repository and upload files:

![01-create-repo](./readme-resources/01-create-repo.png)

```bash
git init
git remote add origin git@github.com...
git add .
git commit -m "initial commit"
git push -u origin main

```

Create a new _Render_ app:

![02-create-render-app](./readme-resources/02-create-render-app.png)

Configure account to get access to the new repository:

![03-configure-account](./readme-resources/03-configure-account.png)

Configure web service:

![04-configure-web-service](./readme-resources/04-configure-web-service.png)

![05-configure-runtime](./readme-resources/05-configure-runtime.png)

Add environment variables (Advanced settings):

![06-add-env-vars](./readme-resources/06-add-env-vars.png)

Update docker settings:

![07-docker-settings](./readme-resources/07-docker-settings.png)

Clicks on `Create Web Service` button.

Open browser at `https://<app-name>.onrender.com` and run `info`, `warn` and `error` logs.

Check results in rollbar, remember filter by environment:

![09-rollbar-env-filter](./readme-resources/09-rollbar-env-filter.png)

Checks logs in `Render`:

![10-render-logs](./readme-resources/10-render-logs.png)

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
