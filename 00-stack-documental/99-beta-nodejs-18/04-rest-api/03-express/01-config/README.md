# 01 Config

In this example we are going to add a basic setup needed to support API rest with express.

We will start from scratch.

# Steps to build it

Let's create the `package.json`:

```bash
npm init -y
```

# Libraries

We are going to install the main library which we base our project, [Express](https://expressjs.com/) and here is the [github project](https://github.com/expressjs/express).

```bash
npm install express --save
```

Let's add the main file to start the project:

> NOTE: we are using here CommonJS, the old-fashioned Nodejs import modules.

_./src/index.js_

```javascript
const express = require("express");

const app = express();

app.get("/", (req, res) => {
  res.send("My awesome books portal");
});

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});
```

Instead of execute this index file with node, we could create a npm script command that it will do it for us:

_./package.json_

```diff
{
  "name": "01-config",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "scripts": {
-   "test": "echo \"Error: no test specified\" && exit 1"
+   "start": "node src/index"
  },
  "keywords": [],
  "author": "",
  "license": "ISC",
  "dependencies": {
    "express": "^4.17.1"
  }
}
```

Run app:

```bash
npm start

```

So far so good, this feature is nice for quick demos but not to be used in real world, as a developer we need:

- Automatically restarts the server when we made some changes.
- We would like to use ES6 and beyond features like ES6 imports, optional chaining, etc.
- We would like to use Typescript for avoid typos and detect early errors.

Let's use new NodeJS >= 18 features:

Instead of rename all files to `mjs` we will use the new [type](https://nodejs.org/api/packages.html#packagejson-and-file-extensions) prop in the package.json:

_./package.json_

```diff
{
  ...
+ "type": "module",
  "scripts": {
    "start": "node src/index"
  },

...
```

Using ES module imports:

_./src/index.js_

```diff
- const express = require("express");
+ import express from "express";

const app = express();

app.get("/", (req, res) => {
  res.send("My awesome books portal");
});

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

Previously, we were using some tools like [nodemon](https://github.com/remy/nodemon) to restart the server each time, now we can use the new [watch](https://nodejs.org/dist/latest-v18.x/docs/api/all.html#all_cli_--watch) flag (it's still in Experimental status):

_./package.json_

```diff
...
  "scripts": {
-   "start": "node src/index"
+   "start": "node --watch src/index"
  },
...
```

Using the experimental `watch` flag is not the best option, because it's not ready at all and it could cause some extra re-builds, so we will use `nodemon`:

```bash
npm install nodemon --save-dev

```

Update `package.json`:

_./package.json_

```diff
...
  "scripts": {
-   "start": "node --watch src/index"
+   "start": "nodemon src/index"
  },
...
```

Now, `Nodejs >= 18` provides a lot of ES "new" features (even [import alias](https://nodejs.org/api/packages.html#imports)), that's why, we will not install [babel](https://github.com/babel/babel) this time.

Let's install necessary dev libraries:

- [typescript](https://github.com/microsoft/TypeScript): adds optional types to Javascript that helps you detects early errors.

- [npm-run-all](https://github.com/mysticatea/npm-run-all): is a tool that helps you run multiple npm scripts commands in parallel

- [rimraf](https://github.com/isaacs/rimraf): OS agnostic tool for deleting files and folders.

```bash
npm install typescript npm-run-all rimraf --save-dev

```

Add typescript config file:

_./tsconfig.json_

```json
{
  "compilerOptions": {
    "target": "ES2022",
    "module": "NodeNext",
    "moduleResolution": "NodeNext",
    "declaration": false,
    "noImplicitAny": false,
    "sourceMap": true,
    "noLib": false,
    "allowJs": true,
    "suppressImplicitAnyIndexErrors": true,
    "skipLibCheck": true,
    "esModuleInterop": true
  },
  "include": ["src/**/*"]
}

```

> [ES Modules with TypeScript and NodeJS](https://www.typescriptlang.org/docs/handbook/esm-node.html)
>
> [Compiler Options](https://www.typescriptlang.org/tsconfig)

Let's create npm commands:

_./package.json_

```diff
...
  "scripts": {
-   "start": "nodemon src/index"
+   "prestart": "npm run clean && npm run build:dev",
+   "start": "run-p -l build:watch start:dev",
+   "start:dev": "nodemon dist/index",
+   "clean": "rimraf dist",
+   "build:dev": "tsc --outDir dist",
+   "build:watch": "npm run build:dev -- --watch --preserveWatchOutput"
  },

```

> `clean`: Remove dist folder
>
> `build:dev`: Build the TypeScript code and put the result in the `dist` folder
>
> `prestart`: We will need build the TypeScript code before run `node dist/index`
>
> `build:watch`: Re-build the TypeScript code after some change.
>

Finally, rename the index file to `.ts` extension.

Run app:

```bash
npm start

```

Let's install typings for express:

```bash
npm install @types/express --save-dev

```

> Let's try some changes

We should `ignore` the `node_modules` and `dist` folders to avoid upload it to our git repository:

_./.gitignore_

```
node_modules
dist

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
