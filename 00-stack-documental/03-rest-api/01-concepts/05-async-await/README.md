# 05 Async / Await

In this example we are going to use async/await in Nodejs.

We will start from `04-promises`.

# Steps to build it

Let's update previous example to work with async/await:

_./index.js_

```diff
const fs = require("fs");
const { promisify } = require("util");

const readFile = promisify(fs.readFile);

- readFile("./file.txt", { encoding: "utf-8" })
-   .then((data) => {
-     console.log(data);
-     return readFile("./file.txt", { encoding: "utf-8" });
-   })
-   .then((data2) => {
-     console.log("Executing after");
-     console.log(data2);
-   })
-   .catch((error) => {
-     console.error(error);
-   });

+ const run = async () => {
+   try {
+     const data = await readFile("./file.txt", { encoding: "utf-8" });
+     console.log(data);
+     console.log("Executing after");
+     const data2 = await readFile("./file.txt", { encoding: "utf-8" });
+     console.log(data2);
+   } catch (error) {
+     console.error(error);
+   }
+ };

+ run();
console.log("Start program");

```

> [Async/Await nodejs support](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/async_function)

Run app:

```bash
node index

```


# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).


# Async / Await support


