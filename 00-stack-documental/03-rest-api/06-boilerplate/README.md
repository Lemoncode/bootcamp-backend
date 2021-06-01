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
npm install @types/mongoose --save-dev
```

Let's refactor the db server's connection:

_./src/core/servers/db.server.ts_

```diff
- import { MongoClient, Db } from "mongodb";
+ import { connect } from "mongoose";

- let dbInstance: Db;

export const connectToDBServer = async (connectionURI: string) => {
- const client = new MongoClient(connectionURI);
- await client.connect();
- dbInstance = client.db();

+ await connect(connectionURI, {
+   useNewUrlParser: true, // https://mongoosejs.com/docs/deprecations.html#the-usenewurlparser-option
+   useUnifiedTopology: true, // https://mongoosejs.com/docs/deprecations.html#useunifiedtopology
+   useFindAndModify: false, // https://mongoosejs.com/docs/deprecations.html#findandmodify
+ });
};

- export const getDBInstance = (): Db => dbInstance;

```

> useNewUrlParser - False by default. -> True by default in Mongo driver.
> useUnifiedTopology - False by default. -> True by default in Mongo driver.
> useFindAndModify - True by default. -> Set to false to avoid use deprecated FindAndModify

Create `book context` with mongoose schema:

_./src/dals/book/book.context.ts_

```typescript
import mongoose, { Schema, SchemaDefinition } from "mongoose";
import { Book } from "./book.model";

const bookSchema = new Schema({
  title: { type: Schema.Types.String, required: true },
  releaseDate: { type: Schema.Types.Date, required: true },
  author: { type: Schema.Types.String, required: true },
} as SchemaDefinition<Book>);

export const BookContext = mongoose.model<Book>("Book", bookSchema);

```

> [Connection buffering](https://mongoosejs.com/docs/connections.html#buffering)
> [Mongoose Schema](https://mongoosejs.com/docs/guide.html)
> Notice the Book model is defined as singular but it will be mapped to `books` collection, [reference](https://mongoosejs.com/docs/models.html#compiling)

Now, we could update the `db repository`:

_./src/dals/book/respositories/book.db-repository.ts_

```diff
import { ObjectId } from "mongodb";
- import { getDBInstance } from "core/servers";
+ import { BookContext } from "../book.context";
import { BookRepository } from "./book.repository";
import { Book } from "../book.model";

export const dbRepository: BookRepository = {
  getBookList: async () => {
-   const db = getDBInstance();
-   return await db.collection<Book>("books").find().toArray();
+   return await BookContext.find();
  },
...
};

```

> Set breakpoints to check the errors

Why it fails? Due to [Mongoose models](https://mongoosejs.com/docs/queries.html) provide several static helper functions, each of these functions returns mongoose query objects. Getting a better performance, we should use `lean` method:

_./src/dals/book/respositories/book.db-repository.ts_

```diff
...

export const dbRepository: BookRepository = {
- getBookList: async () => {
-   return await BookContext.find();
- },
+ getBookList: async () => await BookContext.find().lean(),
...

```

Update `get book`:

_./src/dals/book/respositories/book.db-repository.ts_

```diff
...
- getBook: async (id: string) => {
+ getBook: async (id: string) =>
-   const db = getDBInstance();
-   return await db.collection<Book>("books").findOne({
-     _id: new ObjectId(id),
-   });
- },
+   await BookContext.findOne({ _id: new ObjectId(id) }).lean(),
...

```

Update `save book`:

_./src/dals/book/respositories/book.db-repository.ts_

```diff
...
- saveBook: async (book: Book) => {
+ saveBook: async (book: Book) =>
-   const db = getDBInstance();
-   const { value } = await db.collection<Book>("books").findOneAndUpdate(
+   await BookContext.findOneAndUpdate(
      {
        _id: book._id,
      },
      { $set: book },
-     { upsert: true, returnDocument: "after" }
+     { upsert: true, new: true }
-   );
-   return value;
- },
+   ).lean(),
...

```

Update `delete book`:

_./src/dals/book/respositories/book.db-repository.ts_

```diff
...
  deleteBook: async (id: string) => {
-   const db = getDBInstance();
-   const { deletedCount } = await db.collection<Book>("books").deleteOne({
+   const { deletedCount } = await BookContext.deleteOne({
      _id: new ObjectId(id),
    });
    return deletedCount === 1;
  },
};

```

Finally, if we want to log each query that `mongoose` request to mongo, we could use the `debug` mode:

_./src/core/servers/db.server.ts_

```diff
import { connect } from "mongoose";
+ import { envConstants } from "core/constants";

+ set("debug", !envConstants.isProduction);

export const connectToDBServer = async (connectionURI: string) => {
...

```

> [Reference](https://mongoosejs.com/docs/api.html#mongoose_Mongoose-set)

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
