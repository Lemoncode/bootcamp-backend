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

Let's install necessary dev libraries for that:

- [babel](https://github.com/babel/babel): is a tool that helps you write code in the latest versions of Javascript. We need install some plugins and presets even the typescript one.

- [typescript](https://github.com/microsoft/TypeScript): adds optional types to Javascript that helps you detects early errors.

- [nodemon](https://github.com/remy/nodemon): is a tool that helps automatically restarting the node application when file changes

- [npm-run-all](https://github.com/mysticatea/npm-run-all): is a tool that helps you run multiple npm scripts commands in parallel

```bash
npm install @babel/cli @babel/core @babel/node @babel/preset-env @babel/preset-typescript typescript nodemon npm-run-all --save-dev

```

Add babel config file:

_./.babelrc_

```json
{
  "presets": [
    [
      "@babel/preset-env",
      {
        "targets": {
          "node": "16"
        }
      }
    ],
    "@babel/preset-typescript"
  ]
}
```

Add typescript config file:

_./tsconfig.json_

```json
{
  "compilerOptions": {
    "target": "es6",
    "module": "es6",
    "moduleResolution": "node",
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

Let's create npm commands:

_./package.json_

```diff
...
  "scripts": {
-   "start": "node src/index"
+   "start": "run-p -l type-check:watch start:dev",
+   "start:dev": "nodemon --exec babel-node --extensions \".ts\" src/index.ts",
+   "type-check": "tsc --noEmit",
+   "type-check:watch": "npm run type-check -- --watch"
  },
```

Finally, rename the index file to `.ts` extension.

Run app:

```bash
npm start

```

We can try ES6 modules:

_./src/index.ts_

```diff
- const express = require("express");
+ import express from "express";

const app = express();

...
```

Let's install typings for express:

```bash
npm install @types/express --save-dev

```

> Let's try some changes

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
