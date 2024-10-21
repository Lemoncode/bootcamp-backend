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

Since we are using `express.static` we can build `front` app and place all files in `ENV.STATIC_FILES_PATH` path:

_front terminal_

```bash
npm run build

```

Copy `front/dist` files into `back/public`.

Update `gitignore`

_back/.gitignore_

```diff
node_modules
dist
.env
mongo-data
+ public

```

Run only `back` app:

_back terminal_

```bash
npm start

```

Open browser in `http://localhost:3000`

The second step is create a new `npm command` in `back` project to compile `ts` into `js` files, in this case, we will use `tsc`(the TypeScript Compiler):

Add `tsconfig.prod.json` file:

_./back/tsconfig.prod.json_

```json
{
  "extends": "./tsconfig.json",
  "compilerOptions": {
    "rootDir": "./src",
    "outDir": "dist"
  }
}
```

_back terminal_

```bash
npm install rimraf --save-dev

```

_./back/package.json_

```diff
...
  "scripts": {
    ...
    "start:local-db": "docker-compose up -d",
+   "clean": "rimraf dist",
+   "build": "npm run clean && tsc --project tsconfig.prod.json",
    "type-check": "tsc --noEmit --preserveWatchOutput",
    ...
  },
...
```

Let's try it:

_back terminal_

```bash
npm run build

```

A nice improvement is ignore not necessary files for production:

_./back/tsconfig.prod.json_

```diff
{
  "extends": "./tsconfig.json",
  "compilerOptions": {
    "outDir": "dist"
  },
+ "exclude": ["**/*.spec.ts", "./src/console-runners"]
}
```

Let's check it:

_back terminal_

```bash
npm run build

```

We could run this `production` version using node:

_back terminal_

```bash
node --require dotenv/config dist

```

> NOTE: We are using dontenv to load environment variables from `.env` file.

Why is it failing? Because we are using Node.js `alias imports` targeting `src` folder, let's manually create a package.json inside `dist` folder:

_./dist/package.json_

```json
{
  "type": "module",
  "imports": {
    "#*": "./*"
  }
}
```

Run it again:

_back terminal_

```bash
node --require dotenv/config dist

```

Open browser in `http://localhost:3000` and you will see the app running including the frontend part.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
