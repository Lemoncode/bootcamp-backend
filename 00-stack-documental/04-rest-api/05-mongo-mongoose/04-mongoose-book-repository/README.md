# 03 Mongoose book repository

In this example we are going to implement book repository with Mongoose.

We will start from `03-mongo-book-repository`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

Let's install [mongoose](https://github.com/Automattic/mongoose):

```bash
npm install mongoose --save

```

> It includes the typings file.

Let's refactor the db server's connection:

_./src/core/servers/db.server.ts_

```diff
- import { MongoClient, Db } from "mongodb";
+ import { connect } from "mongoose";

- export let db: Db;

export const connectToDBServer = async (connectionURI: string) => {
- const client = new MongoClient(connectionURI);
- await client.connect();

- db = client.db();

+ await connect(connectionURI);
};

```

Update `book context` with mongoose schema:

_./src/dals/book/book.context.ts_

```diff
- import { db } from '#core/servers/index.js';
+ import { model, Schema } from 'mongoose';
import { Book } from './book.model.js';

+ const bookSchema = new Schema<Book>({
+   title: { type: Schema.Types.String, required: true },
+   releaseDate: { type: Schema.Types.Date, required: true },
+   author: { type: Schema.Types.String, required: true },
+ });


- export const getBookContext = () => db?.collection<Book>('books');
+ export const bookContext = model<Book>('Book', bookSchema);

```

> [Connection buffering](https://mongoosejs.com/docs/connections.html#buffering)
>
> [Mongoose Schema](https://mongoosejs.com/docs/guide.html)
>
> Notice the Book model is defined as singular but it will be mapped to `books` collection, [reference](https://mongoosejs.com/docs/models.html#compiling)

Now, we could update the `db repository`:

_./src/dals/book/respositories/book.db-repository.ts_

```diff
import { ObjectId } from 'mongodb';
import { BookRepository } from './book.repository.js';
import { Book } from '../book.model.js';
- import { getBookContext } from '../book.context.js';
+ import { bookContext } from '../book.context.js';

export const dbRepository: BookRepository = {
  getBookList: async (page?: number, pageSize?: number) => {
    const skip = Boolean(page) ? (page - 1) * pageSize : 0;
    const limit = pageSize ?? 0;
-   return await getBookContext().find().skip(skip).limit(limit).toArray();
+   return await bookContext.find().skip(skip).limit(limit);
  },
...
};

```

> Set breakpoints to check returned array
>
> Comment build errors.

Try url:

```
URL: http://localhost:3000/api/books
METHOD: GET
```

Due to [Mongoose models](https://mongoosejs.com/docs/queries.html) provide several static helper functions, each of these functions returns mongoose query objects. Getting a better performance, we should use `lean` method:

_./src/dals/book/respositories/book.db-repository.ts_

```diff
...

export const dbRepository: BookRepository = {
  getBookList: async (page?: number, pageSize?: number) => {
    const skip = Boolean(page) ? (page - 1) * pageSize : 0;
    const limit = pageSize ?? 0;
-   return await bookContext.find().skip(skip).limit(limit);
+   return await bookContext.find().skip(skip).limit(limit).lean();
  },
...

```

Update `get book`:

_./src/dals/book/respositories/book.db-repository.ts_

```diff
...
  getBook: async (id: string) => {
-   return await getBookContext().findOne({
+   return await bookContext.findOne({
      _id: new ObjectId(id),
-   });
+   })
+   .lean();
  },
...

```

> It throws an error, `Unsupported BSON version, bson types must be from bson 5.0 or later`.
>
> Check [release notes](https://github.com/mongodb/node-mongodb-native/releases/)
>
> Try `_id: id,` on query. It works here but it doesn't on aggregations.
>

Downgrade mongodb driver version to 5:

```bash
npm install mongodb@5 --save

```

Try again the `get one` query:

```bash
npm start

```

Update `save book`:

_./src/dals/book/respositories/book.db-repository.ts_

```diff
...
  saveBook: async (book: Book) => {
-   return await getBookContext().findOneAndUpdate(
+   return await bookContext.findOneAndUpdate(
      {
        _id: book._id,
      },
      { $set: book },
      { upsert: true, returnDocument: 'after' }
    )
+   .lean();
  },
...

```

> Body

```
{
    "title": "El señor de los anillos",
    "releaseDate": "1954-07-29T00:00:00.000Z",
    "author": "J. R. R. Tolkien"
}
```

> NOTE: It is not necessary to update delete method.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
