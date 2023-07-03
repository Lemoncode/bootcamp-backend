# 02 DALS

In this example we are going to structure code in `dals` folder.

We will start from `01-core`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

The `dals` folder is related with the entities from your domain, it means it's a good place getting `models`, `repositories`, `mock-data`, etc. We will migrate `mock-db.ts` to this folder:

_./dals/book/book.model.ts_

```typescript
export interface Book {
  id: string;
  title: string;
  releaseDate: Date;
  author: string;
}

```

Add barrel file:

_./dals/book/index.ts_

```typescript
export * from "./book.model.js";

```

Let's create the `mock-data` file that it will be represent the database in memory:

_./dals/mock-data.ts_

```typescript
import { Book } from "./book/index.js";

export interface DB {
  books: Book[];
}

export const db: DB = {
  books: [
    {
      id: "1",
      title: "Choque de reyes",
      releaseDate: new Date("1998-11-16"),
      author: "George R. R. Martin",
    },
    {
      id: "2",
      title: "Harry Potter y el prisionero de Azkaban",
      releaseDate: new Date("1999-07-21"),
      author: "J. K. Rowling",
    },
    {
      id: "3",
      title: "The Witcher - The Last Wish",
      releaseDate: new Date("1993-11-02"),
      author: "Andrzej Sapkowski",
    },
    {
      id: "4",
      title: "El Hobbit",
      releaseDate: new Date("1937-09-21"),
      author: "J. R. R. Tolkien",
    },
    {
      id: "5",
      title: "Assassin's Quest",
      releaseDate: new Date("1997-03-03"),
      author: "Robin Hobb",
    },
    {
      id: "6",
      title: "Homeland",
      releaseDate: new Date("1990-09-19"),
      author: "R. A. Salvatore",
    },
    {
      id: "7",
      title: "American Gods",
      releaseDate: new Date("2001-06-19"),
      author: "Neil Gaiman",
    },
  ],
};

```

Let's create `repositories`, they are interfaces to getting entities as well as creating and changing them, etc.

_./dals/book/repositories/book.repository.ts_

```typescript
import { Book } from "../book.model.js";

export interface BookRepository {
  getBookList: () => Promise<Book[]>;
  getBook: (id: string) => Promise<Book>;
  saveBook: (book: Book) => Promise<Book>;
  deleteBook: (id: string) => Promise<boolean>;
}

```

Let's implement `mock-repository`:

_./dals/book/repositories/book.mock-repository.ts_

```typescript
import { BookRepository } from "./book.repository.js";
import { Book } from "../book.model.js";
import { db } from "../../mock-data.js";

const insertBook = (book: Book) => {
  const id = (db.books.length + 1).toString();
  const newBook: Book = {
    ...book,
    id,
  };

  db.books = [...db.books, newBook];
  return newBook;
};

const updateBook = (book: Book) => {
  db.books = db.books.map((b) => (b.id === book.id ? { ...b, ...book } : b));
  return book;
};

export const mockRepository: BookRepository = {
  getBookList: async () => db.books,
  getBook: async (id: string) => db.books.find((b) => b.id === id),
  saveBook: async (book: Book) =>
    Boolean(book.id) ? updateBook(book) : insertBook(book),
  deleteBook: async (id: string) => {
    db.books = db.books.filter((b) => b.id !== id);
    return true;
  },
};

```

We could create the empty `db repository` pending to implement:

_./dals/book/repositories/book.db-repository.ts_

```typescript
import { BookRepository } from "./book.repository.js";
import { Book } from "../book.model.js";

export const dbRepository: BookRepository = {
  getBookList: async () => {
    throw new Error("Not implemented");
  },
  getBook: async (id: string) => {
    throw new Error("Not implemented");
  },
  saveBook: async (book: Book) => {
    throw new Error("Not implemented");
  },
  deleteBook: async (id: string) => {
    throw new Error("Not implemented");
  },
};

```

Add barrel file:

_./dals/book/repositories/index.ts_

```typescript
import { mockRepository } from "./book.mock-repository.js";
import { dbRepository } from "./book.db-repository.js";

// TODO: Create env variable
const isApiMock = true;

export const bookRepository = isApiMock ? mockRepository : dbRepository;

```

Update barrel file:

_./dals/book/index.ts_

```diff
export * from "./book.model.js";
+ export * from "./repositories/index.js";

```

Finally, let's create the env variable:

_./.env.example_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
+ API_MOCK=true

```

_./.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
+ API_MOCK=true

```

Update constants:

_./src/core/constants/env.constants.ts_

```diff
export const envConstants = {
  isProduction: process.env.NODE_ENV === 'production',
  PORT: process.env.PORT,
  STATIC_FILES_PATH: process.env.STATIC_FILES_PATH, 
  CORS_ORIGIN: process.env.CORS_ORIGIN,
  CORS_METHODS: process.env.CORS_METHODS,
+ isApiMock: process.env.API_MOCK === 'true',
};

```

Update barrel file:

_./dals/book/repositories/index.ts_

```diff
import { mockRepository } from "./book.mock-repository.js";
import { dbRepository } from "./book.db-repository.js";
+ import { envConstants } from "../../../core/constants/index.js";

- // TODO: Create env variable
- const isApiMock = true;

- export const bookRepository = isApiMock ? mockRepository : dbRepository;
+ export const bookRepository = envConstants.isApiMock ? mockRepository : dbRepository;

```

Let's try using the repository:

_./src/dals/index.ts_

```typescript
export * from "./book/index.js";

```

_./src/books.api.ts_

```diff
import { Router } from "express";
+ import { bookRepository } from "./dals/index.js";
import {
- getBookList,
  getBook,
  insertBook,
  updateBook,
  deleteBook,
} from "./mock-db.js";

export const booksApi = Router();

booksApi
  .get("/", async (req, res, next) => {
    try {
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
-     let bookList = await getBookList();
+     let bookList = await bookRepository.getBookList();

...
```

> NOTE: Try to change env variable API_MOCK=false

As a great improvement, we will configure our project to allow import aliases, NodeJS has it owns system the [subpath.imports](https://nodejs.org/api/packages.html#subpath-imports):

> In early NodeJS versions, we were using [babel-plugin-module-resolver](https://github.com/tleunen/babel-plugin-module-resolver) for babel.

Let's add main aliases:

_./pacakge.json_

```diff
{
  ...
  "scripts": {
  },
+ "imports": {
+   "#common/*": "./src/common/*",
+   "#common-app/*": "./src/common-app/*",
+   "#core/*": "./src/core/*",
+   "#dals/*": "./src/dals/*",
+   "#pods/*": "./src/pods/*"
+ },
  ...
}

```

> NOTE: Entries in the "imports" field must always start with # to ensure they are disambiguated from external package specifiers.

Configure typescript:

_./tsconfig.json_

```diff{
  "compilerOptions": {
    "target": "ESNext",
    "module": "ESNext",
    "moduleResolution": "NodeNext",
    "skipLibCheck": true,
    "isolatedModules": true,
    "esModuleInterop": true,
+   "baseUrl": "./src",
+   "paths": {
+     "#*": ["*"]
+   }
  },
  "include": ["src/**/*"]
}

```

Update imports with aliases:

_./src/index.ts_

```diff
- import "./core/load-env.js";
+ import "#core/load-env.js";
import express from "express";
import path from "path";
import url from "url";
- import { createRestApiServer } from "./core/servers/index.js";
+ import { createRestApiServer } from "#core/servers/index.js";
- import { envConstants } from "./core/constants/index.js";
+ import { envConstants } from "#core/constants/index.js";
import { booksApi } from "./books.api.js";
...

```

_./src/dals/book/repositories/index.ts_

```diff
import { mockRepository } from "./book.mock-repository.js";
import { dbRepository } from "./book.db-repository.js";
- import { envConstants } from "../../../core/constants/index.js";
+ import { envConstants } from "#core/constants/index.js";

...

```

_./src/books.api.ts_

```diff
import { Router } from "express";
- import { bookRepository } from "./dals/index.js";
+ import { bookRepository } from "#dals/index.js";
import { getBook, insertBook, updateBook, deleteBook } from "./mock-db.js";

...

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
