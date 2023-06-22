# 05 CONSOLE RUNNERS

In this example we are going to structure code in `console-runners` folder.

We will start from `04-common`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

The `console-runners` folder is related with nodejs processes that we want to execute outside the app. A nice library for interactive CLIs is [inquier](https://github.com/SBoudrias/Inquirer.js/)

```bash
npm install inquirer @types/inquirer --save-dev
```

Let's create the `index` file:

_./src/console-runners/index.ts_

```typescript
import inquier from "inquirer";

let exit = false;
while (!exit) {
  const answer = await inquier.prompt({
    name: "consoleRunner",
    type: "list",
    message: "Which console-runner do you want to run?",
    choices: ["create-admin", "exit"],
  });

  if (answer.consoleRunner !== "exit") {
    console.log("Create admin runner");
  } else {
    exit = true;
  }
}

```

> [Prompt types](https://github.com/SBoudrias/Inquirer.js/#prompt-types)

Let's create a npm command to execute it:

_./package.json_

```diff
...
  "scripts": {
    "start": "run-p -l type-check:watch start:dev",
    "start:dev": "nodemon --transpileOnly --esm src/index.ts",
+   "start:console-runners": "nodemon --no-stdin --transpileOnly --esm src/console-runners/index.ts",
    "type-check": "tsc --noEmit --preserveWatchOutput",
    "type-check:watch": "npm run type-check -- --watch"
  },
...
```

Running it:

```bash
npm run start:console-runners
```

> If your project does not supports native ES Modules for NodeJS you have to  install the 8.x version.

We can create a file for each `runner`:

_./src/console-runners/create-admin.runner.ts_

```typescript
import inquirer, { QuestionCollection } from "inquirer";

const passwordQuestions: QuestionCollection = [
  {
    name: "password",
    type: "password",
    message: "Password:",
    mask: true,
  },
  {
    name: "confirmPassword",
    type: "password",
    message: "Confirm password:",
    mask: true,
  },
];

export const run = async () => {
  // TODO: Connect to DB
  const { user } = await inquirer.prompt({
    name: "user",
    type: "input",
    message: "User name:",
  });

  let passwordAnswers = await inquirer.prompt(passwordQuestions);
  while (passwordAnswers.password !== passwordAnswers.confirmPassword) {
    console.error("Password does not match, fill it again");
    passwordAnswers = await inquirer.prompt(passwordQuestions);
  }

  // TODO: Insert into DB and disconnect it
  console.log(`User ${user} created!`);
};

```

Update main file:

_./src/console-runners/index.ts_

```diff
import inquier from "inquirer";

let exit = false;
while (!exit) {
  const answer = await inquier.prompt({
    name: "consoleRunner",
    type: "list",
    message: "Which console-runner do you want to run?",
    choices: ["create-admin", "exit"],
  });

  if (answer.consoleRunner !== "exit") {
-   console.log("Create admin runner");
+   const { run } = await import(`./${answer.consoleRunner}.runner.js`);
+   await run();
  } else {
    exit = true;
  }
}

```

> [Dynamic imports](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/import)

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
