# 03 Mongo deploy

In this example we are going to learn how to deploy MongoDB.

We will start from `02-manual-render-deploy`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
cd front
npm install

```

In a second terminal:

```bash
cd back
npm install

```

Once we have the app deployed in Heroku in API mock mode and it's working, we will deploy the MongoDB in this case in [MongoDB Atlas](https://www.mongodb.com/cloud/atlas) the official cloud site.

We could start at free cluster:

![01-create-new-cluster](./readme-resources/01-create-new-cluster.png)

![02-start-free-cluster](./readme-resources/02-start-free-cluster.png)

We could select between three providers and different regions:

![03.1-select-provider](./readme-resources/03.1-select-provider.png)

![03.2-select-region](./readme-resources/03.2-select-region.png)

Select the cluster tier, in this case `M0 Sandbox` which it's a free tier with No backup:

![04-select-cluster-tier](./readme-resources/04-select-cluster-tier.png)

Finally, give a name (if you want) and create the cluster:

![05-create-cluster](./readme-resources/05-create-cluster.png)

After create the cluster, we will see the quick start guide but we will navigate to clusters pages:

![06-main-cluster-page](./readme-resources/06-main-cluster-page.png)

By default, MongoDB Atlas only allows access to configured IPs, let's add a new rule to allow all IPs:

![07-configure-network-access](./readme-resources/07-configure-network-access.png)

Let's configure database access, adding new user:

![08.1-configure-database-access](./readme-resources/08.1-configure-database-access.png)

![08.2-configure-database-access](./readme-resources/08.2-configure-database-access.png)

> Let's copy the autogenerated password. We will use in the MongoDB Connection URI
>
> Add DB user privileges using specific privileges.

Let's copy the `MongoDB Connection URI`:

![09-click-connect-button](./readme-resources/09-click-connect-button.png)

![10-copy-connection-uri](./readme-resources/10-copy-connection-uri.png)

Update env variable:

_./back/.env_

```diff
...
IS_API_MOCK=true
- MONGODB_URL=mongodb://localhost:27017/book-store
+ MONGODB_URL=mongodb+srv://<user>:<password>@<cluster>.mongodb.net/book-store?retryWrites=true&w=majority
AUTH_SECRET=MY_AUTH_SECRET
...

```

> Replace <user>, <password> and <cluster> with MongoDB Atlas provided values.
>
> Use `book-store` database name.

Now, we will insert users's documents in `production` database:

_./back/src/console-runners/seed-data.runner.ts_

```diff
import { hash } from '#common/helpers/index.js';
import { getUserContext } from '#dals/user/user.context.js';
- import { getBookContext } from '#dals/book/book.context.js';
import { db } from '#dals/mock-data.js';

export const run = async () => {
  for (const user of db.users) {
    const hashedPassword = await hash(user.password);

    await getUserContext().insertOne({
      ...user,
      password: hashedPassword,
    });
  }
- await getBookContext().insertMany(db.books);
};

```

Run `console-runner`:

_back terminal_

```bash
npm run start:console-runners
```

Check result in MongoDB Atlas:

![11-click-collections-button](./readme-resources/11-click-collections-button.png)

![12-mongo-collections](./readme-resources/12-mongo-collections.png)

Restore local env variable:

_./back/.env_

```diff
...
IS_API_MOCK=true
- MONGODB_URL=mongodb+srv://<user>:<password>@<cluster>.mongodb.net/book-store?retryWrites=true&w=majority
+ MONGODB_URL=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET
...

```

Finally, we will update _Render_ env variables using `production MONGODB_URL` value:

![13-update-render-env-variables](./readme-resources/13-update-render-env-variables.png)

Since we didn't insert any book, we could use deployed app, to insert one and see the result in MongoDB Atlas:

![14-insert-book-from-deployed-app](./readme-resources/14-insert-book-from-deployed-app.png)

![15-check-documents](./readme-resources/15-check-documents.png)

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
