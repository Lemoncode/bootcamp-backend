# 04 Migrate book api

In this example we are going to migrate book API REST to GraphQL.

We will start from `03-config`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install

```

Let's continue with book GraphQL implementation, this time we will add the pagination params:

_./src/pods/book/graphql/book.type-defs.ts_

```diff
import { gql } from 'apollo-server-express';

export const bookTypeDefs = gql`
  type Book {
    id: String!
    title: String!
    releaseDate: String!
    author: String!
  }

  type Query {
-   books: [Book!]!
+   books(page: Int, pageSize: Int): [Book!]!
  }
`;

```

_./src/pods/book/graphql/book.resolvers.ts_

```diff
+ import { GraphQLFieldResolver } from 'graphql';
import { bookRepository } from 'dals';
import { Book } from '../book.api-model';
import { mapBookListFromModelToApi } from '../book.mappers';
+ import { paginateBookList } from '../book.helpers';

+ interface BookResolvers {
+   Query: {
+     books: GraphQLFieldResolver<any, any, { page?: number; pageSize?: number }>;
+   };
+ }

- export const bookResolvers = {
+ export const bookResolvers: BookResolvers = {
  Query: {
-   books: async (): Promise<Book[]> => {
+   books: async (_, { page, pageSize }): Promise<Book[]> => {
      const bookList = await bookRepository.getBookList();
-     return mapBookListFromModelToApi(bookList);
+     const paginatedBookList = paginateBookList(bookList, page, pageSize);
+     return mapBookListFromModelToApi(paginatedBookList);
    },
  },
};

```

We could use this official types but it's somewhat limited because we cannot change the returned type:

_./src/pods/book/graphql/book.resolvers.ts_

```diff
- import { GraphQLFieldResolver } from 'graphql';
+ import { GraphQLResolveInfo } from 'graphql';
+ import { IResolvers } from '@graphql-tools/utils';
import { bookRepository } from 'dals';
import { Book } from '../book.api-model';
import { mapBookListFromModelToApi } from '../book.mappers';
import { paginateBookList } from '../book.helpers';

+ // TODO: Move to common/models/graphql.model.ts
+ // Add more types when needed
+ type GraphQLResolver<ReturnedType, Args = any> = (
+   rootObject: any,
+   args: Args,
+   context: any,
+   info: GraphQLResolveInfo
+ ) => Promise<ReturnedType>;

- interface BookResolvers {
+ interface BookResolvers extends IResolvers {
  Query: {
-   books: GraphQLFieldResolver<any, any, { page?: number; pageSize?: number }>;
+   books: GraphQLResolver<Book[], { page?: number; pageSize?: number }>;
  };
}

export const bookResolvers: BookResolvers = {
  Query: {
-   books: async (_, { page, pageSize }): Promise<Book[]> => {
+   books: async (_, { page, pageSize }) => {
      const bookList = await bookRepository.getBookList();
      const paginatedBookList = paginateBookList(bookList, page, pageSize);
      return mapBookListFromModelToApi(paginatedBookList);
    },
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

_./src/pods/book/graphql/book.type-defs.ts_

```diff
import { gql } from 'apollo-server-express';

export const bookTypeDefs = gql`
  type Book {
    id: String!
    title: String!
    releaseDate: String!
    author: String!
  }

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
} from '../book.mappers';
import { paginateBookList } from '../book.helpers';

...

interface BookResolvers extends IResolvers {
  Query: {
    books: GraphQLResolver<Book[], { page?: number; pageSize?: number }>;
+   book: GraphQLResolver<Book, { id: string }>;
  };
}

export const bookResolvers: BookResolvers = {
  Query: {
    books: async (_, { page, pageSize }) => {
      const bookList = await bookRepository.getBookList();
      const paginatedBookList = paginateBookList(bookList, page, pageSize);
      return mapBookListFromModelToApi(paginatedBookList);
    },
+   book: async (_, { id }) => {
+     const book = await bookRepository.getBook(id);
+     return mapBookFromModelToApi(book);
+   },
  },
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

_./src/pods/book/graphql/book.type-defs.ts_

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
import { GraphQLResolveInfo } from 'graphql';
import { IResolvers } from '@graphql-tools/utils';
import { bookRepository } from 'dals';
import { Book } from '../book.api-model';
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
+ mapBookFromApiToModel,
} from '../book.mappers';
import { paginateBookList } from '../book.helpers';

...

interface BookResolvers extends IResolvers {
  Query: {
    books: GraphQLResolver<Book[], { page?: number; pageSize?: number }>;
    book: GraphQLResolver<Book, { id: string }>;
  };
+ Mutation: {
+   saveBook: GraphQLResolver<Book, { book: Book }>;
+ };
}

export const bookResolvers: BookResolvers = {
  Query: {
    books: async (_, { page, pageSize }) => {
      const bookList = await bookRepository.getBookList();
      const paginatedBookList = paginateBookList(bookList, page, pageSize);
      return mapBookListFromModelToApi(paginatedBookList);
    },
    book: async (_, { id }) => {
      const book = await bookRepository.getBook(id);
      return mapBookFromModelToApi(book);
    },
  },
+ Mutation: {
+   saveBook: async (_, { book }) => {
+     const modelBook = mapBookFromApiToModel(book);
+     const newBook = await bookRepository.saveBook(modelBook);
+     return mapBookFromModelToApi(newBook);
+   },
+ },
};

```

Example insert query, we could use a [variable](https://graphql.org/learn/queries/#variables) here:

```graphql
mutation($book: BookInput!) {
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

Example update query:

```graphql
mutation($book: BookInput!) {
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

The delete is another `mutation` operation, where we could send an error if we could not find the resource:

_./src/pods/book/graphql/book.type-defs.ts_

```diff
+ import { UserInputError } from 'apollo-server-express';
import { GraphQLResolveInfo } from 'graphql';

...

  type Mutation {
    saveBook(book: BookInput!): Book!
+   deleteBook(id: ID!): Boolean!
  }
`;

```

_./src/pods/book/graphql/book.resolvers.ts_

```diff
+ import { UserInputError } from 'apollo-server-express';
import { GraphQLResolveInfo } from 'graphql';
import { IResolvers } from '@graphql-tools/utils';
+ import { logger } from 'core/logger';
...

interface BookResolvers extends IResolvers {
  Query: {
    books: GraphQLResolver<Book[], { page?: number; pageSize?: number }>;
    book: GraphQLResolver<Book, { id: string }>;
  };
  Mutation: {
    saveBook: GraphQLResolver<Book, { book: Book }>;
+   deleteBook: GraphQLResolver<boolean, { id: string }>;
  };
}

export const bookResolvers: BookResolvers = {
  ...
  Mutation: {
    saveBook: async (_, { book }) => {
      const modelBook = mapBookFromApiToModel(book);
      const newBook = await bookRepository.saveBook(modelBook);
      return mapBookFromModelToApi(newBook);
    },
+   deleteBook: async (_, { id }) => {
+     const isDeleted = await bookRepository.deleteBook(id);
+     if (!isDeleted) {
+       const message = `Cannot delete book for id: ${id}`;
+       logger.warn(message);
+       throw new UserInputError(message);
+     }
+     return isDeleted;
+   },
  },
};

```

> [Apollo GraphQL Error handling](https://www.apollographql.com/docs/apollo-server/data/errors/)

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
