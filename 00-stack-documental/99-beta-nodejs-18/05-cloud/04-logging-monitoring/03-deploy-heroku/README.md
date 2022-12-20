# 03 Deploy heroku

In this example we are going to deploy app to Heroku.

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
+ import { envConstants } from 'core/constants';
import { console, file, rollbar } from './transports';

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

ENV NODE_ENV=production
ENV STATIC_FILES_PATH=./public
- ENV API_MOCK=false
+ ENV API_MOCK=true
ENV CORS_ORIGIN=false

...
```

We will create a new heroku app:

![01-create-heroku-app](./readme-resources/01-create-heroku-app.png)

![02-create-heroku-app](./readme-resources/02-create-heroku-app.png)

Create new repository and upload files:

```bash
git init
git remote add origin git@github.com...
git add .
git commit -m "initial commit"
git push -u origin main

```

We need an [auth token](https://devcenter.heroku.com/articles/heroku-cli-commands#heroku-authorizations-create) to login inside Github Action job:

```bash
heroku login
heroku authorizations:create -d <description>
```

> -d: Set a custom authorization description
> -e: Set expiration in seconds (default no expiration)
> `heroku authorizations`: Get auth token list.

Add `Auth token` to git repository secrets:

![03-github-secret](./readme-resources/03-github-secret.png)

![04-token-as-secret](./readme-resources/04-token-as-secret.png)

> [Heroku API KEY storage](https://devcenter.heroku.com/articles/heroku-cli-commands#heroku-authorizations-create)

We will add `HEROKU_APP_NAME` as secret too:

![05-heroku-app-name](./readme-resources/05-heroku-app-name.png)

> We need Heroku app name as identifier Heroku deployment.

Run again github actions:

![06-open-failed-job](./readme-resources/06-open-failed-job.png)

![07-re-run-job](./readme-resources/07-re-run-job.png)

Add heroku env variables:

![08-heroku-env-variables](./readme-resources/08-heroku-env-variables.png)

> Include the `AUTH_SECRET` to get valid JWT token-

Open browser at `https://<app-name>.herokuapp.com/` and run `info`, `warn` and `error` logs.

Check results in rollbar, remember filter by environment:

![09-rollbar-env-filter](./readme-resources/09-rollbar-env-filter.png)

Checks logs in heroku:

```bash
heroku logs --tail -a <app-name>
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
