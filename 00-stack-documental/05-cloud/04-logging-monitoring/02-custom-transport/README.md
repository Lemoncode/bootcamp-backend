# 02 Custom transport

In this example we are going to create a custom logger transport.

We will start from `01-logging`.

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

In this example, we will create a custom logger transport, in this case, we will send the logs to [rollbar](https://rollbar.com/).

First, let's create a new account and select the SDK language:

![01-select-rollbar-language](./readme-resources/01-select-rollbar-language.png)

Now, rollbar is waiting for data from our app:

![02-waiting-to-send-messages](./readme-resources/02-waiting-to-send-messages.png)

Let's install rollbar:

```bash
npm install rollbar --save

```

Since there isn't any official winston rollbar transport, we will create a custom transport. We will explicitly install `winston`'s internal dependencies to create new transport:

```bash
npm install winston-transport --save
```

> [Adding custom transports](https://github.com/winstonjs/winston#adding-custom-transports)

Add rollbar transport:

_./back/src/common/logger-transports/rollbar.transport.ts_

```javascript
import Transport from 'winston-transport';
import Rollbar, { Configuration } from 'rollbar';

type Config = Transport.TransportStreamOptions & Configuration;

export class RollbarTransport extends Transport {
  private config: Config;
  private rollbar: Rollbar;

  constructor(config: Config = {}) {
    super(config);

    this.config = config;
    this.rollbar = new Rollbar({
      ...this.config,
    });
  }

  log(info, next) {
    setImmediate(() => this.emit('logged', info));
    const level = info.level;
    const message = info.message;

    if (level === 'warn' || level === 'error') {
      this.rollbar[level](message);
    }
    next();
  }
}

```

> In future, we could create a custom library for it.

Add barrel file

_./back/src/common/logger-transports/index.ts_

```javascript
export * from './rollbar.transport';

```

Add rollbar transport instance (NOTE: Pending to add constants):

_./back/src/core/logger/transports/rollbar.transport.ts_

```javascript
import { RollbarTransport } from 'common/logger-transports';
import { envConstants } from 'core/constants';

export const rollbar = new RollbarTransport({
  accessToken: envConstants.ROLLBAR_ACCESS_TOKEN,
  environment: envConstants.NODE_ENV,
  captureUncaught: envConstants.isProduction,
  captureUnhandledRejections: envConstants.isProduction,
});

```

> [Rollbar docs](https://docs.rollbar.com/docs/nodejs)

Update barrel file:

_./back/src/core/logger/transports/index.ts_

```diff
export * from './console.transport';
export * from './file.transport';
+ export * from './rollbar.transport';

```

Add `env variables`:

_./back/.env.example_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
API_MOCK=true
MONGODB_URI=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET
+ ROLLBAR_ACCESS_TOKEN=value

```

_./back/.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
API_MOCK=true
MONGODB_URI=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET
+ ROLLBAR_ACCESS_TOKEN=<value-provided-by-rollbar>

```

_./back/src/core/constants/env.constants.ts_

```diff
export const envConstants = {
+ NODE_ENV: process.env.NODE_ENV,
  isProduction: process.env.NODE_ENV === 'production',
  PORT: process.env.PORT,
  STATIC_FILES_PATH: process.env.STATIC_FILES_PATH,
  isApiMock: process.env.API_MOCK === 'true',
  MONGODB_URI: process.env.MONGODB_URI,
  AUTH_SECRET: process.env.AUTH_SECRET,
+ ROLLBAR_ACCESS_TOKEN: process.env.ROLLBAR_ACCESS_TOKEN,
};

```

Use transport:

_./back/src/core/logger/logger.ts_

```diff
import { createLogger } from 'winston';
- import { console, file } from './transports';
+ import { console, file, rollbar } from './transports';

export const logger = createLogger({
- transports: [console, file],
+ transports: [console, file, rollbar],
  exitOnError: false,
});

```

Open browser at `http://localhost:8080/` and run `info`, `warn` and `error` logs. Check results in rollbar.

![03-rollbar-dashboard](./readme-resources/03-rollbar-dashboard.png)

Another features supported by Rollbar:

- [Deploys and Versions](https://docs.rollbar.com/docs/deploy-tracking): you could correlate deploys and version with customer issues
- [RQL](https://docs.rollbar.com/docs/rql): Rollbar Query Language provides SQL-like interface over Rollbar data.
- [Notifications](https://docs.rollbar.com/docs/notifications): send by email, slack or another app errors occurences.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
