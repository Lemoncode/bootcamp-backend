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
export * from "./book.model";

```

Let's create the `mock-data` file that it will be represent the database in memory:

_./dals/mock-data.ts_

```typescript
import { Book } from "./book";

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
import { Book } from "../book.model";

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
import { BookRepository } from "./book.repository";
import { Book } from "../book.model";
import { db } from "../../mock-data";

const insertBook = (book: Book) => {
  const id = (db.books.length + 1).toString();
  const newBook = {
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
import { BookRepository } from "./book.repository";
import { Book } from "../book.model";

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
import { mockRepository } from "./book.mock-repository";
import { dbRepository } from "./book.db-repository";

// TODO: Create env variable
const isApiMock = true;

export const bookRepository = isApiMock ? mockRepository : dbRepository;

```

Update barrel file:

_./dals/book/index.ts_

```diff
export * from "./book.model";
+ export * from "./repositories";

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
import { mockRepository } from "./book.mock-repository";
import { dbRepository } from "./book.db-repository";
+ import { envConstants } from "../../../core/constants";

- // TODO: Create env variable
- const isApiMock = true;

- export const bookRepository = isApiMock ? mockRepository : dbRepository;
+ export const bookRepository = envConstants.isApiMock ? mockRepository : dbRepository;

```

Let's try using the repository:

_./src/dals/index.ts_

```typescript
export * from "./book";

```

_./src/books.api.ts_

```diff
import { Router } from "express";
+ import { bookRepository } from "./dals";
import {
- getBookList,
  getBook,
  insertBook,
  updateBook,
  deleteBook,
} from "./mock-db";

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

As a great improvement, we will configure our project to allow import aliases installing [babel-plugin-module-resolver](https://github.com/tleunen/babel-plugin-module-resolver):

```bash
npm install babel-plugin-module-resolver --save-dev
```

> NOTE: It's not an official plugin.

Let's add main folders aliases:

_./.babelrc_

```diff
{
  "presets": [
    [
      "@babel/preset-env",
      {
        "targets": {
          "node": "14"
        }
      }
    ],
    "@babel/preset-typescript"
  ],
  "plugins": [
    "@babel/plugin-proposal-optional-chaining",
+   [
+     "module-resolver",
+     {
+       "root": ["."],
+       "alias": {
+         "common": "./src/common",
+         "common-app": "./src/common-app",
+         "core": "./src/core",
+         "dals": "./src/dals",
+         "pods": "./src/pods"
+       }
+     }
+   ]
  ],
  "env": {
    "development": {
      "sourceMaps": "inline"
    }
  }
}

```

Configure typescript:

_./tsconfig.json_

```diff{
  "compilerOptions": {
    "target": "es6",
    "module": "es6",
    "moduleResolution": "node",
    "declaration": false,
    "noImplicitAny": false,
    "sourceMap": true,
    "noLib": false,
    "allowJs": true,
    "suppressImplicitAnyIndexErrors": true,
    "skipLibCheck": true,
    "esModuleInterop": true,
+   "baseUrl": "./src"
  },
  "include": ["src/**/*"]
}

```

Update imports with aliases:

_./src/app.ts_

```diff
import express from "express";
import path from "path";
- import { createRestApiServer } from "./core/servers";
+ import { createRestApiServer } from "core/servers";
- import { envConstants } from "./core/constants";
+ import { envConstants } from "core/constants";
import { booksApi } from "./books.api";
...

```

_./src/dals/book/repositories/index.ts_

```diff
import { mockRepository } from "./book.mock-repository";
import { dbRepository } from "./book.db-repository";
- import { envConstants } from "../../../core/constants";
+ import { envConstants } from "core/constants";

...

```

_./src/books.api.ts_

```diff
import { Router } from "express";
- import { bookRepository } from "./dals";
+ import { bookRepository } from "dals";
import { getBook, insertBook, updateBook, deleteBook } from "./mock-db";

...

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
