# 02 Simple Routing

In this example we are going to add a basic setup needed to support simple Routing with express.

We will start from `01-config`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install

```

Add `mock-db` files:

_./src/mock-db.ts_

```typescript
let mockBookList = [
  {
    id: 1,
    title: "Choque de reyes",
    releaseDate: "16/11/1998",
    author: "George R. R. Martin",
  },
  {
    id: 2,
    title: "Harry Potter y el prisionero de Azkaban",
    releaseDate: "21/07/1999",
    author: "J. K. Rowling",
  },
  {
    id: 3,
    title: "The Witcher - The Last Wish",
    releaseDate: "02/11/1993",
    author: "Andrzej Sapkowski",
  },
  {
    id: 4,
    title: "El Hobbit",
    releaseDate: "21/09/1937",
    author: "J. R. R. Tolkien",
  },
  {
    id: 5,
    title: "Assassin's Quest",
    releaseDate: "03/03/1997",
    author: "Robin Hobb",
  },
  {
    id: 6,
    title: "Homeland",
    releaseDate: "19/09/1990",
    author: "R. A. Salvatore",
  },
  {
    id: 7,
    title: "American Gods",
    releaseDate: "19/06/2001",
    author: "Neil Gaiman",
  },
];

export const getBookList = async () => {
  return mockBookList;
};

export const getBook = async (id) => {
  return mockBookList.find((book) => book.id === id);
};

export const insertBook = async (book) => {
  const id = mockBookList.length + 1;
  const newBook = {
    ...book,
    id,
  };

  mockBookList = [...mockBookList, newBook];
  return newBook;
};

export const updateBook = async (id, updatedBook) => {
  mockBookList = mockBookList.map((book) =>
    book.id === id
      ? {
          ...book,
          ...updatedBook,
          id,
        }
      : book
  );
};

export const deleteBook = async (id) => {
  mockBookList = mockBookList.filter((book) => book.id !== id);
  return true;
};
```

Let's try to implement the books portal methods that we saw in previous section. We will start with "Get available book list":

_./src/index.ts_

```diff
import express from "express";
+ import { getBookList } from "./mock-db.js";

const app = express();

app.get("/", (req, res) => {
  res.send("My awesome books portal");
});

+ app.get("/api/books", async (req, res) => {
+   const bookList = await getBookList();
+   res.send(bookList);
+ });

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

> NOTE: [Mandatory file extensions](https://nodejs.org/api/esm.html#mandatory-file-extensions). We have to use `.js` even for TypeScript
>
> Note: express are setting content-type header for us.

How we could define the route for "Get book detail"?

_./src/index.ts_

```diff
import express from "express";
- import { getBookList } from "./mock-db.js";
+ import { getBookList, getBook } from "./mock-db.js";

const app = express();

app.get("/", (req, res) => {
  res.send("My awesome books portal");
});

app.get("/api/books", async (req, res) => {
  const bookList = await getBookList();
  res.send(bookList);
});

+ app.get("/api/books/:id", async (req, res) => {
+   const { id } = req.params;
+   const bookId = Number(id);
+   const book = await getBook(bookId);
+   res.send(book);
+ });

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
