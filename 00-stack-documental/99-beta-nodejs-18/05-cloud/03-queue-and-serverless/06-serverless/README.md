# 06 Serverless

In this example we are going to user serverless framework.

We will start from `05-serverless`.

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
  runtime: nodejs16.x
  memorySize: 128

functions:
  simple-api:
    handler: ./src/simple-api.handler
    events:
      - httpApi:
          path: /
          method: get

```

> [AWS Lambda pricings](https://aws.amazon.com/lambda/pricing/)

Create `simple-api` file:

_./back/src/simple-api.js_

```javascript
module.exports.handler = async (event) => {
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
    "start": "run-p -l type-check:watch start:dev",
    "start:dev": "nodemon --exec babel-node --extensions \".ts\" src/index.ts",
    "prestart:console-runners": "npm run prestart",
    "start:console-runners": "run-p type-check:watch console-runners",
    "console-runners": "npm run type-check && nodemon --no-stdin --exec babel-node -r dotenv/config --extensions \".ts\" src/console-runners/index.ts",
+   "start:serverless": "serverless offline",
    "type-check": "tsc --noEmit",
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

Run it:

```bash
npm run start:serverless

```

Let's use `typescript with serverless`:

> There are a [serverless plugin](https://www.serverless.com/plugins/serverless-plugin-typescript) but it has its own builder outside babel.

Rename file to `.ts`:

_./back/src/simple-api.ts_

```diff
- module.exports.handler = async (event) => {
+ export const handler = async (event) => {
  return {
    statusCode: 200,
    body: JSON.stringify({
      message: 'Hello from function',
    }),
  };
};

```

Using plugin in config:

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
npm run build
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

There is available [serverless-http](https://github.com/dougmoscrop/serverless-http) plugin to work with express and serverless.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
