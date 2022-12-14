# 01 Production bundle

In this example we are going to learn how to create a production bundle.

We will start from `00-boilerplate/back`.

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

Run `front` and `back` projects to check current implementation:

_front terminal_

```bash
npm start

```

_back terminal_

```bash
npm start

```

Open browser in `http://localhost:8080`

Since we are using `restApiServer.use('/', express.static(staticFilesPath));` we can build `front` app and place all files there:

_front terminal_

```bash
npm run build:prod

```

Copy `front/dist` files into `back/public`.

Update `gitignore`

_back/.gitignore_

```diff
node_modules
.env
mongo-data
globalConfig.json
+ public
+ dist

```

Run only `back` app:

_back terminal_

```bash
npm start

```

Open browser in `http://localhost:3000`

The second step is create a new `npm command` in `back` project to compile `ts` into `js` files, in this case, we will use babel, but first, we will install `rimraf` library to clean `dist` folder before run a new build and `cross-env` to create inline env variable:

_back terminal_

```bash
npm install rimraf cross-env --save-dev

```

_./back/package.json_

```diff
...
  "scripts": {
    ...
    "test:watch": "npm run test -- --watchAll -i",
+   "clean": "rimraf dist",
+   "build": "npm run type-check && npm run clean && npm run build:prod",
+   "build:prod": "cross-env NODE_ENV=production babel src -d dist --extensions \".ts\""
  },
...
```

Let's try it:

_back terminal_

```bash
npm run build

```

A nice improvement is ignore not necessary files for production:

_./back/.babelrc_

```diff
...
  "env": {
    "development": {
      "sourceMaps": "inline"
    },
+   "production": {
+     "ignore": ["**/*.spec.ts", "./src/console-runners"]
+   }
  }
}

```

Let's try it:

_back terminal_

```bash
npm run build

```

We could run this `production` version using node:

_back terminal_

```bash
node dist

```

> NOTE: Since `src` has same path route level as `dist` we provide valid env variables with `.env` file like public folder path, etc.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
