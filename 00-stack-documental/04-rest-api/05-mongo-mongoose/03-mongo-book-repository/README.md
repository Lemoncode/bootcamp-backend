# 03 Mongo book repository

In this example we are going to implement book repository in MongoDB.

We will start from `02-config`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

Clean `app` file:

_./src/app.ts_

```diff
import express from "express";
import path from "path";
- import { createRestApiServer, connectToDBServer, getDBInstance } from "core/servers";
+ import { createRestApiServer, connectToDBServer } from "core/servers";
import { envConstants } from "core/constants";
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from "common/middlewares";
import { booksApi } from "pods/book";
...

restApiServer.listen(envConstants.PORT, async () => {
  if (!envConstants.isApiMock) {
    await connectToDBServer(envConstants.MONGODB_URI);
    console.log("Connected to DB");
-   const db = getDBInstance();
-   const books = await db.collection("books").find().toArray();
-   console.log({ books });
  } else {
    console.log("Running API mock");
  }
  console.log(`Server ready at port ${envConstants.PORT}`);
});
```

Notice an important refactor is the `id` field, MongoDB automatically create the field `_id` when a document was inserted:

_./src/dals/book/book.model.ts_

```diff
+ import { ObjectId } from "mongodb";

export interface Book {
- id: string;
+ _id: ObjectId;
  title: string;
  releaseDate: Date;
  author: string;
}

```

Let's refactor `mock-repository`:

_./src/dals/book/repositories/book.mock-repository.ts_

```diff
+ import { ObjectId } from "mongodb";
import { BookRepository } from "./book.repository";
import { Book } from "../book.model";
import { db } from "../../mock-data";

const insertBook = (book: Book) => {
- const id = (db.books.length + 1).toString();
+ const _id = new ObjectId();
  const newBook = {
    ...book,
-   id,
+   _id,
  };

  db.books = [...db.books, newBook];
  return newBook;
};

const updateBook = (book: Book) => {
- db.books = db.books.map((b) => (b.id === book.id ? { ...b, ...book } : b));
+ db.books = db.books.map((b) => (b._id.toHexString() === book._id.toHexString() ? { ...b, ...book } : b));
  return book;
};

export const mockRepository: BookRepository = {
  getBookList: async () => db.books,
- getBook: async (id: string) => db.books.find((b) => b.id === id),
+ getBook: async (id: string) => db.books.find((b) => b._id.toHexString() === id),
  saveBook: async (book: Book) =>
-   Boolean(book.id) ? updateBook(book) : insertBook(book),
+   Boolean(book._id) ? updateBook(book) : insertBook(book),
  deleteBook: async (id: string) => {
-   db.books = db.books.filter((b) => b.id !== id);
+   db.books = db.books.filter((b) => b._id.toHexString() !== id);
    return true;
  },
};

```

Update `mock-data`:

_./src/dals/mock-data.ts_

```diff
+ import { ObjectId } from "mongodb";
import { Book } from "./book";

export interface DB {
  books: Book[];
}

export const db: DB = {
  books: [
    {
-     id: "1",
+     _id: new ObjectId(),
      title: "Choque de reyes",
      releaseDate: new Date("1998-11-16"),
      author: "George R. R. Martin",
    },
    {
      -d: "2",
+     _id: new ObjectId(),
      title: "Harry Potter y el prisionero de Azkaban",
      releaseDate: new Date("1999-07-21"),
      author: "J. K. Rowling",
    },
    {
-     id: "3",
+     _id: new ObjectId(),
      title: "The Witcher - The Last Wish",
      releaseDate: new Date("1993-11-02"),
      author: "Andrzej Sapkowski",
    },
    {
-     id: "4",
+     _id: new ObjectId(),
      title: "El Hobbit",
      releaseDate: new Date("1937-09-21"),
      author: "J. R. R. Tolkien",
    },
    {
-     id: "5",
+     _id: new ObjectId(),
      title: "Assassin's Quest",
      releaseDate: new Date("1997-03-03"),
      author: "Robin Hobb",
    },
    {
-     id: "6",
+     _id: new ObjectId(),
      title: "Homeland",
      releaseDate: new Date("1990-09-19"),
      author: "R. A. Salvatore",
    },
    {
-     id: "7",
+     _id: new ObjectId(),
      title: "American Gods",
      releaseDate: new Date("2001-06-19"),
      author: "Neil Gaiman",
    },
  ],
};

```

Update `mappers`:

_./src/pods/book/book.mappers.ts_

```diff
+ import { ObjectId } from "mongodb";
import * as model from "dals";
import * as apiModel from "./book.api-model";

export const mapBookFromModelToApi = (book: model.Book): apiModel.Book => ({
- id: book.id,
+ id: book._id.toHexString(),
  title: book.title,
  releaseDate: book.releaseDate.toISOString(),
  author: book.author,
});

export const mapBookListFromModelToApi = (
  bookList: model.Book[]
): apiModel.Book[] => bookList.map(mapBookFromModelToApi);

export const mapBookFromApiToModel = (book: apiModel.Book): model.Book => ({
- id: book.id,
+ _id: new ObjectId(book.id),
  title: book.title,
  releaseDate: new Date(book.releaseDate),
  author: book.author,
});

```

Update env variable `API_MOCK=true` and running:

_./.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
- API_MOCK=false
+ API_MOCK=true
 MONGODB_URI=mongodb://localhost:27017/book-store

```

Check mock queries `Get book list` and `Update book`.

Update env variable `API_MOCK=false` and running:

_./.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
- API_MOCK=true
+ API_MOCK=false
 MONGODB_URI=mongodb://localhost:27017/book-store

```

Implement `get book list`:

_./src/dals/book/repositories/book.db-repository.ts_

```diff
+ import { getDBInstance } from "core/servers";
import { BookRepository } from "./book.repository";
import { Book } from "../book.model";

export const dbRepository: BookRepository = {
  getBookList: async () => {
-   throw new Error("Not implemented");
+   const db = getDBInstance();
+   return await db.collection<Book>("books").find().toArray();
  },
...

```

Implement `insert new book`:

_./src/dals/book/repositories/book.db-repository.ts_

```diff
...
  saveBook: async (book: Book) => {
-   throw new Error("Not implemented");
+   const db = getDBInstance();
+   const { insertedId } = await db.collection<Book>("books").insertOne(book);
+   return {
+     ...book,
+     _id: insertedId,
+   };
  },
...
```

Implement `update book`:

_./src/dals/book/repositories/book.db-repository.ts_

```diff
...
  saveBook: async (book: Book) => {
    const db = getDBInstance();
-   const { insertedId } = await db.collection<Book>("books").insertOne(book);
+   const { value } = await db.collection<Book>("books").findOneAndUpdate(
+     {
+       _id: book._id,
+     },
+     { $set: book },
+     { upsert: true, returnDocument: "after" }
+   );
-   return {
-     ...book,
-     _id: insertedId,
-   };
+   return value;
  },
...

```

> NOTE: Show `updateOne` method.
> [Mongo Console docs](https://docs.mongodb.com/manual/reference/method/db.collection.findOneAndUpdate/) differs from [Mongo Driver docs](https://mongodb.github.io/node-mongodb-native/3.6/api/Collection.html#findOneAndUpdate)

Implement `get book`:

_./src/dals/book/repositories/book.db-repository.ts_

```diff
+ import { ObjectId } from "mongodb";
import { getDBInstance } from "core/servers";
import { BookRepository } from "./book.repository";
import { Book } from "../book.model";

...
  getBook: async (id: string) => {
-   throw new Error("Not implemented");
+   const db = getDBInstance();
+   return await db.collection<Book>("books").findOne({
+     _id: new ObjectId(id),
+   });
  },
...

```

Implement `delete book`:

_./src/dals/book/repositories/book.db-repository.ts_

```diff
...
  deleteBook: async (id: string) => {
-   throw new Error("Not implemented");
+   const db = getDBInstance();
+   const { deletedCount } = await db.collection<Book>('books').deleteOne({
+     _id: new ObjectId(id),
+   });
+   return deletedCount === 1;
  },

```

Update `rest-api` to update response error:

_./src/pods/book/book.rest-api.ts_

```diff
...
  .delete("/:id", async (req, res, next) => {
    try {
      const { id } = req.params;
-     await bookRepository.deleteBook(id);
+     const isDeleted = await bookRepository.deleteBook(id);
-     res.sendStatus(204);
+     res.sendStatus(isDeleted ? 204 : 404);
    } catch (error) {
      next(error);
    }
  });
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
