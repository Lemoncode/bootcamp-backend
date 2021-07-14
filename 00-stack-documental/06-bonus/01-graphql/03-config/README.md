# 03 Config

In this example we are going to install and configure a GraphQL server.

We will start from `02-boilerplate`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install

```

# Libraries

We are going to install the main library to work with graphql and express, [apollo-server-express](https://www.npmjs.com/package/apollo-server-express). ([Documentation page](https://www.apollographql.com/docs/apollo-server/))

```bash
npm install apollo-server-express graphql --save

```
> It has `graphql` lib as peerDependency.

# Config

ApolloServer comes with types definitions and we don't need an extra package for TypeScript. We have to define a new `ApolloServer` instance to create a new `GraphQL Server`:

_./src/app.ts_

```diff
import express from 'express';
import path from 'path';
+ import { ApolloServer } from 'apollo-server-express';
import { createRestApiServer, connectToDBServer } from 'core/servers';
import { envConstants } from 'core/constants';
import { logger } from 'core/logger';
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from 'common/middlewares';
import { booksApi } from 'pods/book';
import { securityApi, authenticationMiddleware } from 'pods/security';
import { userApi } from 'pods/user';

+ (async function () {
    const restApiServer = createRestApiServer();
+   const graphqlServer = new ApolloServer({});
+   await graphqlServer.start();
+   graphqlServer.applyMiddleware({ app: restApiServer as any });

    const staticFilesPath = path.resolve(
      __dirname,
      envConstants.STATIC_FILES_PATH
    );
    restApiServer.use('/', express.static(staticFilesPath));

    restApiServer.use(logRequestMiddleware(logger));

    restApiServer.use('/api/security', securityApi);
    restApiServer.use('/api/books', authenticationMiddleware, booksApi);
    restApiServer.use('/api/users', authenticationMiddleware, userApi);

    restApiServer.use(logErrorRequestMiddleware(logger));

    restApiServer.listen(envConstants.PORT, async () => {
      if (!envConstants.isApiMock) {
        await connectToDBServer(envConstants.MONGODB_URI);
        logger.info('Connected to DB');
      } else {
        logger.info('Running API mock');
      }
      logger.info(`Server ready at port ${envConstants.PORT}`);
+     logger.info(
+       `GraphQL server ready at http://localhost:${envConstants.PORT}${graphqlServer.graphqlPath}`
+     );
    });
+ })();

```

> Since v3 we have to use `graphqlServer.start` method. [Reference](https://github.com/apollographql/apollo-server/blob/main/CHANGELOG.md#changes-to-nodejs-framework-integrations)

Let's create a dummy endpoint to test it:

_./src/app.ts_

```diff
import express from 'express';
import path from 'path';
- import { ApolloServer } from 'apollo-server-express';
+ import { ApolloServer, gql } from 'apollo-server-express';
...

+ const typeDefs = gql`
+   type Query {
+     hello: String!
+   }
+ `;

+ const resolvers = {
+   Query: {
+     hello: () => {
+       return 'Working endpoint!';
+     },
+   },
+ };

(async function () {
  const restApiServer = createRestApiServer();
- const graphqlServer = new ApolloServer({});
+ const graphqlServer = new ApolloServer({
+   typeDefs,
+   resolvers,
+ });
  await graphqlServer.start();
  graphqlServer.applyMiddleware({ app: restApiServer as any });
...
```

Let's run it:

```bash
npm start

```

> Open GraphQL server at [http://localhost:3000/graphql](http://localhost:3000/graphql)
>
> Check grapql tool, only available at development mode
>
> Check docs
>
> All queries are stored in localStorage.
>
> We could enable previous [GraphQL Playground](https://www.apollographql.com/docs/apollo-server/migration/#graphql-playground)

Let's move GraphQL Server config to `core`:

_./src/core/servers/graphql.server.ts_

```javascript
import { ApolloServer, ApolloServerExpressConfig } from 'apollo-server-express';

export const createGraphQLServer = async (
  expressApp,
  config: ApolloServerExpressConfig
) => {
  const graphqlServer = new ApolloServer(config);
  await graphqlServer.start();

  graphqlServer.applyMiddleware({ app: expressApp });
  return graphqlServer;
};

```

Update barrel:

_./src/core/servers/index.ts_

```diff
export * from './rest-api.server';
export * from './db.server';
+ export * from './graphql.server';

```

Update app:

_./src/app.ts_

```diff
import express from 'express';
import path from 'path';
- import { ApolloServer, gql } from 'apollo-server-express';
import {
  createRestApiServer,
  connectToDBServer,
+ createGraphQLServer,
} from 'core/servers';
import { envConstants } from 'core/constants';
...

- const typeDefs = gql`
-   type Query {
-     hello: String!
-   }
- `;

- const resolvers = {
-   Query: {
-     hello: () => {
-       return 'Working endpoint!';
-     },
-   },
- };

(async function () {
  const restApiServer = createRestApiServer();
- const graphqlServer = new ApolloServer({
-   typeDefs,
-   resolvers,
- });
+ const graphqlServer = await createGraphQLServer(restApiServer, {
+   typeDefs: null,
+   resolvers: null,
+ });
- await graphqlServer.start();
- graphqlServer.applyMiddleware({ app: restApiServer as any });

...

```

Now, we can start implementing `book` GraphQL Schema:

_./src/pods/book/graphql/book.type-defs.ts_

```javascript
import { gql } from 'apollo-server-express';

export const bookTypeDefs = gql`
  type Book {
    id: String!
    title: String!
    releaseDate: String!
    author: String!
  }

  type Query {
    books: [Book!]!
  }
`;

```

> Install [Apollo GraphQL VSCode extension](https://www.apollographql.com/docs/devtools/editor-plugins/).
>
> [Scalar Types](https://graphql.org/learn/schema/#scalar-types)

Implementing resolvers:

_./src/pods/book/graphql/book.resolvers.ts_

```javascript
import { bookRepository } from 'dals';
import { Book } from '../book.api-model';
import { mapBookListFromModelToApi } from '../book.mappers';

export const bookResolvers = {
  Query: {
    books: async (): Promise<Book[]> => {
      const bookList = await bookRepository.getBookList();
      return mapBookListFromModelToApi(bookList);
    },
  },
};

```

Add barrel file:

_./src/pods/book/graphql/index.ts_

```javascript
export * from './book.type-defs';
export * from './book.resolvers';

```

Update barrel file:

_./src/pods/book/index.ts_

```diff
export * from './book.rest-api';
+ export * from './graphql';

```

Update app:

_./src/app.ts_

```diff
import express from 'express';
import path from 'path';
import {
  createRestApiServer,
  connectToDBServer,
  createGraphQLServer,
} from 'core/servers';
import { envConstants } from 'core/constants';
import { logger } from 'core/logger';
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from 'common/middlewares';
- import { booksApi } from 'pods/book';
+ import { booksApi, bookTypeDefs, bookResolvers } from 'pods/book';
import { securityApi, authenticationMiddleware } from 'pods/security';
import { userApi } from 'pods/user';

(async function () {
  const restApiServer = createRestApiServer();
  const graphqlServer = createGraphQLServer(restApiServer, {
-   typeDefs: null,
+   typeDefs: bookTypeDefs,
-   resolvers: null,
+   resolvers: bookResolvers,
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
