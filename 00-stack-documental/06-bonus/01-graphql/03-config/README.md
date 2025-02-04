# 03 Config

In this example we are going to install and configure a GraphQL server.

We will start from `02-boilerplate`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install

```

# Libraries

We are going to install the main library to work with graphql and express, [graphql-http](https://github.com/graphql/graphql-http).

> [Documentation page](https://graphql.org/learn/)
>
> [grapqhl package docs](https://graphql.org/graphql-js/)

```bash
npm install graphql-http graphql  --save

```

> It has `graphql` lib as dependency.
>
> [Playground](https://github.com/graphql/graphql-playground/tree/main)

# Add Playground

_./src/core/graphql/playground.html_

```html
<!DOCTYPE html>
<html lang="en">
  <head>
    <title>GraphiQL</title>
    <style>
      body {
        height: 100%;
        margin: 0;
        width: 100%;
        overflow: hidden;
      }

      #graphiql {
        height: 100vh;
      }
    </style>
    <script
      crossorigin
      src="https://unpkg.com/react@18/umd/react.production.min.js"
    ></script>
    <script
      crossorigin
      src="https://unpkg.com/react-dom@18/umd/react-dom.production.min.js"
    ></script>
    <script
      src="https://unpkg.com/graphiql/graphiql.min.js"
      type="application/javascript"
    ></script>
    <link rel="stylesheet" href="https://unpkg.com/graphiql/graphiql.min.css" />
    <script
      src="https://unpkg.com/@graphiql/plugin-explorer/dist/index.umd.js"
      crossorigin
    ></script>

    <link
      rel="stylesheet"
      href="https://unpkg.com/@graphiql/plugin-explorer/dist/style.css"
    />
  </head>

  <body>
    <div id="graphiql">Loading...</div>
    <script>
      const root = ReactDOM.createRoot(document.getElementById('graphiql'));
      const fetcher = GraphiQL.createFetcher({
        url: '/graphql',
      });
      const explorerPlugin = GraphiQLPluginExplorer.explorerPlugin();
      root.render(
        React.createElement(GraphiQL, {
          fetcher,
          defaultEditorToolsVisibility: true,
          plugins: [explorerPlugin],
        })
      );
    </script>
  </body>
</html>
```

# Config

Let's create a dummy `GraphQL server` to check it how it works:

_./src/index.ts_

```diff
import '#core/monitoring.js';
import express from 'express';
import path from 'node:path';
+ import { createHandler } from 'graphql-http/lib/use/express';
+ import { buildSchema } from 'graphql';

...

app.use(
  '/',
  express.static(path.resolve(import.meta.dirname, ENV.STATIC_FILES_PATH))
);

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
+ app.use('/graphql', createHandler({ schema, rootValue: resolvers }));
+ if (!ENV.IS_PRODUCTION) {
+   app.use('/playground', async (req, res) => {
+     res.sendFile(
+       path.join(import.meta.dirname, './core/graphql/playground.html')
+     );
+   });
+ }

...

  logger.info(`Server ready at port ${ENV.PORT}`);
+ logger.info(`GraphQL Server ready at port ${ENV.PORT}/graphql`);
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
import path from 'node:path';
import { Express } from 'express';
import { createHandler, HandlerOptions } from 'graphql-http/lib/use/express';
import { ENV } from '#core/constants/index.js';

export const createGraphqlServer = (
  expressApp: Express,
  options: HandlerOptions
) => {
  expressApp.use('/graphql', createHandler(options));
  if (!ENV.IS_PRODUCTION) {
    expressApp.use('/playground', async (req, res) => {
      res.sendFile(
        path.join(import.meta.dirname, '../graphql/playground.html')
      );
    });
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
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from '#common/middlewares/index.js';
import {
  createRestApiServer,
  dbServer,
+ createGraphqlServer,
} from '#core/servers/index.js';
...


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
- app.use('/graphql', createHandler({ schema, rootValue: resolvers }));
- if (!ENV.IS_PRODUCTION) {
-   app.use('/playground', graphqlPlayground({ endpoint: '/graphql' }));
- }
+ createGraphqlServer(app, { schema: null, rootValue: null });

app.use(logRequestMiddleware(logger));

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
export * from './book.resolvers.js';
export * from './book.schema.js';

```

Update barrel file:

_./src/pods/book/index.ts_

```diff
export * from "./book.api.js";
+ export * from './graphql/index.js';

```

Update app:

_./src/index.ts_

```diff
...

- import { bookApi } from '#pods/book/index.js';
+ import { bookApi, bookSchema, bookResolvers } from '#pods/book/index.js';
import { securityApi, authenticationMiddleware } from '#pods/security/index.js';
import { userApi } from '#pods/user/index.js';

...
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
