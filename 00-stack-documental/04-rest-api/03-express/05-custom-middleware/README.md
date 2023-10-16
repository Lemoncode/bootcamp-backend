# 05 Custom middleware

In this example we are going to create a custom middleware.

We will start from `04-built-in-middlewares`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

Having a logger is very typical in a real project, for example to warn of application errors. In this example, we will create a two custom middlewares:

The first one, it's to log some info like req url:

_./src/index.ts_

```diff
...

+ app.use(async (req, res, next) => {
+   console.log(req.url);
+   next();
+ });

app.use("/api/books", booksApi);

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

The second one, it's to catch errors and log it by console:

_./src/index.ts_

```diff
...

app.use(async (req, res, next) => {
  console.log(req.url);
  next();
});

+ app.use(async (error, req, res, next) => {
+   console.error(error);
+   res.sendStatus(500)
+ });

app.use("/api/books", booksApi);

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

Let's simulate an error in `book api`:

_./src/books.api.ts_

```diff
...
booksApi
- .get("/", async (req, res) => {
+ .get("/", async (req, res, next) => {
+   try {
      const bookList = await getBookList();
+     throw Error("Simulating error");
      res.send(bookList);
+   } catch (error) {
+     next(error);
+   }
  })
```

Run app:

```bash
npm start

```

> NOTE: It looks like it's working but it DOES NOT. We are receiving an HTML with the error instead of a 500 without a body response

Why the app is not working? It's because it's important the middleware order when we use it, that is:

_./src/index.ts_

```diff
...

app.use(async (req, res, next) => {
  console.log(req.url);
  next();
});

- app.use(async (error, req, res, next) => {
-   console.error(error);
-   res.sendStatus(500)
- });

app.use("/api/books", booksApi);

+ app.use(async (error, req, res, next) => {
+   console.error(error);
+   res.sendStatus(500)
+ });

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
