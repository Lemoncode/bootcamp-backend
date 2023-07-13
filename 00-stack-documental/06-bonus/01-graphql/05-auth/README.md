# 05 Auth

In this example we are going to add authentication / authorization with GraphQL.

We will start from `04-migrate-book-api`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install

```

Let's migrate `security` rest api:

_./src/pods/security/graphql/security.schema.ts_

```javascript
import { buildSchema as graphql } from 'graphql';

// TODO: Implement Void Custom Scalar Type

export const securitySchema = graphql(`
  type Mutation {
    login(email: String!, password: String!): Boolean
    logout: Boolean
  }
`);
```

Move GraphQLResolver to `common-app/models`:

_./src/common-app/models/graphql.ts_

```javascript
import { GraphQLResolveInfo } from 'graphql';

// Pending to type Context
export type GraphQLResolver<Args, ReturnType> = (
  args: Args,
  context: unknown,
  info: GraphQLResolveInfo
) => Promise<ReturnType>;
```

Update barrel file:

_./src/common-app/models/index.ts_

```diff
export * from './user-session.js';
export * from './role.js';
+ export * from './graphql.js';

```

Update `book resolvers`:

_./src/pods/book/graphql/book.resolvers.ts_

```diff
- import { GraphQLResolveInfo } from 'graphql';
+ import { GraphQLResolver } from '#common-app/models/index.js';
import { logger } from '#core/logger/index.js';
...

- // TODO: Move to common-app/models/graphql.model.ts
- // Pending to add Context type
- type GraphQLResolver<Args, Context, ReturnType> = (
-   args: Args,
-   context: Context,
-   info: GraphQLResolveInfo
- ) => Promise<ReturnType>;

interface BookResolvers {
- books: GraphQLResolver<{ page?: number; pageSize?: number }, unknown, Book[]>;
+ books: GraphQLResolver<{ page?: number; pageSize?: number }, Book[]>;
- book: GraphQLResolver<{ id: string }, unknown, Book>;
+ book: GraphQLResolver<{ id: string }, Book>;
- saveBook: GraphQLResolver<{ book: Book }, unknown, Book>;
+ saveBook: GraphQLResolver<{ book: Book }, Book>;
- deleteBook: GraphQLResolver<{ id: string }, unknown, boolean>;
+ deleteBook: GraphQLResolver<{ id: string }, boolean>;
}
...

```

Add `security` resolvers:

_./src/pods/security/graphql/security.resolvers.ts_

```javascript
import { GraphQLResolver } from '#common-app/models/index.js';

interface SecurityResolvers {
  login: GraphQLResolver<{ email: string; password: string }, void>;
  logout: GraphQLResolver<never, void>;
}

export const securityResolvers: SecurityResolvers = {
  login: async ({ email, password }) => {
    console.log({ email, password });
  },
  logout: async () => {},
};
```

Add barrel file:

_./src/pods/security/graphql/index.ts_

```javascript
export * from './security.resolvers.js';
export * from './security.schema.js';
```

Update barrel file:

_./src/pods/security/index.ts_

```diff
export * from './security.rest-api.js';
export * from './security.middlewares.js';
+ export * from './graphql/index.js';

```

How can we merge multiple schemas? We can use [graphql-tools](https://github.com/ardatan/graphql-tools)

```bash
npm i @graphql-tools/schema --save-dev

```

Update app:

_./src/index.ts_

```diff
import '#core/load-env.js';
import '#core/monitoring.js';
import express from 'express';
import path from 'path';
import url from 'url';
+ import { mergeSchemas } from '@graphql-tools/schema';
...
import { booksApi, bookSchema, bookResolvers } from '#pods/book/index.js';
import {
  securityApi,
  authenticationMiddleware,
+ securitySchema,
+ securityResolvers,
} from '#pods/security/index.js';
import { userApi } from '#pods/user/index.js';

...
restApiServer.use('/', express.static(staticFilesPath));
createGraphqlServer(restApiServer, {
- schema: bookSchema,
+ schema: mergeSchemas({ schemas: [securitySchema, bookSchema] }),
- rootValue: bookResolvers,
+ rootValue: { ...securityResolvers, ...bookResolvers },
  formatError: (error) => {
    ...

```

Let's implement the `login` method:

_./src/pods/security/graphql/security.resolvers.ts_

```diff
+ import jwt from 'jsonwebtoken';
- import { GraphQLResolver } from '#common-app/models/index.js';
+ import { GraphQLResolver, UserSession } from '#common-app/models/index.js';
+ import { envConstants } from '#core/constants/index.js';
+ import { userRepository } from '#dals/index.js';

interface SecurityResolvers {
  login: GraphQLResolver<{ email: string; password: string }, unknown, void>;
  logout: GraphQLResolver<never, unknown, void>;
}

export const securityResolvers: SecurityResolvers = {
  login: async ({ email, password }) => {
-   console.log({ email, password });
+   const user = await userRepository.getUserByEmailAndPassword(
+     email,
+     password
+   );
+   if (user) {
+     const userSession: UserSession = {
+       id: user._id.toHexString(),
+       role: user.role,
+     };
+     const token = jwt.sign(userSession, envConstants.AUTH_SECRET, {
+       expiresIn: '1d',
+       algorithm: 'HS256',
+     });
+   }
  },
  logout: async () => {},
};

```

How could I get `res` object to set a cookie? We could use [GraphQL Context](https://github.com/graphql/graphql-http#context) for that:


_./src/common-app/models/graphql.ts_

```diff
import { GraphQLResolveInfo } from 'graphql';
+ import { Request as GraphQLRequest } from 'graphql-http';
+ import { RequestContext } from 'graphql-http/lib/use/express';
+ import { Request, Response } from 'express';

+ export interface GraphQLContext {
+   req: GraphQLRequest<Request, RequestContext>;
+   res: Response;
+ };

- // Pending to type Context
export type GraphQLResolver<Args, ReturnType> = (
  args: Args,
- context: unknown,
+ context: GraphQLContext,
  info: GraphQLResolveInfo
) => Promise<ReturnType>;

```

_./src/index.ts_

```diff
...
import { mergeSchemas } from '@graphql-tools/schema';
+ import { GraphQLContext } from '#common-app/models/index.js';
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from '#common/middlewares/index.js';
...

createGraphqlServer(restApiServer, {
  schema: mergeSchemas({ schemas: [securitySchema, bookSchema] }),
  rootValue: { ...securityResolvers, ...bookResolvers },
+ context: (req): any => {
+   const { res } = req.context;
+   return { req, res } as GraphQLContext;
+ },
...

```

Update security resolver:

_./src/pods/security/graphql/security.resolvers.ts_

```diff
import jwt from 'jsonwebtoken';
import { GraphQLResolver, UserSession } from '#common-app/models/index.js';
+ import { logger } from '#core/logger/index.js';
import { envConstants } from '#core/constants/index.js';
import { userRepository } from '#dals/index.js';

interface SecurityResolvers {
  login: GraphQLResolver<{ email: string; password: string }, void>;
  logout: GraphQLResolver<never, void>;
}

export const securityResolvers: SecurityResolvers = {
- login: async ({ email, password }) => {
+ login: async ({ email, password }, context) => {
    const user = await userRepository.getUserByEmailAndPassword(
      email,
      password
    );
    if (user) {
      const userSession: UserSession = {
        id: user._id.toHexString(),
        role: user.role,
      };
      const token = jwt.sign(userSession, envConstants.AUTH_SECRET, {
        expiresIn: '1d',
        algorithm: 'HS256',
      });
+     context.res.cookie('authorization', `Bearer ${token}`, {
+       httpOnly: true,
+       secure: envConstants.isProduction,
+     });
-   }
+   } else {
+     const message = `Invalid credentials for email ${email}`;
+     logger.warn(message);
+     throw new Error(JSON.stringify({ message, statusCode: 401 }));
+   }
  },
  logout: async () => {},
};

```


Configure playground to `include` credentials:

![01-configure-playground](./readme-resources/01-configure-playground.png)

Example invalid credentials query:

```graphql
mutation {
  login(email: "test@email.com", password: "test")
}
```

Example valid credentials query:

```graphql
mutation {
  login(email: "admin@email.com", password: "test")
}
```

![01-added-cookie](./readme-resources/01-added-cookie.png)

Implement `logout`:

_./src/pods/security/graphql/security.resolvers.ts_

```diff
...

- logout: async () => {
+ logout: async (_, content) => {
+   // NOTE: We cannot invalidate token using jwt libraries.
+   // Different approaches:
+   // - Short expiration times in token
+   // - Black list tokens on DB
+   content.res.clearCookie('authorization');
  },
```

How could we securize routes? We could use [GraphQL custom directives](https://the-guild.dev/graphql/tools/docs/schema-directives):

```bash
npm install @graphql-tools/utils --save
```

_./src/pods/security/graphql/security.schema.ts_

```diff
...
+ directive @isAuthenticated on FIELD_DEFINITION

  type Mutation {
    login(email: String!, password: String!): Boolean
-   logout: Boolean
+   logout: Boolean @isAuthenticated
  }
`;

```

> [GraphQL Directives](http://spec.graphql.org/June2018/#sec-Type-System.Directives)

Implement directive:

_./src/pods/security/graphql/security.directives.ts_

```javascript
import { mapSchema, MapperKind, getDirective } from '@graphql-tools/utils';
import { GraphQLSchema, defaultFieldResolver } from 'graphql';
import { verifyJWT } from '#common/helpers/index.js';
import { UserSession } from '#common-app/models/index.js';
import { envConstants } from '#core/constants/index.js';
import { GraphQLContext } from '#common-app/models/graphql.js';

const isAuthenticatedResolver = async (
  _,
  context: GraphQLContext,
  info,
  next: () => void
) => {
  try {
    const [, token] = context.req.raw.cookies.authorization?.split(' ') || [];
    const userSession = await verifyJWT<UserSession>(
      token,
      envConstants.AUTH_SECRET
    );
    context.userSession = userSession;
    return next();
  } catch (error) {
    const message = 'User is not authenticated';
    throw new Error(JSON.stringify({ message, statusCode: 401 }));
  }
};

export const securityDirectives = (schema: GraphQLSchema) =>
  mapSchema(schema, {
    [MapperKind.OBJECT_FIELD]: (fieldConfig) => {
      const directive = getDirective(schema, fieldConfig, 'isAuthenticated');
      if (directive) {
        const { resolve = defaultFieldResolver } = fieldConfig;
        return {
          ...fieldConfig,
          resolve: async (source, args, context, info) => {
            const next = () => resolve(source, args, context, info);
            return await isAuthenticatedResolver(args, context, info, next);
          },
        };
      }
    },
  });

```

_./src/pods/security/graphql/security.schema.ts_

```diff
...
+ import { securityDirectives } from './security.directives.js';

// TODO: Implement Void Custom Scalar Type

- export const securitySchema = graphql(`
+ export const securitySchema = securityDirectives(
+   graphql(`
  ...
- `);
+  `)
+);


```

Update barrel file:

_./src/pods/security/graphql/index.ts_

```diff
export * from './security.resolvers.js';
export * from './security.schema.js';
+ export * from './security.directives.js';

```

Update `context`:

_./src/common-app/models/graphql.ts_

```diff
...
import { Request, Response } from 'express';
+ import { UserSession } from './user-session.js';

export interface GraphQLContext {
  req: GraphQLRequest<Request, RequestContext>;
  res: Response;
+ userSession?: UserSession;
};

...

```

Check `logout` method with / without cookie.

Now we could authenticate books queries:

_./src/pods/book/graphql/book.schema.ts_

```diff
...
+ import { securityDirectives } from '#pods/security/graphql/index.js';

- export const bookSchema = graphql(`
+ export const bookSchema = securityDirectives(
+   graphql(`
      type Book {
        id: String!
        title: String!
        releaseDate: String!
        author: String!
      }

      type Query {
-       books(page: Int, pageSize: Int): [Book!]!
+       books(page: Int, pageSize: Int): [Book!]! @isAuthenticated
-       book(id: ID!): Book!
+       book(id: ID!): Book! @isAuthenticated
      }

      input BookInput {
        id: String
        title: String!
        releaseDate: String!
        author: String!
      }

      type Mutation {
-       saveBook(book: BookInput!): Book!
+       saveBook(book: BookInput!): Book! @isAuthenticated
-       deleteBook(id: ID!): Boolean!
+       deleteBook(id: ID!): Boolean! @isAuthenticated
      }
-   `);
+   `)
+ );

```

As exercise you could create the `authorize` directive to provide `allowedRoles`.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
