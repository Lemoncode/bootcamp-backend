# 03 Config

In this example we are going to install and configure a GraphQL server.

We will start from `02-boilerplate`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install

```

# Libraries

We are going to install the main library to work with graphql and express, [graphql-http](https://github.com/graphql/graphql-http). ([Documentation page](https://graphql.org/graphql-js/))

```bash
npm install graphql-http graphql graphql-playground-middleware-express --save

```

> It has `graphql` lib as dependency.
>
> [Playground](https://github.com/graphql/graphql-playground/tree/main)

# Config

ApolloServer comes with types definitions and we don't need an extra package for TypeScript. We have to define a new `ApolloServer` instance to create a new `GraphQL Server`:

_./src/index.ts_

```diff
import '#core/load-env.js';
import '#core/monitoring.js';
import express from 'express';
import path from 'path';
import url from 'url';
+ import { createHandler } from 'graphql-http/lib/use/express';
+ import { buildSchema } from 'graphql';
+ import playground from 'graphql-playground-middleware-express';
+ const graphqlPlayground = playground.default;

...

restApiServer.use('/', express.static(staticFilesPath));

+ const schema = buildSchema(`
+   type Query {
+     hello: String
+   }
+ `);
+ const resolvers = {
+   hello: () => {
+     return 'Working endpoint!';
+   },
+ };
+ restApiServer.use('/graphql', createHandler({ schema, rootValue: resolvers }));
+ if (!envConstants.isProduction) {
+   restApiServer.use('/playground', graphqlPlayground({ endpoint: '/graphql' }));
+ }

...

  logger.info(`Server ready at port ${envConstants.PORT}`);
+ logger.info(`GraphQL Server ready at port ${envConstants.PORT}/graphql`);
});

```

Let's run it:

```bash
npm start

```

> Open GraphQL server at [http://localhost:3000/graphql](http://localhost:3000/graphql)
>
> Open GraphQL playground at [http://localhost:3000/playground](http://localhost:3000/playground)
>
> Check docs
>
> All queries are stored in localStorage.

Let's move GraphQL Server config to `core`:

_./src/core/servers/graphql.server.ts_

```javascript
import { Express } from 'express';
import { createHandler, HandlerOptions } from 'graphql-http/lib/use/express';
import playground from 'graphql-playground-middleware-express';
const graphqlPlayground = playground.default;
import { envConstants } from '#core/constants/index.js';

export const createGraphqlServer = (
  expressApp: Express,
  options: HandlerOptions
) => {
  expressApp.use('/graphql', createHandler(options));
  if (!envConstants.isProduction) {
    expressApp.use('/playground', graphqlPlayground({ endpoint: '/graphql' }));
  }
};

```

Update barrel:

_./src/core/servers/index.ts_

```diff
export * from './rest-api.server.js';
export * from './db.server.js';
+ export * from './graphql.server.js';

```

Update app:

_./src/index.ts_

```diff
import '#core/load-env.js';
import '#core/monitoring.js';
import express from 'express';
import path from 'path';
import url from 'url';
- import { createHandler } from 'graphql-http/lib/use/express';
- import { buildSchema } from 'graphql';
- import playground from 'graphql-playground-middleware-express';
- const graphqlPlayground = playground.default;
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from '#common/middlewares/index.js';
import {
  createRestApiServer,
  connectToDBServer,
+ createGraphqlServer,
} from '#core/servers/index.js';
import { envConstants } from '#core/constants/index.js';
import { logger } from '#core/logger/index.js';
import { booksApi } from '#pods/book/index.js';
import { securityApi, authenticationMiddleware } from '#pods/security/index.js';
import { userApi } from '#pods/user/index.js';

const restApiServer = createRestApiServer();

const __dirname = path.dirname(url.fileURLToPath(import.meta.url));
const staticFilesPath = path.resolve(__dirname, envConstants.STATIC_FILES_PATH);
restApiServer.use('/', express.static(staticFilesPath));

- const schema = buildSchema(`
-   type Query {
-     hello: String
-   }
- `);
- const resolvers = {
-   hello: () => {
-     return 'Working endpoint!';
-   },
- };
- restApiServer.use('/graphql', createHandler({ schema, rootValue: resolvers }));
- if (!envConstants.isProduction) {
-   restApiServer.use('/playground', graphqlPlayground({ endpoint: '/graphql' }));
- }
+ createGraphqlServer(restApiServer, { schema: null, rootValue: null });

restApiServer.use(logRequestMiddleware(logger));

...

```

> Pending to add `schema` and `resolvers`.

Now, we can start implementing `book` GraphQL Schema:

_./src/pods/book/graphql/book.schema.ts_

```javascript
import { buildSchema as graphql } from 'graphql';

export const bookSchema = graphql(`
  type Book {
    id: String!
    title: String!
    releaseDate: String!
    author: String!
  }

  type Query {
    books: [Book!]!
  }
`);

```

> Install > [VSCode Extension](https://marketplace.visualstudio.com/items?itemName=GraphQL.vscode-graphql-syntax).
>
> [Scalar Types](https://graphql.org/learn/schema/#scalar-types)

Implementing resolvers:

_./src/pods/book/graphql/book.resolvers.ts_

```javascript
import { bookRepository } from '#dals/index.js';
import { Book } from '../book.api-model.js';
import { mapBookListFromModelToApi } from '../book.mappers.js';

export const bookResolvers = {
  books: async (): Promise<Book[]> => {
    const bookList = await bookRepository.getBookList();
    return mapBookListFromModelToApi(bookList);
  },
};

```

Add barrel file:

_./src/pods/book/graphql/index.ts_

```javascript
export * from './book.resolver.js';
export * from './book.schema.js';

```

Update barrel file:

_./src/pods/book/index.ts_

```diff
export * from "./book.rest-api.js";
+ export * from './graphql/index.js';

```

Update app:

_./src/index.ts_

```diff
...
import { envConstants } from '#core/constants/index.js';
import { logger } from '#core/logger/index.js';
- import { booksApi } from '#pods/book/index.js';
+ import { booksApi, bookSchema, bookResolvers } from '#pods/book/index.js';
import { securityApi, authenticationMiddleware } from '#pods/security/index.js';
import { userApi } from '#pods/user/index.js';

...
restApiServer.use('/', express.static(staticFilesPath));
createGraphqlServer(restApiServer, {
- schema: null,
+ schema: bookSchema,
- rootValue: null,
+ rootValue: bookResolvers,
});

...

```

Now, we can run and check it:

```bash
npm start
```

- Example query:

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

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
