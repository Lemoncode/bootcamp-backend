# 04 Manual AWS deploy

In this example we are going to learn how to manual deploy in AWS.

We will start from `03-mongo-deploy`.

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

This time, we will deploy our web app in AWS using the [Beanstalk Service](https://aws.amazon.com/elasticbeanstalk/).

Let's create a beanstalk app:

![01-create-beanstalk-app](./readme-resources/01-create-beanstalk-app.png)

Give a name:

![02-give-app-name](./readme-resources/02-give-app-name.png)

Choose NodeJS platform:

![03-choose-platform](./readme-resources/03-choose-platform.png)

Before upload the code, we will create a `zip` file with same files that we deploy on Heroku example. Let's copy all necessary files:

- `dist` folder content.
- `public` folder.

Create `package.json`:

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
    "#common/*.js": "./common/*.js",
    "#common-app/*.js": "./common-app/*.js",
    "#core/*.js": "./core/*.js",
    "#dals/*.js": "./dals/*.js",
    "#pods/*.js": "./pods/*.js"
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

Result:

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

Create `zip` file:

![04-create-zip-file](./readme-resources/04-create-zip-file.png)

Update code:

![05-upload-code](./readme-resources/05-upload-code.png)

Let's add `env variables`:

![06-configuring-env-variables](./readme-resources/06-configuring-env-variables.png)

![07-add-env-variables](./readme-resources/07-add-env-variables.png)

> NOTE: Since aws security group was configured only with HTTP inbound rule, we have to set `NODE_ENV` equals `development` to avoid create Cookie with secure flag.

Create app:

![08-create-app](./readme-resources/08-create-app.png)

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
