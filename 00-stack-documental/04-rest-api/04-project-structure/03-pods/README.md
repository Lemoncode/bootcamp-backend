# 03 PODS

In this example we are going to structure code in `pods` folder.

We will start from `02-dals`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

The `pods` folder is related with each encapsulated piece of functionality that implements your project, in this case, we will create the `book` pod.

Let's start with `API model`:

_./src/pods/book/book.api-model.ts_

```typescript
export interface Book {
  id: string;
  title: string;
  releaseDate: string;
  author: string;
}

```

So, we need to implement `mappers` to transform:

- `Model to API`: when we get entities
- `API to Model`: when we update or insert entities

_./src/pods/book/book.mappers.ts_

```typescript
import * as model from "dals";
import * as apiModel from "./book.api-model";

const mapBookFromModelToApi = (book: model.Book): apiModel.Book => ({
  id: book.id,
  title: book.title,
  releaseDate: book.releaseDate.toISOString(),
  author: book.author,
});

export const mapBookListFromModelToApi = (
  bookList: model.Book[]
): apiModel.Book[] => bookList.map(mapBookFromModelToApi);

```

> [Reference](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Date/toISOString)

Let's implement the `rest-api`, move `./src/books.api.ts` -> `./src/pods/book/book.rest-api.ts`:

_./src/pods/book/book.rest-api.ts_

```diff
import { Router } from "express";
import { bookRepository } from "dals";
+ import { mapBookListFromModelToApi } from "./book.mappers";
- import { getBook, insertBook, updateBook, deleteBook } from "./mock-db";
+ import { getBook, insertBook, updateBook, deleteBook } from "../../mock-db";

export const booksApi = Router();

booksApi
  .get("/", async (req, res, next) => {
    try {
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
      let bookList = await bookRepository.getBookList();

      if (page && pageSize) {
        const startIndex = (page - 1) * pageSize;
        const endIndex = Math.min(startIndex + pageSize, bookList.length);
        bookList = bookList.slice(startIndex, endIndex);
      }
-     res.send(bookList);
+     res.send(mapBookListFromModelToApi(bookList));
    } catch (error) {
      next(error);
    }

...


```

Add barrel file:

_./src/pods/book/index.ts_

```typescript
export * from "./book.rest-api";

```

Update app:

_./src/app.ts_

```diff
import express from "express";
import path from "path";
import { createRestApiServer } from "core/servers";
import { envConstants } from "core/constants";
- import { booksApi } from "./books.api";
+ import { booksApi } from "pods/book";

...

```

Let's export single entity mapper:

_./src/pods/book/book.mappers.ts_

```diff
import * as model from "dals";
import * as apiModel from "./book.api-model";

- const mapBookFromModelToApi = (book: model.Book): apiModel.Book => ({
+ export const mapBookFromModelToApi = (book: model.Book): apiModel.Book => ({
  id: book.id,
  title: book.title,
...

```

Update `get by id` method:

_./src/pods/book/book.rest-api.ts_

```diff
import { Router } from "express";
import { bookRepository } from "dals";
- import { mapBookListFromModelToApi } from "./book.mappers";
+ import { mapBookListFromModelToApi, mapBookFromModelToApi } from "./book.mappers";
- import { getBook, insertBook, updateBook, deleteBook } from "./mock-db";
+ import { insertBook, updateBook, deleteBook } from "../../mock-db";

...
- .get("/:id", async (req, res) => {
+ .get("/:id", async (req, res, next) => {
+   try {
      const { id } = req.params;
-     const bookId = Number(id);
-     const book = await getBook(bookId);
+     const book = await bookRepository.getBook(id);
-     res.send(book);
+     res.send(mapBookFromModelToApi(book));
+   } catch (error) {
+     next(error);
+   }
  })
...
```

Let's implement `API to Model` mapper:

_./src/pods/book/book.mappers.ts_

```diff
...

export const mapBookListFromModelToApi = (
  bookList: model.Book[]
): apiModel.Book[] => bookList.map(mapBookFromModelToApi);

+ export const mapBookFromApiToModel = (book: apiModel.Book): model.Book => ({
+   id: book.id,
+   title: book.title,
+   releaseDate: new Date(book.releaseDate),
+   author: book.author,
+ });

...

```

Update `post` method:

_./src/pods/book/book.rest-api.ts_

```diff
import { Router } from "express";
import { bookRepository } from "dals";
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
+ mapBookFromApiToModel,
} from "./book.mappers";
- import { insertBook, updateBook, deleteBook } from "../../mock-db";
+ import { updateBook, deleteBook } from "../../mock-db";

...
- .post("/", async (req, res, next) => {
+ .post("/", async (req, res) => {
+   try {
-     const book = req.body;
+     const book = mapBookFromApiToModel(req.body);
-     const newBook = await insertBook(book);
+     const newBook = await bookRepository.saveBook(book);
-     res.status(201).send(newBook);
+     res.status(201).send(mapBookFromModelToApi(newBook));
+   } catch (error) {
+     next(error);
+   }
  })
...
```

Running post method:

```
{
    "title": "El señor de los anillos",
    "releaseDate": "1954-07-29T00:00:00.000Z",
    "author": "J. R. R. Tolkien"
}
```

Update `put` method:

_./src/pods/book/book.rest-api.ts_

```diff
import { Router } from "express";
import { bookRepository } from "dals";
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from "./book.mappers";
- import { updateBook, deleteBook } from "../../mock-db";
+ import { deleteBook } from "../../mock-db";

...
- .put("/:id", async (req, res) => {
+ .put("/:id", async (req, res, next) => {
+   try {
      const { id } = req.params;
-     const bookId = Number(id);
-     const book = req.body;
+     const book = mapBookFromApiToModel({ ...req.body, id });
-     await updateBook(bookId, book);
+     await bookRepository.saveBook(book);
      res.sendStatus(204);
+   } catch (error) {
+     next(error);
+   }
  })
...
```

Running put method:

```
URL: http://localhost:3000/api/books/1

BODY:
{
    "title": "Choque de reyes Actualizado",
    "releaseDate": "2022-07-21T00:00:00.000Z",
    "author": "Otro autor"
}
```

Update `delete` method:

_./src/pods/book/book.rest-api.ts_

```diff
import { Router } from "express";
import { bookRepository } from "dals";
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from "./book.mappers";
- import { deleteBook } from "../../mock-db";

...
- .delete("/:id", async (req, res) => {
+ .delete("/:id", async (req, res, next) => {
+   try {
      const { id } = req.params;
-     const bookId = Number(id);
-     await deleteBook(bookId);
+     await bookRepository.deleteBook(id);
      res.sendStatus(204);
+   } catch (error) {
+     next(error);
+   }
  });

```

Now, we could delete `./src/mock-db.ts` file.

A nice improvement is move the pagination functionality to a mock data `helpers` in `book.mock-repository` file:

_./src/dals/book/repositories/book.mock-repository.ts_

```diff
...
const updateBook = (book: Book) => {
  db.books = db.books.map((b) => (b.id === book.id ? { ...b, ...book } : b));
  return book;
};

+ const paginateBookList = (
+   bookList: Book[],
+   page: number,
+   pageSize: number
+ ): Book[] => {
+   let paginatedBookList = [...bookList];
+   if (page && pageSize) {
+     const startIndex = (page - 1) * pageSize;
+     const endIndex = Math.min(startIndex + pageSize, paginatedBookList.length);
+     paginatedBookList = paginatedBookList.slice(startIndex, endIndex);
  }

+   return paginatedBookList;
+ };

export const mockRepository: BookRepository = {
- getBookList: async () => db.books,
+ getBookList: async (page?: number, pageSize?: number) =>
+   paginateBookList(db.books, page, pageSize),
  getBook: async (id: string) => db.books.find((b) => b.id === id),
  saveBook: async (book: Book) =>
    Boolean(book.id) ? updateBook(book) : insertBook(book),
  deleteBook: async (id: string) => {
    db.books = db.books.filter((b) => b.id !== id);
    return true;
  },
};

```

Update contract:

_./src/dals/book/repositories/book.repository.ts_

```diff
import { Book } from '../book.model';

export interface BookRepository {
- getBookList: () => Promise<Book[]>;
+ getBookList: (page?: number, pageSize?: number) => Promise<Book[]>;
  getBook: (id: string) => Promise<Book>;
  saveBook: (book: Book) => Promise<Book>;
  deleteBook: (id: string) => Promise<boolean>;
}

```

Update db repository:

_./src/dals/book/repositories/book.db-repository.ts_

```diff
import { BookRepository } from './book.repository';
import { Book } from '../book.model';

export const dbRepository: BookRepository = {
- getBookList: async () => {
+ getBookList: async (page?: number, pageSize?: number) => {
    throw new Error('Not implemented');
  },
  getBook: async (id: string) => {
    throw new Error('Not implemented');
  },
  saveBook: async (book: Book) => {
    throw new Error('Not implemented');
  },
  deleteBook: async (id: string) => {
    throw new Error('Not implemented');
  },
};

```

Update api:

_./src/pods/book/book.rest-api.ts_

```diff
...
booksApi
  .get('/', async (req, res, next) => {
    try {
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
-     let bookList = await bookRepository.getBookList();
+     const bookList = await bookRepository.getBookList(page, pageSize);

-     if (page && pageSize) {
-       const startIndex = (page - 1) * pageSize;
-       const endIndex = Math.min(startIndex + pageSize, bookList.length);
-       bookList = bookList.slice(startIndex, endIndex);
-     }
      res.send(mapBookListFromModelToApi(bookList));
    } catch (error) {
      next(error);
    }
  })
...
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
