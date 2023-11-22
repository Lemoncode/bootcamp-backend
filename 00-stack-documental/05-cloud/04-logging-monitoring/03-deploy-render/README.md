# 03 Deploy Render

En este ejemplo vamos a desplegar la aplicación en `Render`.

Tomamos como punto de partida el ejemplo `02-custom-transport`.

# Steps to build it

Si no lo hemos hecho antes, instalamos las dependencias de back y front.

En un terminal:

```bash
cd front
npm install

```

Abrimos un segundo terminal:

```bash
cd back
npm install

```

Esto de `Rollbar` esta muy bien, pero para que nos envíe y almacen logs de producción o de entornos pre, pero en nuestro local es un poco rollo ir metiendo trallazos desde localhost, así que vamos a configurarlo de forma que solo se envíen los logs a `Rollbar` cuando estemos en producción.

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

Acualizamos el docker file para usar de momento al API_MOCK (más adelante lo actualizarekos, o incluso en el propio `Render`)

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

Creamos un nuevo repositorio y subimos los ficheros (aquí podemos hacerlo por comando, o usar el método sucio y rápido que vimos antes :)):

![01-create-repo](./readme-resources/01-create-repo.png)

```bash
git init
git remote add origin git@github.com...
git add .
git commit -m "initial commit"
git push -u origin main

```

Creamos una nueva app en `Render`:

![02-create-render-app](./readme-resources/02-create-render-app.png)

Configuramos la cuenta para tener acceso al nuevo repositorio:

![03-configure-account](./readme-resources/03-configure-account.png)

Configuramos el web service:

![04-configure-web-service](./readme-resources/04-configure-web-service.png)

![05-configure-runtime](./readme-resources/05-configure-runtime.png)

Añadimos la variables de entorno (Advanced settings):

![06-add-env-vars](./readme-resources/06-add-env-vars.png)

OJO, comprobar si tenemos que poner node_env a production (se supone que en `Render` lo hace por nosotros), en nuestro index podemos comprobarlo:

_./back/src/index.ts_

```diff
  logger.info(`Server ready at port ${envConstants.PORT}`);
+  console.log('******Is Production', envConstants.isProduction);
});
```

> Nos puede tardar en que se muestren los errores en rollbar

Actualizamos los docker settings:

![07-docker-settings](./readme-resources/07-docker-settings.png)

Clicamos en el botón `Create Web Service`:

Abrimos el navegador en la url: `https://<app-name>.onrender.com` y le damos caña para que genera eventos de `info`, `warn` y `error` logs.

Vamos a ver si han llegado los logs a `Rollbar`, IMPORTANTE acuérdate de filtrar por entorno (seguramente no esté `producción` seleccionado y te salga en blanco)

![09-rollbar-env-filter](./readme-resources/09-rollbar-env-filter.png)

Vamos a ver los logs en `Render`:

![10-render-logs](./readme-resources/10-render-logs.png)

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
