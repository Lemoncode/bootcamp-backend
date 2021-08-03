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

Let's try complex queries related with insert/update/get subdocuments, arrays and relationships in external collections.

## Queries over arrays subdocuments

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
+       genres: 1,
+       imdb: 1,
+       'tomatoes.viewer.rating': 1,
+     },
    )
    .lean();
};
...

```

> _id: this field is optionally
>
> title: this is a string field
>
> genres: this is an array field
>
> imdb: this will retrieve all fields inside imdb.
>
> 'tomatoes.viewer.rating': this will retrieve only the rating field inside.

We want to insert a new `genre` for a movie with `_id` equals `573a1390f29313caabcd4135`:

_./src/console-runners/queries.runner.ts_

```diff
import { disconnect } from 'mongoose';
+ import { ObjectId } from 'mongodb';

...

const runQueries = async () => {
  const result = await movieContext
-   .find(
+   .findOneAndUpdate(
      {
-       runtime: { $lte: 15 },
+       _id: new ObjectId('573a1390f29313caabcd4135'),
      },
+     {
+       $push: {
+         genres: 'Drama',
+       },
+     },
+     {
+       new: true,
+       projection: {
          _id: 1,
          title: 1,
          genres: 1,
          imdb: 1,
          'tomatoes.viewer.rating': 1,
+       },
+     }
    )
    .lean();
};
...

```

> [Reference $push](https://docs.mongodb.com/manual/reference/operator/update/push/#mongodb-update-up.-push)
>
> NOTE: `updateOne` vs `findOneAndUpdate`
>
> Same behaviour but `updateOne` returns the object with number of updated documents, if ok, etc.
> `findOneAndUpdate` returns the object after updated it.

We want rename `Drama` genre in this document by `Fantasy`:

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const result = await movieContext
    .findOneAndUpdate(
      {
        _id: new ObjectId('573a1390f29313caabcd4135'),
+       genres: 'Drama',
      },
      {
-       $push: {
+       $set: {
-         genres: 'Drama',
+         'genres.$': 'Fantasy',
        },
      },
...

```

> [Reference .$](https://docs.mongodb.com/manual/reference/operator/update/positional/#---update-)
>
> This only update the first Match
>
> 'genres.$[]': for all items in array without any condition

To update all values that match with some condition, we have to use `$[<identifier>]` + `arrayFIlters`:

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const result = await movieContext
    .findOneAndUpdate(
      {
        _id: new ObjectId('573a1390f29313caabcd4135'),
-       genres: 'Drama',
      },
      {
        $set: {
-         'genres.$': 'Fantasy',
+         'genres.$[drama]': 'Fantasy',
        },
      },
      {
+       arrayFilters: [{ drama: 'Drama' }],
        new: true,
        projection: {
          _id: 1,
          title: 1,
          genres: 1,
          imdb: 1,
          'tomatoes.viewer.rating': 1,
        },
      }
...

```

> Update array in Mongo Compass like ['Short', 'Drama', 'Drama'].
>
> [Reference](https://docs.mongodb.com/v5.0/reference/operator/update/positional-filtered/#mongodb-update-up.---identifier--)

We want delete `Fantasy` genre in this document:

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const result = await movieContext
    .findOneAndUpdate(
      {
        _id: new ObjectId('573a1390f29313caabcd4135'),
-       genres: 'Drama',
      },
      {
-       $set: {
+       $pull: {
-         'genres.$': 'Fantasy',
+         genres: 'Fantasy',
        },
      },
      {
-       arrayFilters: [{ drama: 'Drama' }],
        new: true,
        projection: {
          _id: 1,
          title: 1,
          genres: 1,
          imdb: 1,
          'tomatoes.viewer.rating': 1,
        },
      }
...

```

> [Reference $pull](https://docs.mongodb.com/manual/reference/operator/update/pull/#mongodb-update-up.-pull)


Try same example with `{ _id: new ObjectId(), name: 'Drama' }`:

```typescript
// Context
genres: [{ type: Schema.Types.Mixed }],

// Insert
  {
    $push: {
      genres: {
        _id: new ObjectId(),
        name: 'Drama',
      },
    },
  },

// Update
  {
    _id: new ObjectId('573a1390f29313caabcd4135'),
    'genres._id': new ObjectId('...'),
  },
  {
    $set: {
      'genres.$.name': 'Fantasy',
    },
  },

// Delete

  {
    $pull: {
      genres: {
        _id: new ObjectId('...'),
      },
    },
  },
```

> [Reference query array of documents](https://docs.mongodb.com/manual/tutorial/query-array-of-documents/)
> [Array projections](https://docs.mongodb.com/manual/tutorial/project-fields-from-query-results/#project-specific-array-elements-in-the-returned-array)
> [Array query limitations](https://docs.mongodb.com/manual/reference/operator/projection/positional/#array-field-limitations)

We could use projections like:

```diff
  {
    new: true,
    projection: {
      _id: 1,
      title: 1,
-     genres: 1,
+     'genres.name': 1,
      imdb: 1,
      'tomatoes.viewer.rating': 1,
    },
  }
```

> Advanced scenario: we cannot combine both `$elemMatch` and specific fields together since MongoDB 4.4

```diff
 // Get only updated genre
  {
    new: true,
    projection: {
      _id: 1,
      title: 1,
+     genres: {
+       $elemMatch: {
+         _id: new ObjectId('...'),
+       },
+     },
      imdb: 1,
      'tomatoes.viewer.rating': 1,
    },
  }
```
> [Projection restrictions](https://docs.mongodb.com/manual/reference/limits/#mongodb-limit-Projection-Restrictions)

## Queries over object subdocuments

Restore context:

_./src/dals/movie/movie.context.ts_

```diff
- genres: [{ type: Schema.Types.Mixed }],
+ genres: [{ type: Schema.Types.String }],

```

We want `delete` `imdb` review object in this document:

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const result = await movieContext
    .findOneAndUpdate(
      {
        _id: new ObjectId('573a1390f29313caabcd4135'),
      },
      {
-       $pull: {
+       $unset: {
-         genres: 'Fantasy',
+         imdb: '',
        },
      },
      {
        new: true,
        projection: {
          _id: 1,
          title: 1,
          genres: 1,
          imdb: 1,
          'tomatoes.viewer.rating': 1,
        },
      }
...
```

> [Reference $unset](https://docs.mongodb.com/manual/reference/operator/update/unset/#mongodb-update-up.-unset)

We want `insert new` `imdb` review object in this document:

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const result = await movieContext
    .findOneAndUpdate(
      {
        _id: new ObjectId('573a1390f29313caabcd4135'),
      },
      {
-       $unset: {
+       $set: {
-         imdb: '',
+         imdb: {
+           rating: 6.2,
+           votes: 1189,
+           id: 5,
+         },
        },
      },
...
```

> [Reference $set](https://docs.mongodb.com/manual/reference/operator/update/set/)
> Update full object it's the same code

Update only one field inside subdocument:

```diff
...
const runQueries = async () => {
  const result = await movieContext
    .findOneAndUpdate(
      {
        _id: new ObjectId('573a1390f29313caabcd4135'),
      },
      {
        $set: {
-         imdb: {
-           rating: 6.2,
-           votes: 1189,
-           id: 5,
-         },
+         'imdb.rating': 8.2,
        },
      },
...
```

## Queries over relationship documents

First, we will implement the `commentSchema`:

_./src/dals/comment/comment.context.ts_

```diff
import mongoose, { Schema, SchemaDefinition } from 'mongoose';
import { Comment } from './comment.model';

const commentSchema = new Schema({
+ name: { type: Schema.Types.String, required: true },
+ email: { type: Schema.Types.String, required: true },
+ movie_id: { type: Schema.Types.ObjectId, required: true },
+ text: { type: Schema.Types.String, required: true },
+ date: { type: Schema.Types.Date, required: true },
} as SchemaDefinition<Comment>);

export const commentContext = mongoose.model<Comment>('Comment', commentSchema);

```

In this case, the insert, update and delete operations are straightforward because it's a simple
document without any subdocument. But if we try to get some movie's fields from comment documents,
we need `aggregations`.

First, we will retrieve a comment with simple fields for ObjectId equals `5a9427648b0beebeb69579e7`:

_./src/console-runners/queries.runner.ts_

```diff
import { disconnect } from 'mongoose';
import { ObjectId } from 'mongodb';
import { envConstants } from 'core/constants';
import { connectToDBServer } from 'core/servers';
- import { movieContext } from 'dals/movie/movie.context';
+ import { commentContext } from 'dals/comment/comment.context';

const runQueries = async () => {
- const result = await movieContext
+ const result = await commentContext
-   .findOneAndUpdate(
+   .findOne(
      {
-       _id: new ObjectId('573a1390f29313caabcd4135'),
+       _id: new ObjectId('5a9427648b0beebeb69579e7'),
      },
-     {
-       $set: {
-         'imdb.rating': 8
-       },
-     },
+     {
+       _id: 1,
+       name: 1,
+       email: 1,
+       movie_id: 1,
+       text: 1,
+       date: 1,
+     }
-     {
-       new: true,
-       projection: {
-         _id: 1,
-         title: 1,
-         genres: 1,
-         imdb: 1,
-         'tomatoes.viewer.rating': 1,
-       },
-     }
    )
    .lean();
};
...
```

> [Mongoose populate](https://mongoosejs.com/docs/api.html#query_Query-populate): it's not recommended becuase it could impact in performance.

If we want get the `movie's title` when we retrieve the comment, we could model it like:

_./src/dals/comment/comment.model.ts_

```diff
import { ObjectId } from 'mongodb';
+ import { Movie } from 'dals/movie';

// https://docs.atlas.mongodb.com/sample-data/sample-mflix/#sample_mflix.comments

export interface Comment {
  _id: ObjectId;
  name: string;
  email: string;
  movie_id: ObjectId;
+ movie?: Movie;
  text: string;
  date: Date;
}

```

And we could use `aggregations` for include necessary fields:

_./src/console-runners/queries.runner.ts_

```diff
...

const runQueries = async () => {
- const result = await commentContext
-   .findOne(
-     {
-       _id: new ObjectId('5a9427648b0beebeb69579e7'),
-     },
-     {
-       _id: 1,
-       name: 1,
-       email: 1,
-       movie_id: 1,
-       text: 1,
-       date: 1,
-     }
-   )
-   .lean();
+ const [comment] = await commentContext.aggregate([
+   {
+     $match: {
+       _id: new ObjectId('5a9427648b0beebeb69579e7'),
+     },
+   },
+   {
+     $lookup: {
+       from: 'movies',
+       localField: 'movie_id',
+       foreignField: '_id',
+       as: 'movies',
+     },
+   },
+   {
+     $project: {
+       _id: 1,
+       name: 1,
+       email: 1,
+       movie_id: 1,
+       text: 1,
+       date: 1,
+       movies: 1,
+     },
+   },
+ ]);
};

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);
  await runQueries();
  await disconnect();
};

```

> NOTE: Aggregate method always returns an array.
> [Reference aggregations](https://docs.mongodb.com/manual/reference/operator/aggregation-pipeline/)

If we want fill the `movie` field instead of `movies` list, we could use [$addFields](https://docs.mongodb.com/manual/reference/operator/aggregation/addFields/) and [$arrayElemAt](https://docs.mongodb.com/manual/reference/operator/aggregation/arrayElemAt/):

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const [comment] = await commentContext.aggregate([
    {
      $match: {
        _id: new ObjectId('5a9427648b0beebeb69579e7'),
      },
    },
    {
      $lookup: {
        from: 'movies',
        localField: 'movie_id',
        foreignField: '_id',
        as: 'movies',
      },
    },
+   {
+     $addFields: {
+       movie: { $arrayElemAt: ['$movies', 0] },
+     },
+   },
    {
      $project: {
        _id: 1,
        name: 1,
        email: 1,
        movie_id: 1,
        text: 1,
        date: 1,
-       movies: 1,
+       'movie.title': 1,
      },
    },
  ]);
};
...

```

In this case, we could use too [$unwind](https://docs.mongodb.com/manual/reference/operator/aggregation/unwind/):

_./src/console-runners/queries.runner.ts_

```diff
...

const runQueries = async () => {
  const [comment] = await commentContext.aggregate([
    {
      $match: {
        _id: new ObjectId('5a9427648b0beebeb69579e7'),
      },
    },
    {
      $lookup: {
        from: 'movies',
        localField: 'movie_id',
        foreignField: '_id',
-       as: 'movies',
+       as: 'movie',
      },
    },
+   { $unwind: '$movie' },
-   {
-     $addFields: {
-       movie: { $arrayElemAt: ['$movies', 0] },
-     },
-   },
    {
      $project: {
        _id: 1,
        name: 1,
        email: 1,
        movie_id: 1,
        'movie.title': 1,
        text: 1,
        date: 1,
      },
    },
  ]);
};

...
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
