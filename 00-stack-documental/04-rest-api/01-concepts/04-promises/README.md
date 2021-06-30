# 04 Promises

In this example we are going to use promises in Nodejs.

We will start from `03-callback`.

# Steps to build it

Let's update previous example to work with promises. We can try to create our custom promise like:

_./index.js_

```diff
const fs = require("fs");

- fs.readFile("./file.txt", { encoding: "utf-8" }, (error, data) => {
-   if (error) {
-     console.error(error);
-   } else {
-     console.log(data);
-   }
- });

+ const readFile = (fileName) =>
+   new Promise((resolve, reject) => {
+     fs.readFile(fileName, { encoding: "utf-8" }, (error, data) => {
+       if (error) {
+         reject(error);
+       } else {
+         resolve(data);
+       }
+     });
+   });

+ readFile("./file.txt")
+   .then((data) => {
+     console.log(data);
+   })
+   .catch((error) => {
+     console.error(error);
+   });

console.log("Start program");

```

Run app:

```bash
node index

```

We can create the promisified the readFile method using `util` library from Nodejs:

_./index.js_

```diff
const fs = require("fs");
+ const { promisify } = require("util");

- const readFile = (fileName) =>
-   new Promise((resolve, reject) => {
-     fs.readFile(fileName, { encoding: "utf-8" }, (error, data) => {
-       if (error) {
-         reject(error);
-       } else {
-         resolve(data);
-       }
-     });
-   });

+ const readFile = promisify(fs.readFile);

- readFile("./file.txt")
+ readFile("./file.txt", { encoding: "utf-8" })
  .then((data) => {
    console.log(data);
  })
  .catch((error) => {
    console.error(error);
  });

console.log("Start program");

```

With promises, we could `chain` it several ones:

_./index.js_

```diff
...

readFile("./file.txt", { encoding: "utf-8" })
  .then((data) => {
    console.log(data);
+   return readFile("./file.txt", { encoding: "utf-8" });
  })
+ .then((data2) => {
+   console.log("Executing after");
+   console.log(data2);
+ })
  .catch((error) => {
    console.error(error);
  });

console.log("Start program");

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
