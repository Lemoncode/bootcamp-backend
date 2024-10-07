# 02 Manual render deploy

In this example we are going to learn how to deploy app to render manually.

We will start from `01-production-bundle`.

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

[Render](https://render.com/) is a cloud provider that allows you to deploy different types of apps based on git repository changes.

First, we need to prepare the final files that we want to deploy, let's build the front project:

_front terminal_

```bash
npm run build

```

Let's copy the `front/dist` folder in the `back/public` folder.

Build the back project:

_back terminal_

```bash
npm run build

```

Now we have something like:

_./back_

```
|-- config/
|-- dist/
|-- node_module/
|-- public/
|-- src/
|-- ...
|-- package-lock.json
|-- package.json

```

Let's create a new empty repository to deploy our app placing the builded files:

![01-create-repo](./readme-resources/01-create-repo.png)

- Clone repository:

```bash
git clone git@github.com<url> .

```

> NOTE: Add a final dot to clone the repository in the current folder.

Let's copy all necessary files:

- Copy `back/dist` into `./`
- Copy `back/public` into `./public`
- And copy `back/package.json` into `./package.json`. Let's update imports and remove unnecessary dependencies.

_./package.json_

```diff
{
  "name": "bootcamp-backend-lemoncode",
  "type": "module",
  "scripts": {
-   "prestart": "node ./create-dev-env.js && docker compose down --remove-orphans",
-   "start": "run-p -l type-check:watch start:dev start:local-db",
-   "start:dev": "tsx --require dotenv/config --watch src/index.ts",
-   "prestart:console-runners": "npm run prestart",
-   "start:console-runners": "run-p type-check:watch console-runners start:local-db",
-   "console-runners": "tsx --require dotenv/config --watch src/console-runners/index.ts",
-   "start:local-db": "docker compose up -d",
-   "clean": "rimraf dist",
-   "build": "npm run clean && tsc --project tsconfig.prod.json",
-   "type-check": "tsc --noEmit --preserveWatchOutput",
-   "type-check:watch": "npm run type-check -- --watch",
-   "test": "vitest run -c ./config/test/config.ts",
-   "test:watch": "vitest watch -c ./config/test/config.ts"
+   "start": "node index.js"
  },
  "imports": {
-   "#*": "./src/*"
+   "#*": "./*"
  },
  "dependencies": {
    "@aws-sdk/client-s3": "^3.658.1",
    "@aws-sdk/s3-request-presigner": "^3.658.1",
    "cookie-parser": "^1.4.6",
    "cors": "^2.8.5",
    "express": "^4.21.0",
    "jsonwebtoken": "^9.0.2",
    "mongodb": "^6.9.0"
- },
+ }
- "devDependencies": {
-   "@types/cookie-parser": "^1.4.7",
-   "@types/cors": "^2.8.17",
-   "@types/express": "^5.0.0",
-   "@types/jsonwebtoken": "^9.0.7",
-   "@types/node": "^22.7.4",
-   "@types/prompts": "^2.4.9",
-   "@types/supertest": "^6.0.2",
-   "dotenv": "^16.4.5",
-   "mongodb-memory-server": "^10.0.1",
-   "npm-run-all": "^4.1.5",
-   "prompts": "^2.4.2",
-   "rimraf": "^6.0.1",
-   "supertest": "^7.0.0",
-   "tsx": "^4.19.1",
-   "typescript": "^5.6.2",
-   "vitest": "^2.1.1"
- }
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

Deploy it:

```bash
git add .
git commit -m "deploy app"
git push -u origin main

```

Create a new render app:

![02-create-render-app](./readme-resources/02-create-render-app.png)

![03-connect-github](./readme-resources/03-connect-github.png)

Configure web service:

![04-configure-web-service](./readme-resources/04-configure-web-service.png)

![05-configure-runtime](./readme-resources/05-configure-runtime.png)

![06-build-start-commands-and-instance-type](./readme-resources/06-build-start-commands-and-instance-type.png)

Add environment variables (Advanced settings):

![07-add-env-vars](./readme-resources/07-add-env-vars.png)

> [Specifying a Node Version in Render](https://render.com/docs/node-version)

Clicks on `Create Web Service` button.

After the successful deploy, open `https://<app-name>.onrender.com`.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
