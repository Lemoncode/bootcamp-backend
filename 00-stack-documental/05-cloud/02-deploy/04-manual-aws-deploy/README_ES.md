# 04 Manual AWS deploy

En este ejemplo vamos a aprender a desplegar manualmente en AWS.

Tomamos como punto de partida el ejemplo `03-mongo-deploy`.

# Pasos

Si no lo has hecho ya, vamos a hacer `npm install` en front y back:

Abrimos un terminal:

```bash
cd front
npm install

```

Y abrimos un segundo terminal:

```bash
cd back
npm install

```

Esta vez, vamos a desplegar nuestra aplicación web en AWS usando el servicio [Beanstalk](https://aws.amazon.com/elasticbeanstalk/).

Vamos a crear una beanstalk app:

![01-create-beanstalk-app](./readme-resources/01-create-beanstalk-app.png)

Le damos un nombre:

![02-give-app-name](./readme-resources/02-give-app-name.png)

El siguiente paso es elegir la plataforma, en nuestro caso NodeJS:

![03-choose-platform](./readme-resources/03-choose-platform.png)

Antes de subir el código, vamos a crear un `zip` con los mismos ficheros que desplegamos en el ejemplo de `Render`. Copiemos todos los ficheros necesarios:

> Podemos tirar del corte que tenemos en el repo de render.

Por si acaso recordamos los pasos

- El contenido de `dist`.
- Y el de `public` folder.

Y creamos un el `package.json` con las dependencias de producción, el comando para arrancar, y los alias apuntando a raíz en vez de `src`.

_./package.json_

```json
{
  "name": "bootcamp-backend",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "type": "module",
  "scripts": {
    "start": "node index.js"
  },
  "imports": {
    "#common/*": "./common/*",
    "#common-app/*": "./common-app/*",
    "#core/*": "./core/*",
    "#dals/*": "./dals/*",
    "#pods/*": "./pods/*"
  },
  "keywords": [],
  "author": "",
  "license": "ISC",
  "dependencies": {
    "@aws-sdk/client-s3": "^3.281.0",
    "@aws-sdk/s3-request-presigner": "^3.282.0",
    "cookie-parser": "^1.4.6",
    "cors": "^2.8.5",
    "dotenv": "^16.0.3",
    "express": "^4.18.2",
    "jsonwebtoken": "^8.5.1",
    "mongodb": "^4.12.1"
  }
}
```

El resultado quedaría tal que así:

```
|- common/
|- common-app/
|- core/
|- dals/
|- pods/
|- public/
|- index.js
|- package.json

```

Vamos a crear un fichero `zip`:

![04-create-zip-file](./readme-resources/04-create-zip-file.png)

Submos el código:

![05-upload-code](./readme-resources/05-upload-code.png)

Continuamos a los siguientes pasos:

![06-preset](./readme-resources/06-preset.png)

Definimos valores por defecto y añadimos las variable de entorno `en variables` en el paso 5:

![07-add-env-variables](./readme-resources/07-add-env-variables.png)

> NOTA: Como el aws security group está configurado para trabajar sólo con HTTP, tenemos que poner `NODE_ENV` a `development` para que no se cree la cookie con el flag `secure` (en AWS no tenemos _gratis_ _https_ toca o bien comprar un certificado, o liarse a configurarlo con _letsencrypt_ a bajo nivel).

Ahora toca esperar un pcoco y que se despliege (acuerdat de acceder por http)

Cuando tengamos todo listo, verás que aparece una etiqueta _Dominio_ donde podemos abrir la aplicación.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
