# 05 Auth

In this example we are going to add authentication / authorization with GraphQL.

We will start from `04-migrate-book-api`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install

```

Let's migrate `security` rest api:

_./src/pods/security/graphql/security.type-defs.ts_

```javascript
import { gql } from 'apollo-server-express';

// TODO: Implement Void Custom Scalar Type

export const securityTypeDefs = gql`
  type Mutation {
    login(email: String!, password: String!): Boolean
    logout: Boolean
  }
`;

```

> [Custom Scalar Types](https://www.apollographql.com/docs/apollo-server/schema/custom-scalars/)

Add resolvers:

_./src/pods/security/graphql/security.resolvers.ts_

```javascript
import { IResolvers } from '@graphql-tools/utils';
import { GraphQLResolver } from 'common/models';

interface SecurityResolvers extends IResolvers {
  Mutation: {
    login: GraphQLResolver<void, { email: string; password: string }>;
    logout: GraphQLResolver<void>;
  };
}

export const securityResolvers: SecurityResolvers = {
  Mutation: {
    login: async (_, { email, password }) => {
      console.log({ email, password });
    },
    logout: async () => {},
  },
};

```

Add barrel file:

_./src/pods/security/graphql/index.ts_

```javascript
export * from './security.type-defs';
export * from './security.resolvers';

```

Update barrel file:

_./src/pods/security/index.ts_

```diff
export * from './security.rest-api';
export * from './security.middlewares';
+ export * from './graphql';

```

Move GraphQLResolver to `common/models`:

_./src/common/models/graphql.ts_

```javascript
import { GraphQLResolveInfo } from 'graphql';

export type GraphQLResolver<ReturnedType, Args = any> = (
  rootObject: any,
  args: Args,
  context: any,
  info: GraphQLResolveInfo
) => Promise<ReturnedType>;

```

Add barrel file:

_./src/common/models/index.ts_

```javascript
export * from './graphql';

```

Update `book resolvers`:

_./src/pods/book/graphql/book.resolvers.ts_

```diff
import { UserInputError } from 'apollo-server-express';
- import { GraphQLResolveInfo } from 'graphql';
import { IResolvers } from '@graphql-tools/utils';
+ import { GraphQLResolver } from 'common/models';
import { logger } from 'core/logger';
import { bookRepository } from 'dals';
import { Book } from '../book.api-model';
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from '../book.mappers';
import { paginateBookList } from '../book.helpers';

- // TODO: Move to common/models/graphql.model.ts
- // Add more types when needed
- type GraphQLResolver<ReturnedType, Args = any> = (
-   rootObject: any,
-   args: Args,
-   context: any,
-   info: GraphQLResolveInfo
- ) => Promise<ReturnedType>;

...

```

Update app:

```diff
...
import { booksApi, bookTypeDefs, bookResolvers } from 'pods/book';
import {
  securityApi,
  authenticationMiddleware,
+ securityTypeDefs,
+ securityResolvers,
} from 'pods/security';
import { userApi } from 'pods/user';

(async function () {
  const restApiServer = createRestApiServer();
  const graphqlServer = await createGraphQLServer(restApiServer, {
-   typeDefs: bookTypeDefs,
+   typeDefs: [bookTypeDefs, securityTypeDefs],
-   resolvers: bookResolvers,
+   resolvers: [bookResolvers, securityResolvers],
  });
```

Why throw the error `Error: There can be only one type named "Mutation"`? Because we are defining twice the `Mutation` type. We should define only once and `extend` it:



_./src/core/servers/graphql.server.ts_

```diff
import {
  ApolloServer,
  ApolloServerExpressConfig,
+ gql,
} from 'apollo-server-express';
+ import { DocumentNode } from 'graphql';

+ // NOTE: We cannot defined empty type.
+ const coreTypeDefs = gql`
+   type Query {
+     _empty: String
+   }
+
+   type Mutation {
+     _empty: String
+   }
+ `;

export const createGraphQLServer = async (
  expressApp,
  config: ApolloServerExpressConfig
) => {
+ const typeDefs = Array.isArray(config.typeDefs)
+   ? (config.typeDefs as DocumentNode[])
+   : ([config.typeDefs] as DocumentNode[]);

- const graphqlServer = new ApolloServer(config);
+ const graphqlServer = new ApolloServer({
+   ...config,
+   typeDefs: [coreTypeDefs, ...typeDefs],
+ });
  await graphqlS
  await graphqlServer.start();

  graphqlServer.applyMiddleware({ app: expressApp });
  return graphqlServer;
};

```

Update `type-defs`:

_./src/pods/book/graphql/book.type-defs.ts_

```diff
...

- type Query {
+ extend type Query {
    books(page: Int, pageSize: Int): [Book!]!
    book(id: ID!): Book!
  }
...

- type Mutation {
+ extend type Mutation {
    saveBook(book: BookInput!): Book!
    deleteBook(id: ID!): Boolean!
  }
`;

```

_./src/pods/security/graphql/security.type-defs.ts_

```diff
import { gql } from 'apollo-server-express';

// TODO: Implement Void Custom Scalar Type

export const securityTypeDefs = gql`
- type Mutation {
+ extend type Mutation {
    login(email: String!, password: String!): Boolean
    logout: Boolean
  }
`;

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
