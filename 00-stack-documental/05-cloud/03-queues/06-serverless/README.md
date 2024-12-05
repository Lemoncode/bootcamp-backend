# 06 Serverless

In this example we are going to user serverless framework.

We will start from `05-topics`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
cd back
npm install

cd consumers
npm install

```

As a first approach, we could use [AWS Lambda](https://aws.amazon.com/lambda/) or whatever Cloud Provider function and write code in the platform and use it.

But we are going to use the [serverless framework](https://www.serverless.com/framework/docs/getting-started) which could deploy serverless function to several providers like AWS, Azure, Google, Tencent, Alibaba Cloud, etc. and keep the code in our repository.

```bash
npm install -g serverless

```

> Install serverless as global dependency because it's like a CLI tool.
>
> It has a [cli](https://github.com/serverless/serverless) to create a serverless template project.
>
> [Cloud providers](https://www.serverless.com/framework/docs/providers)

Create the first function:

_./back/serverless.yml_

```yml
service: my-back-app
frameworkVersion: '3'

provider:
  name: aws
  region: eu-west-3
  runtime: nodejs18.x
  memorySize: 128

functions:
  simple-api:
    handler: ./src/simple-api.handler
    events:
      - httpApi:
          path: /
          method: get

```

> [Serverless.yml](https://www.serverless.com/framework/docs/providers/aws/guide/serverless.yml)
>
> [AWS Lambda pricings](https://aws.amazon.com/lambda/pricing/)

Create `simple-api` file:

_./back/src/simple-api.js_

```javascript
export const handler = async (event) => {
  return {
    statusCode: 200,
    body: JSON.stringify({
      message: 'Hello from function',
    }),
  };
};

```

We will running offline (instead of deploy to aws) using [serverless-offline](https://github.com/dherault/serverless-offline):

```bash
cd back
npm install serverless-offline --save-dev

```

Update `package.json`:

```diff
...
  "scripts": {
    "prestart": "sh ./create-dev-env.sh",
    "start": "run-p -l type-check:watch start:dev start:local-db",
    "start:dev": "nodemon --transpileOnly --esm src/index.ts",
    "start:console-runners": "run-p -l type-check:watch console-runners start:local-db",
    "console-runners": "nodemon --no-stdin --transpileOnly --esm src/console-runners/index.ts",
    "start:local-db": "docker-compose up -d",
+   "start:serverless": "serverless offline",
    "clean": "rimraf dist",
...
  },
...

```

Using plugin in config:

_./back/serverless.yml_

```diff
service: my-back-app
frameworkVersion: '3'

provider:
  name: aws
  region: eu-west-3
  runtime: nodejs16.x
  memorySize: 128

functions:
  simple-api:
    handler: ./src/simple-api.handler
    events:
      - httpApi:
          path: /
          method: get

+ plugins:
+   - serverless-offline

```

> [Using plugins](https://www.serverless.com/framework/docs/guides/plugins)

Run it:

```bash
npm run start:serverless

```

> Open `http://localhost:3000`

Let's use `typescript with serverless`:

> There is a [serverless plugin](https://www.serverless.com/plugins/serverless-plugin-typescript) but it's doesn't provide any watch mode, it always compiles all the files and you need to restart the serverless process.

Rename file to `simple-api.ts`.

Update `package.json`:

_./back/package.json_

```diff
-   "start:serverless": "serverless offline",
+   "start:serverless": "run-p -l \"build -- --watch\" serverless",
+   "serverless": "serverless offline --reloadHandler",
```

Update serverless config:

_./back/serverless.yml_

```diff
...

functions:
  simple-api:
-   handler: ./src/simple-api.handler
+   handler: ./dist/simple-api.handler
    events:
...

```

Run it:

```bash
npm run start:serverless

```

Now we can see the package which this framework will be use to deploy:

```bash
serverless package

```

Looking at the .zip file, it copies a lot of files. Let's ignore it:

_./back/serverless.yml_

```diff

plugins:
  - serverless-offline

+ package:
+   patterns:
+     - '!./**'
+     - 'dist/simple-api.js'

```

Run again:

```bash
serverless package

```

> With `serverless deploy` creates the package and deploy it to AWS.

Ignore the `.serverless` folder:

_./back/.gitignore_

```diff
node_modules
dist
.env
mongo-data
globalConfig.json
public
+ .serverless
```

There is available [serverless-http](https://github.com/dougmoscrop/serverless-http) plugin to work with express and serverless.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
