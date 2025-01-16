# 01 Logging

In this example we are going to install and configure a logging library.

We will start from `00-boilerplate`.

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

There are many logging libraries for Nodejs like [Winston](https://github.com/winstonjs/winston), [Bunyan](https://github.com/trentm/node-bunyan), [Pino](https://github.com/pinojs/pino), [Loglevel](https://github.com/pimterry/loglevel), [Npmlog](https://github.com/npm/npmlog), etc. Which provide similar features, let's try the first one:

```bash
cd back
npm install winston
```

Let's configure a `logger` instance:

_./back/src/core/logger/logger.ts_

```javascript
import { createLogger, transports } from "winston";

const console = new transports.Console();

export const logger = createLogger({
  transports: [console],
});
```

Add barrel file:

_./back/src/core/logger/index.ts_

```javascript
export * from "./logger.js";
```

Replace `console.log` by `logger`:

_./back/src/index.ts_

```diff
...
import { createRestApiServer, dbServer } from '#core/servers/index.js';
import { ENV } from '#core/constants/index.js';
import { authenticationMiddleware } from '#core/security/index.js';
+ import { logger } from '#core/logger/index.js';
...

app.listen(ENV.PORT, async () => {
  if (!ENV.IS_API_MOCK) {
    await dbServer.connect(ENV.MONGODB_URL);
-   console.log('Running DataBase');
+   logger.info('Running DataBase');
  } else {
-   console.log('Running Mock API');
+   logger.info('Running Mock API');
  }
- console.log(`Server ready at port ${ENV.PORT}`);
+ logger.info(`Server ready at port ${ENV.PORT}`);
});

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

Let's add a warning message:

_./back/src/pods/security/security.api.ts_

```diff
import { Router } from 'express';
import jwt from 'jsonwebtoken';
import { UserSession } from '#core/models/index.js';
import { ENV } from '#core/constants/index.js';
+ import { logger } from '#core/logger/index.js';
...

securityApi
  .post('/login', async (req, res, next) => {
    try {
...
        res.sendStatus(204);
      } else {
        res.clearCookie('authorization');
+       logger.warn(`Invalid credentials for email ${email}`);
        res.sendStatus(401);
      }
    } catch (error) {
      next(error);
    }
  })
...
```

Open browser at `http://localhost:8080/` and type invalid credentials. For example:

```
admin@email.com
1234

```

Let's update the `logger.middlewares` too:

_./back/src/common/middlewares/logger.middlewares.ts_

```diff
import { RequestHandler, ErrorRequestHandler } from 'express';
+ import { Logger } from 'winston';

- export const logRequestMiddleware: RequestHandler =
+ export const logRequestMiddleware = (logger: Logger): RequestHandler =>
    async (req, res, next) => {
-     console.log(req.url);
+     logger.info(req.url);
      next();
    };

- export const logErrorRequestMiddleware: ErrorRequestHandler =
+ export const logErrorRequestMiddleware = (logger: Logger): ErrorRequestHandler =>
    async (error, req, res, next) => {
-    console.error(error);
+    logger.error(error.stack);
     res.sendStatus(500);
  };

```

Update app:

_./back/src/index.ts_

```diff
...

- app.use(logRequestMiddleware);
+ app.use(logRequestMiddleware(logger));

app.use('/api/security', securityApi);
app.use('/api/books', authenticationMiddleware, bookApi);
app.use('/api/users', authenticationMiddleware, userApi);

- app.use(logErrorRequestMiddleware);
+ app.use(logErrorRequestMiddleware(logger));
...

```

And simulate some unexpected error:

_./back/src/pods/book/book.api.ts_

```diff
...
booksApi
  .get('/', async (req, res, next) => {
    try {
+     const book = undefined;
+     book.name;
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
      const bookList = await bookRepository.getBookList(page, pageSize);
      res.send(mapBookListFromModelToApi(bookList));
    } catch (error) {
      next(error);
    }
  })

...
```

Open browser at `http://localhost:8080/` and type valid credentials.

```
admin@email.com
test

```

Even, we could customize the format for each transport:

_./back/src/core/logger/logger.ts_

```diff
- import { createLogger, transports } from 'winston';
+ import { createLogger, transports, format } from 'winston';

+ const { combine, colorize, timestamp, printf } = format;

- const console = new transports.Console();
+ const console = new transports.Console({
+   format: combine(
+     colorize(),
+     timestamp(),
+     printf(({ level, message, timestamp }) => {
+       return `[${level}] [${timestamp}] message: ${message}`;
+     })
+   ),
+ });

export const logger = createLogger({
  transports: [console],
});

```

> `combine`: method to combine multiple formats
>
> `colorize`: add color by log level
>
> `timestamp`: add a timestamp to log
>
> `printf`: create custom message
>
> Open browser at `http://localhost:8080/` and run `info`, `warn` and `error` logs.

The best feature in this kind of libraries is we can save our logs in different transports at the same time. Let's move `console` transport to its own file:

_./back/src/core/logger/transports/console.transport.ts_

```javascript
import { transports, format } from "winston";

const { combine, colorize, timestamp, printf } = format;

export const console = new transports.Console({
  format: combine(
    colorize(),
    timestamp(),
    printf(({ level, message, timestamp }) => {
      return `[${level}] [${timestamp}] message: ${message}`;
    })
  ),
});
```

Let's add a new `File` transport, for example, we will save only `warnings` and `errors` in this file:

_./back/src/core/logger/transports/file.transport.ts_

```javascript
import { transports, format } from "winston";

const { combine, timestamp, prettyPrint } = format;

export const file = new transports.File({
  filename: "app.log",
  format: combine(timestamp(), prettyPrint()),
  level: "warn", // Save level lower or equal than warning
  handleExceptions: true,
});
```

> [Logging levels](https://github.com/winstonjs/winston#logging-levels)
>
> `prettyPrint`: JSON format

Add barrel file:

_./back/src/core/logger/transports/index.ts_

```javascript
export * from "./console.transport.js";
export * from "./file.transport.js";
```

Update logger:

_./back/src/core/logger/logger.ts_

```diff
- import { createLogger, transports, format } from 'winston';
+ import { createLogger } from 'winston';
+ import { console, file } from './transports/index.js';

- const { combine, colorize, timestamp, printf } = format;

- const console = new transports.Console({
-   format: combine(
-     colorize(),
-     timestamp(),
-     printf(({ level, message, timestamp }) => {
-       return `[${level}] [${timestamp}] message: ${message}`;
-     })
-   ),
- });

export const logger = createLogger({
- transports: [console],
+ transports: [console, file],
});

```

Open browser at `http://localhost:8080/` and run `info`, `warn` and `error` logs. Check results in `./back/app.log`.

Add to .gitignore:

_./back/.gitignore_

```diff
node_modules
dist
.env
mongo-data
globalConfig.json
public
+ *.log
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
