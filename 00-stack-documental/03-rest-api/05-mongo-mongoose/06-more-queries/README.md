# 06 More queries

In this example we are going to implement complex queries in MongoDB / Mongoose.

We will start from `05-boilerplate`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

In this example, we will work with [Sample Mflix Dataset](https://docs.atlas.mongodb.com/sample-data/sample-mflix/#std-label-sample-mflix) from MongoDB docs.
But in this case, we will load it using the `seed-data` console runner:

```bash
npm run start:console-runners

> seed-data
>> Seed data path: ...
>> Docker container name: mflix-db
>> Database name: mflix
```

> Seed data path: path in your files system
> Docker container name: you can see this name in `docker-compose.yml` file.
> Database name: you can see this name in `.env.example` or `.env` files.

We could check this data for example with `Mongo Compass`.

We will start with `movies` collection, we have defined the model in `./src/dals/movie/movie.model.ts`
and we will implement the `movieContext`:

_./src/dals/movie/movie.context.ts_

```diff
import mongoose, { Schema, SchemaDefinition } from "mongoose";
- import { Movie } from "./movie.model";
+ import { Movie, IMDB, Tomatoes, TomatoesViewer } from "./movie.model";

+ const imdbSchema = new Schema({
+   rating: { type: Schema.Types.Number, required: true },
+   votes: { type: Schema.Types.Number, required: true },
+   id: { type: Schema.Types.Number, required: true },
+ } as SchemaDefinition<IMDB>);

+ const tomatoesSchema = new Schema({
+   viewer: {
+     type: new Schema({
+       rating: { type: Schema.Types.Number, required: true },
+       numReviews: { type: Schema.Types.Number, required: true },
+     } as SchemaDefinition<TomatoesViewer>),
+     required: true,
+   },
+   lastUpdated: { type: Schema.Types.Date, required: true },
+ } as SchemaDefinition<Tomatoes>);

const movieSchema = new Schema({
+ title: { type: Schema.Types.String, required: true },
+ year: { type: Schema.Types.Number, required: true },
+ runtime: { type: Schema.Types.Number, required: true },
+ released: { type: Schema.Types.Date, required: true },
+ poster: { type: Schema.Types.String },
+ plot: { type: Schema.Types.String, required: true },
+ fullplot: { type: Schema.Types.String, required: true },
+ lastupdated: { type: Schema.Types.Date, required: true },
+ type: { type: Schema.Types.String, required: true },
+ directors: [{ type: Schema.Types.String }],
+ countries: [{ type: Schema.Types.String }],
+ genres: [{ type: Schema.Types.String }],
+ cast: [{ type: Schema.Types.String }],
+ num_mflix_comments: { type: Schema.Types.Number, required: true },
+ imdb: { type: imdbSchema },
+ tomatoes: { type: tomatoesSchema },
} as SchemaDefinition<Movie>);

export const movieContext = mongoose.model<Movie>("Movie", movieSchema);

```

> NOTE: Mongoose has the `Schema.Types.Mixed` type too.

Let's start `queries` console runner:

```bash
npm run start:console-runners

> queries

```

> Execute in JavaScript Debug Terminal

We could run same queries that we did in mongo console:

_./src/console-runners/queries.runner.ts_

```diff
import { disconnect } from "mongoose";
import { envConstants } from "core/constants";
import { connectToDBServer } from "core/servers";
+ import { movieContext } from "dals/movie/movie.context";

const runQueries = async () => {
+ const result = await movieContext
+   .find({
+     runtime: { $lte: 15 },
+   })
+   .lean();
};
...

```

Even, we could use `projections`:

_./src/console-runners/queries.runner.ts_

```diff
...

const runQueries = async () => {
  const result = await movieContext
    .find(
      {
        runtime: { $lte: 15 },
      },
+     {
+       _id: 1,
+       title: 1,
+       directors: 1,
+     },
    )
    .lean();
};
...

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
