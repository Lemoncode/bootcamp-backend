# 01 Logging

En este ejemplo vamos a instalar y configurar una librería de logging.

Tomaremos como punto de partida el ejemplo _00-boilerplate_.

# Pasos

Ejecuta `npm install` para instalar las librerías necesarias:

```bash
cd front
npm install
```

Y en otro terminal:

```bash
cd back
npm install

```

Existen muchas librerías de logging para Nodejs como [Winston](https://github.com/winstonjs/winston), [Bunyan](https://github.com/trentm/node-bunyan), [Pino](https://github.com/pinojs/pino), [Loglevel](https://github.com/pimterry/loglevel), [Npmlog](https://github.com/npm/npmlog), etc. y tienen funcionalidades similares, vamos a probar la primera:

```bash
npm install winston
```

Vamosa configurar una instancia de `logger`:

_./back/src/core/logger/logger.ts_

```javascript
import { createLogger, transports } from "winston";

const console = new transports.Console();

export const logger = createLogger({
  transports: [console],
});
```

Creamos un archivo de `barrel`:

_./back/src/core/logger/index.ts_

```javascript
export * from "./logger.js";
```

Reemplazamos `console.log` por `logger`:

_./back/src/index.ts_

```diff
...
import { createRestApiServer, connectToDBServer } from '#core/servers/index.js';
import { envConstants } from '#core/constants/index.js';
+ import { logger } from '#core/logger/index.js';
...

restApiServer.listen(envConstants.PORT, async () => {
  if (!envConstants.isApiMock) {
    await connectToDBServer(envConstants.MONGODB_URI);
-   console.log('Connected to DB');
+   logger.info('Connected to DB');
  } else {
-   console.log('Running API mock');
+   logger.info('Running API mock');
  }
- console.log(`Server ready at port ${envConstants.PORT}`);
+ logger.info(`Server ready at port ${envConstants.PORT}`);
});

```

Lanzamos los proyectos `front` y `back` para comprobar la implementación actual:

_front terminal_

```bash
npm start

```

_back terminal_

```bash
npm start

```

Vamos a añadir un mensaje de `warning` para que lo procese el log.

_./back/src/pods/security/security.rest-api.ts_

```diff
import { Router } from 'express';
import jwt from 'jsonwebtoken';
import { UserSession } from '#common-app/models/index.js';
+ import { logger } from '#core/logger/index.js';
import { envConstants } from '#core/constants/index.js';
...

securityApi
  .post('/login', async (req, res, next) => {
    try {
...
        res.sendStatus(204);
      } else {
+       logger.warn(`Invalid credentials for email ${email}`);
        res.sendStatus(401);
      }
    } catch (error) {
      next(error);
    }
  })
...
```

Abrimos el navegador en `http://localhost:8080/` y tecleamos credenciales incorrectas.

```
admin@email.com
1234

```

Vamos ahora a actualizar los `logger.middlewares`:

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

Actualizamos la aplicación:

_./back/src/index.ts_

```diff
...

- restApiServer.use(logRequestMiddleware);
+ restApiServer.use(logRequestMiddleware(logger));

restApiServer.use('/api/security', securityApi);
restApiServer.use('/api/books', authenticationMiddleware, booksApi);
restApiServer.use('/api/users', authenticationMiddleware, userApi);

- restApiServer.use(logErrorRequestMiddleware);
+ restApiServer.use(logErrorRequestMiddleware(logger));
...

```

Y vamos a simular que se lanza un error no esperado:

_./back/src/pods/book/book.rest-api.ts_

```diff
...
booksApi
  .get('/', authorizationMiddleware(), async (req, res, next) => {
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

Abrimos el navegador en `http://localhost:8080/` y tecleamos credenciales correctas.

```
admin@email.com
test

```

Incluso podemos personalizar el formato de los mensajes de log para cada tipo de transporte:

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

Una de las características más potentes de este tipo de librerías es que podemos guardar los logs en diferentes transportes a la vez. Vamos a mover el transporte `console` a su propio archivo:

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

Abrimos el navegador en `http://localhost:8080/` y forzamos errores de `info`, `warn` y `error` logs.

Añadimos un nuevo transporte `File` en el que, por ejemplo, guardaremos solo `warnings` y `errors` en este archivo:

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

Y lo añadimos al barrel:

_./back/src/core/logger/transports/index.ts_

```javascript
export * from "./console.transport.js";
export * from "./file.transport.js";
```

Actualizamos el logger:

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

Abrimos el navegador en `http://localhost:8080/` y forzamos errores de `info`, `warn` y `error` logs. Comprobamos los resultados en `./back/app.log`.

Importante... ignorar esos archivos en nuestro repo, lo añadimos al `.gitignore`:

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
