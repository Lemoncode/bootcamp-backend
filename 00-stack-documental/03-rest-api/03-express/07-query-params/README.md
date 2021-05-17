# 07 Query params

In this example we are going to implement routes with query params.

We will start from `06-third-party-middlewares`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

A common way to provide params for filter or pagination in backend side, it's using query params. 

Let's implement backend pagination in book list, we will define the next params:

  - page: the current page
  - paseSize: how many books each page will have.

_./src/books.api.ts_

```diff
...

booksApi
  .get("/", async (req, res, next) => {
    try {
+     const page = Number(req.query.page);
+     const pageSize = Number(req.query.pageSize);
-     const bookList = await getBookList();
+     let bookList = await getBookList();
-     throw Error("Simulating error");

+     if (page && pageSize) {
+       const startIndex = (page - 1) * pageSize;
+       const endIndex = Math.min(startIndex + pageSize, bookList.length);
+       bookList = bookList.slice(startIndex, endIndex);
+     }
      res.send(bookList);
    } catch (error) {
      next(error);
    }
  })
...
```

Let's try some urls:

```
http://localhost:3000/api/books?page=1&pageSize=5
http://localhost:3000/api/books?page=2&pageSize=5
http://localhost:3000/api/books?page=3&pageSize=5

http://localhost:3000/api/books?page=1&pageSize=3
http://localhost:3000/api/books?page=2&pageSize=3
http://localhost:3000/api/books?page=3&pageSize=3

```

> In a real project, we will need send the book's total length too.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
