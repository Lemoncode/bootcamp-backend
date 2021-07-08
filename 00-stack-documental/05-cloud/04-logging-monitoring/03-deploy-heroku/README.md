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

_./back/src/core/logger/transports/rollbar.transport.ts_

```diff
import { RollbarTransport } from 'common/logger-transports';
import { envConstants } from 'core/constants';

export const rollbar = new RollbarTransport({
  accessToken: envConstants.ROLLBAR_ACCESS_TOKEN,
  environment: envConstants.NODE_ENV,
  captureUncaught: envConstants.isProduction,
  captureUnhandledRejections: envConstants.isProduction,
});

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
