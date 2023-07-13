# 04 Migrate book api

In this example we are going to migrate book API REST to GraphQL.

We will start from `03-config`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install

```

Let's continue with book GraphQL implementation, this time we will add the pagination params:

_./src/pods/book/graphql/book.schema.ts_

```diff
...
  type Query {
-   books: [Book!]!
+   books(page: Int, pageSize: Int): [Book!]!
  }
`);

```

_./src/pods/book/graphql/book.resolvers.ts_

```diff
+ import { GraphQLResolveInfo } from 'graphql';
import { bookRepository } from '#dals/index.js';
import { Book } from '../book.api-model.js';
import { mapBookListFromModelToApi } from '../book.mappers.js';

+ // TODO: Move to common/models/graphql.model.ts
+ // Pending to add Context type
+ type GraphQLResolver<Args, Context, ReturnType> = (
+   args: Args,
+   context: Context,
+   info: GraphQLResolveInfo
+ ) => Promise<ReturnType>;

+ interface BookResolvers {
+   books: GraphQLResolver<{ page?: number; pageSize?: number }, unknown, Book[]>;
+ }

- export const bookResolvers = {
+ export const bookResolvers: BookResolvers = {
- books: async (): Promise<Book[]> => {
+ books: async ({ page, pageSize }) => {
-   const bookList = await bookRepository.getBookList();
+   const bookList = await bookRepository.getBookList(page, pageSize);
    return mapBookListFromModelToApi(bookList);
  },
};

```

Now, we can run and check it:

```bash
npm start
```

- Example queries:

```graphql
query {
  books {
    id
    title
    releaseDate
    author
  }
}
```

```graphql
query {
  books(page: 2, pageSize: 3) {
    id
    title
    releaseDate
    author
  }
}
```

Let's define get book details:

_./src/pods/book/graphql/book.schema.ts_

```diff
...
  type Query {
    books(page: Int, pageSize: Int): [Book!]!
+   book(id: ID!): Book!
  }
`;

```

_./src/pods/book/graphql/book.resolvers.ts_

```diff
...

import {
  mapBookListFromModelToApi,
+ mapBookFromModelToApi,
} from '../book.mappers.js';

...

interface BookResolvers {
  books: GraphQLResolver<{ page?: number; pageSize?: number }, unknown, Book[]>;
+ book: GraphQLResolver<{ id: string }, unknown, Book>;
}

export const bookResolvers: BookResolvers = {
  books: async ({ page, pageSize }) => {
    const bookList = await bookRepository.getBookList(page, pageSize);
    return mapBookListFromModelToApi(bookList);
  },
+ book: async ({ id }) => {
+   const book = await bookRepository.getBook(id);
+   return mapBookFromModelToApi(book);
+ },
};


```

Example query:

```graphql
query {
  book(id: "<id-value>") {
    id
    title
    releaseDate
    author
  }
}
```

Let's implement `saveBook` method:

_./src/pods/book/graphql/book.schema.ts_

```diff
...

  type Query {
    books(page: Int, pageSize: Int): [Book!]!
    book(id: ID!): Book!
  }

+ input BookInput {
+   id: String
+   title: String!
+   releaseDate: String!
+   author: String!
+ }

+ type Mutation {
+   saveBook(book: BookInput!): Book!
+ }
`;

```

_./src/pods/book/graphql/book.resolvers.ts_

```diff
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
+ mapBookFromApiToModel,
} from '../book.mappers.js';

...

interface BookResolvers {
  books: GraphQLResolver<{ page?: number; pageSize?: number }, unknown, Book[]>;
  book: GraphQLResolver<{ id: string }, unknown, Book>;
+ saveBook: GraphQLResolver<{ book: Book }, unknown, Book>;
}


export const bookResolvers: BookResolvers = {
  ...
+ saveBook: async ({ book }) => {
+   const bookToSave = mapBookFromApiToModel(book);
+   const savedBook = await bookRepository.saveBook(bookToSave);
+   return mapBookFromModelToApi(savedBook);
+ },
};

```

Example insert mutation, we could use a [variable](https://graphql.org/learn/queries/#variables) here:

```graphql
mutation ($book: BookInput!) {
  saveBook(book: $book) {
    id
    title
    releaseDate
    author
  }
}
```

Variable

```json
{
  "book": {
    "title": "my-book",
    "releaseDate": "2021-07-14T12:30:00",
    "author": "my-author"
  }
}
```

Example update mutation:

```graphql
mutation ($book: BookInput!) {
  saveBook(book: $book) {
    id
    title
    releaseDate
    author
  }
}
```

Variable

```json
{
  "book": {
    "id": "<id-value>",
    "title": "Updated title",
    "releaseDate": "2021-07-14T12:30:00",
    "author": "Updated author"
  }
}
```

The delete is another `mutation` operation, where we could send an error if we could not find the resource:

_./src/pods/book/graphql/book.schema.ts_

```diff
...

  type Mutation {
    saveBook(book: BookInput!): Book!
+   deleteBook(id: ID!): Boolean!
  }
`;

```

_./src/pods/book/graphql/book.resolvers.ts_

```diff
import { GraphQLResolveInfo } from 'graphql';
+ import { logger } from '#core/logger/index.js';
import { bookRepository } from '#dals/index.js';
...

interface BookResolvers {
  ...
  saveBook: GraphQLResolver<{ book: Book }, unknown, Book>;
+ deleteBook: GraphQLResolver<{ id: string }, unknown, boolean>;
}

export const bookResolvers: BookResolvers = {
  ...
+ deleteBook: async ({ id }) => {
+   const isDeleted = await bookRepository.deleteBook(id);
+   if (!isDeleted) {
+     const message = `Cannot delete book with id: ${id}`;
+     logger.warn(message);
+     throw new Error(JSON.stringify({ message, statusCode: 404 }));
+   }
+   return true;
+ },
};

```

Format custom errors:

_./src/index.ts_

```diff
...
createGraphqlServer(restApiServer, {
  schema: bookSchema,
  rootValue: bookResolvers,
+ formatError: (error) => {
+   const { message, statusCode } = JSON.parse(error.message);
+   return {
+     ...error,
+     message,
+     extensions: {
+       statusCode,
+     },
+   };
+ },
});
...
```

Example delete mutation (getting error):

```graphql
mutation {
  deleteBook(id: "invalid")
}
```

> [Graphql Errors Shape](https://graphql.org/graphql-js/error/#graphqlerror)


Example delete mutation (valid id):

```graphql
mutation {
  deleteBook(id: "<id-value>")
}
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
