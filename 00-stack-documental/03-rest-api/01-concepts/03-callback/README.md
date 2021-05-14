# 03 Callback

In this example we are going to use callbacks in Nodejs.

We will start from scratch.

# Steps to build it

Let's create a simple `txt`:

_./file.txt_

```txt
Hello from file!

```

And try to read it using `fs`:

```javascript
const fs = require("fs");

fs.readFile("./file.txt", { encoding: "utf-8" }, (error, data) => {
  if (error) {
    console.error(error);
  } else {
    console.log(data);
  }
});

console.log("Start program");

```

Run app:

```bash
node index

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
