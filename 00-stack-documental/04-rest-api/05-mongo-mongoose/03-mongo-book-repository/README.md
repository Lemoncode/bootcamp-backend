# 03 Mongo book repository

In this example we are going to implement book repository in MongoDB.

We will start from `02-official-driver`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

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
import { BookRepository } from "./book.repository.js";
import { Book } from "../book.model.js";
import { db } from "../../mock-data.js";

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
  db.books = db.books.map((b) =>
-   b.id === book.id ? { ...b, ...book } : b
+   b._id.toHexString() === book._id.toHexString() ? { ...b, ...book } : b
  );
  return book;
};

...

export const mockRepository: BookRepository = {
  getBookList: async (page?: number, pageSize?: number) =>
    paginateBookList(db.books, page, pageSize),
  getBook: async (id: string) =>
-   db.books.find((b) => b.id === id),
+   db.books.find((b) => b._id.toHexString() === id),
  saveBook: async (book: Book) =>
-   db.books.some((b) => b.id === book.id)
+   db.books.some((b) => b.id === book.id)
      ? updateBook(book)
      : insertBook(book),
  deleteBook: async (id: string) => {
-   const exists = db.books.some((b) => b.id === id);
+   const exists = db.books.some((b) => b._id.toHexString() === id);
-   db.books = db.books.filter((b) => b.id !== id);
+   db.books = db.books.filter((b) => b._id.toHexString() !== id);
    return exists;
  },
};

```

Update `mock-data`:

_./src/dals/mock-data.ts_

```diff
+ import { ObjectId } from "mongodb";
import { Book } from "./book/index.js";

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
import * as model from "#dals/index.js";
import * as apiModel from "./book.api-model.js";

export const mapBookFromModelToApi = (book: model.Book): apiModel.Book => ({
- id: book.id,
+ id: book._id.toHexString(),
  title: book.title,
  releaseDate: book.releaseDate?.toISOString(),
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

Let's check if mock mode is still working. We can update env variable `IS_API_MOCK=true` and running:

_./.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
- IS_API_MOCK=false
+ IS_API_MOCK=true
 MONGODB_URL=mongodb://localhost:27017/book-store

```

```bash
npm start
```

Check mock queries `Get book list` and `Update book`.

```
URL: http://localhost:3000/api/books
METHOD: GET
```

```
URL: http://localhost:3000/api/books/<object-id>
METHOD: PUT
BODY:
{
    "title": "Choque de reyes Actualizado",
    "releaseDate": "2022-07-21T00:00:00.000Z",
    "author": "Otro autor"
}
```

Now we can start to implement `mongodb-repository`. Update env variable `IS_API_MOCK=false` and running:

_./.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
- IS_API_MOCK=true
+ IS_API_MOCK=false
 MONGODB_URL=mongodb://localhost:27017/book-store

```

```bash
npm start
```

Implement `get book list`:

_./src/dals/book/repositories/book.mongodb-repository.ts_

```diff
+ import { dbServer } from '#core/servers/index.js';
import { BookRepository } from "./book.repository.js";
import { Book } from "../book.model.js";

export const mongoDBRepository: BookRepository = {
  getBookList: async (page?: number, pageSize?: number) => {
-   throw new Error("Not implemented");
+   return await dbServer.db.collection<Book>('books').find().toArray();
  },
...

```

Try url:

```
URL: http://localhost:3000/api/books
METHOD: GET
```

Implement `insert new book`:

_./src/dals/book/repositories/book.mongodb-repository.ts_

```diff
...
  saveBook: async (book: Book) => {
-   throw new Error("Not implemented");
+   const { insertedId } = await dbServer.db
+     .collection<Book>('books')
+     .insertOne(book);
+   return {
+     ...book,
+     _id: insertedId,
+   };
  },
...
```

Try url:

```
URL: http://localhost:3000/api/books
METHOD: POST
BODY:
{
    "title": "El señor de los anillos",
    "releaseDate": "1954-07-29T00:00:00.000Z",
    "author": "J. R. R. Tolkien"
}
```

Fetch all books:

```
URL: http://localhost:3000/api/books
METHOD: GET
```

Implement pagination:

_./src/dals/book/repositories/book.mongodb-repository.ts_

```diff
...
getBookList: async (page?: number, pageSize?: number) => {
+   const skip = Boolean(page) ? (page - 1) * pageSize : 0;
+   const limit = pageSize ?? 0;
    return await dbServer.db
      .collection<Book>('books')
      .find()
+     .skip(skip)
+     .limit(limit)
      .toArray();
  },
...
```

> [Skip](https://www.mongodb.com/docs/manual/reference/method/cursor.skip/)
>
> [Skip Nodejs API](https://mongodb.github.io/node-mongodb-native/4.8/classes/FindCursor.html#skip)
>
> [limit](https://www.mongodb.com/docs/manual/reference/method/cursor.limit/)
>
> [Limit Nodejs API](https://mongodb.github.io/node-mongodb-native/4.8/classes/FindCursor.html#limit)

Try url:

```
URLs:
http://localhost:3000/api/books?page=1&pageSize=5
http://localhost:3000/api/books?page=2&pageSize=5

METHOD: GET
```

Implement `update book`:

_./src/dals/book/repositories/book.mongodb-repository.ts_

```diff
...
  saveBook: async (book: Book) => {
-   const { insertedId } = await dbServer.db
-     .collection<Book>('books')
-     .insertOne(book);
--   return {
-     ...book,
-     _id: insertedId,
-   };
+   return await dbServer.db.collection<Book>('books').findOneAndUpdate(
+     {
+       _id: book._id,
+     },
+     { $set: book },
+     { upsert: true, returnDocument: 'after' }
+   );
  },
...

```

> NOTE: Show `updateOne` method.
>
> In the Mongo v4 does not exist `returnDocument`, but it does in >v5.
>
> [Mongo Console docs](https://docs.mongodb.com/manual/reference/method/db.collection.findOneAndUpdate/) differs from [Mongo Driver docs](https://mongodb.github.io/node-mongodb-native/3.6/api/Collection.html#findOneAndUpdate)

Try url:

```
URL: http://localhost:3000/api/books
METHOD: POST
BODY:
{
    "title": "Choque de reyes",
    "releaseDate": "1998-11-16T00:00:00.000Z",
    "author": "George R. R. Martin"
}
```

Fetch all books:

```
URL: http://localhost:3000/api/books
METHOD: GET
```

Update second book:

```
URL: http://localhost:3000/api/books/<objet-id>
METHOD: PUT
BODY:
{
    "title": "Choque de reyes Actualizado",
    "releaseDate": "2022-07-21T00:00:00.000Z",
    "author": "Otro autor"
}
```

In order to avoid repeat `db.collection<Book>("books")` we can move to a file and use it:

_./src/dals/book/book.context.ts_

```typescript
import { dbServer } from '#core/servers/index.js';
import { Book } from './book.model.js';

export const getBookContext = () => dbServer.db?.collection<Book>('books');
```

Update `mongodb-repository`:

_./src/dals/book/repositories/book.mongodb-repository.ts_

```diff
- import { dbServer } from '#core/servers/index.js';
import { BookRepository } from './book.repository.js';
import { Book } from '../book.model.js';
+ import { getBookContext } from '../book.context.js';

export const dbRepository: BookRepository = {
  getBookList: async (page?: number, pageSize?: number) => {
    const skip = Boolean(page) ? (page - 1) * pageSize : 0;
    const limit = pageSize ?? 0;
-   return await dbServer.db
-     .collection<Book>('books')
+   return await getBookContext()
      .find()
      .skip(skip)
      .limit(limit)
      .toArray();
  },
...
  saveBook: async (book: Book) => {
-   return await dbServer.db.collection<Book>('books')
+   return await getBookContext()
    .findOneAndUpdate(
      {
        _id: book._id,
      },
      { $set: book },
      { upsert: true, returnDocument: 'after' }
    );
  },
...
```

Implement `get book`:

_./src/dals/book/repositories/book.mongodb-repository.ts_

```diff
+ import { ObjectId } from "mongodb";
import { BookRepository } from './book.repository.js';
import { Book } from '../book.model.js';
import { getBookContext } from '../book.context.js';

...
  getBook: async (id: string) => {
-   throw new Error("Not implemented");
+   return await getBookContext().findOne({
+     _id: new ObjectId(id),
+   });
  },
...

```

Try url:

```
URL: http://localhost:3000/api/books/<objet-id>
METHOD: GET
```

Implement `delete book`:

_./src/dals/book/repositories/book.mongodb-repository.ts_

```diff
...
  deleteBook: async (id: string) => {
-   throw new Error("Not implemented");
+   const { deletedCount } = await getBookContext().deleteOne({
+     _id: new ObjectId(id),
+   });
+   return deletedCount === 1;
  },

```

Try url:

```
URL: http://localhost:3000/api/books/<objet-id>
METHOD: DELETE
```

Update `rest-api` to update response error:

_./src/pods/book/book.api.ts_

```diff
...
  .get('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      const book = await bookRepository.getBook(id);
+     if (book) {
        res.send(mapBookFromModelToApi(book));
+     } else {
+       res.sendStatus(404);
+     }
    } catch (error) {
      next(error);
    }
  })
...
  .put('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
+     if (await bookRepository.getBook(id)) {
        const book = mapBookFromApiToModel({ ...req.body, id });
        await bookRepository.saveBook(book);
        res.sendStatus(204);
+     } else {
+       res.sendStatus(404);
+     }
    } catch (error) {
      next(error);
    }
  })
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

> NOTE: We can improve this code using [countDocument](https://www.mongodb.com/docs/manual/reference/method/db.collection.countDocuments/) instead of get the book

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
