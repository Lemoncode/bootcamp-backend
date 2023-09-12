# 06 More queries

In this example we are going to implement complex queries in MongoDB.

We will start from `05-boilerplate`.

# Steps to build it

- `npm install` to install previous sample packages:

```bash
npm install

```

In this example, we will work with [Sample Mflix Dataset](https://github.com/Lemoncode/mongodb-sample-dbs-backup)

> [More info in MongoDB docs](https://docs.atlas.mongodb.com/sample-data/sample-mflix/#std-label-sample-mflix).

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

We will start with `movies` collection, we have defined the model in `./src/dals/movie/movie.model.ts`.

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
import { envConstants } from '#core/constants/index.js';
import {
  connectToDBServer,
  disconnectFromDBServer,
} from '#core/servers/index.js';
+ import { getMovieContext } from '#dals/movie/movie.context.js';

const runQueries = async () => {
+ const result = await getMovieContext()
+   .find({
+     runtime: { $lte: 15 },
+   })
+   .toArray();
+ console.log({ result });
};
...

```

Even, we could use `projections`:

_./src/console-runners/queries.runner.ts_

```diff
...

const runQueries = async () => {
  const result = await getMovieContext()
    .find({
      runtime: { $lte: 15 },
-   })
+     },
+     {
+       projection: {
+         _id: 1,
+         title: 1,
+         genres: 1,
+         imdb: 1,
+         'tomatoes.viewer.rating': 1,
+       },
+     }
+   )
    .toArray();
};
...

```

> \_id: this field is optionally
>
> title: this is a string field
>
> genres: this is an array field
>
> imdb: this will retrieve all fields inside imdb.
>
> 'tomatoes.viewer.rating': this will retrieve only the rating field inside.

We want to insert a new `genre` for a movie with `_id` equals `573a1390f29313caabcd4135` (the first movie with title `Blacksmith Scene`):

_./src/console-runners/queries.runner.ts_

```diff
+ import { ObjectId } from 'mongodb';
import { envConstants } from '#core/constants/index.js';
...

const runQueries = async () => {
  const result = await getMovieContext()
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
      {
+       returnDocument: 'after',
        projection: {
          _id: 1,
          title: 1,
          genres: 1,
          imdb: 1,
          'tomatoes.viewer.rating': 1,
        },
      }
-   )
-   .toArray();
+   );
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
  const result = await getMovieContext().findOneAndUpdate(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
+     genres: 'Drama',
    },
    {
-     $push: {
-       genres: 'Drama',
-     },
+     $set: {
+       'genres.$': 'Fantasy',
+     },
    },
    {
      returnDocument: 'after',
      projection: {
        _id: 1,
        title: 1,
        genres: 1,
        imdb: 1,
        'tomatoes.viewer.rating': 1,
      },
    }
  );
};

...

```

> [Reference .$](https://www.mongodb.com/docs/manual/reference/operator/update/positional/)
>
> This only update the first Match
>
> 'genres.$[]': for all items in array

> Update array in Mongo Compass like ['Short', 'Drama', 'Drama'].

To update all values that match with some condition, we have to use `$[<identifier>]` + `arrayFilters`:

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const result = await getMovieContext().findOneAndUpdate(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
-     genres: 'Drama',
    },
    {
      $set: {
-       'genres.$': 'Fantasy',
+       'genres.$[genre]': 'Thriller',
      },
    },
    {
+     arrayFilters: [{ genre: 'Fantasy' }],
      returnDocument: 'after',
      projection: {
        _id: 1,
        title: 1,
        genres: 1,
        imdb: 1,
        'tomatoes.viewer.rating': 1,
      },
    }
  );
};
...

```

> [Reference](https://docs.mongodb.com/v5.0/reference/operator/update/positional-filtered/#mongodb-update-up.---identifier--)

We want delete `Thriller` genre in this document:

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const result = await getMovieContext().findOneAndUpdate(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
    },
    {
-     $set: {
-       'genres.$[genre]': 'Thriller',
-     },
+     $pull: {
+       genres: 'Thriller',
+     },
    },
    {
-     arrayFilters: [{ genre: 'Fantasy' }],
      returnDocument: 'after',
      projection: {
        _id: 1,
        title: 1,
        genres: 1,
        imdb: 1,
        'tomatoes.viewer.rating': 1,
      },
    }
  );
};
...

```

> [Reference $pull](https://docs.mongodb.com/manual/reference/operator/update/pull/#mongodb-update-up.-pull)
>
> $pull vs [$pullAll](https://www.mongodb.com/docs/manual/reference/operator/update/pullAll/#mongodb-update-up.-pullAll): $pull remove all elements that match condition or value, $pullAll remove all elements that match the listed values.
>
> `{ $pullAll: { genres: [ 'Fantasy', 'Drama', 'Thriller' ] } }`

Try same example with `{ _id: new ObjectId(), name: 'Drama' }`:

```typescript
// Model
  genres?: { _id: ObjectId; name: string }[];

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
  },
  {
    $set: { 'genres.$[genre].name': 'Fantasy' },
  },
  {
    arrayFilters: [{ 'genre.name': 'Drama' }],
    ...
  }
```

```diff
// Delete

  {
    $pull: {
      genres: {
        name: 'Fantasy',
      },
    },
  },
  {
-   arrayFilters: [{ 'genre.name': 'Drama' }],
    ...
  }

> [Reference query array of documents](https://docs.mongodb.com/manual/tutorial/query-array-of-documents/)
>
> [Update arrayFilters with object](https://www.mongodb.com/docs/manual/reference/operator/update/positional-filtered/#update-all-array-elements-that-match-multiple-conditions) > [Array projections](https://docs.mongodb.com/manual/tutorial/project-fields-from-query-results/#project-specific-array-elements-in-the-returned-array)
>
> [Array query limitations](https://docs.mongodb.com/manual/reference/operator/projection/positional/#array-field-limitations)

## Queries over object subdocuments

Restore model:

_./src/dals/movie/movie.model.ts_

```diff
- genres?: { _id: ObjectId; name: string }[];
+ genres?: string[];

```

We want `delete` `imdb` review object in this document:

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const result = await getMovieContext().findOneAndUpdate(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
    },
    {
-     $pull: {
-       genres: {
-         name: 'Fantasy',
-       },
-     },
+     $unset: {
+       imdb: '',
+     },
    },
    {
      returnDocument: 'after',
      projection: {
        _id: 1,
        title: 1,
        genres: 1,
        imdb: 1,
        'tomatoes.viewer.rating': 1,
      },
    }
  );
};
...
```

> [Reference $unset](https://docs.mongodb.com/manual/reference/operator/update/unset/#mongodb-update-up.-unset)

We want `insert new` `imdb` review object in this document:

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const result = await getMovieContext().findOneAndUpdate(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
    },
    {
-     $unset: {
-       imdb: '',
-     },
+     $set: {
+       imdb: {
+         rating: 6.2,
+         votes: 1189,
+         id: 5,
+       },
+     },
    },
    {
      returnDocument: 'after',
      projection: {
        _id: 1,
        title: 1,
        genres: 1,
        imdb: 1,
        'tomatoes.viewer.rating': 1,
      },
    }
  );
};
...
```

> [Reference $set](https://docs.mongodb.com/manual/reference/operator/update/set/)
> Update full object it's the same code

Update only one field inside subdocument:

```diff
...

const runQueries = async () => {
  const result = await getMovieContext().findOneAndUpdate(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
    },
    {
      $set: {
-       imdb: {
-         rating: 6.2,
-         votes: 1189,
-         id: 5,
-       },
+       'imdb.rating': 8.2,
      },
    },
    {
      returnDocument: 'after',
      projection: {
        _id: 1,
        title: 1,
        genres: 1,
        imdb: 1,
        'tomatoes.viewer.rating': 1,
      },
    }
  );
};
...
```

## Queries over relationship documents

In this case, the insert, update and delete operations are straightforward because it's a simple
document without any subdocument. But if we try to get some movie's fields from comment documents,
we need `aggregations`.

First, we will retrieve a comment with simple fields for ObjectId equals `5a9427648b0beebeb69579e7` (it's the comment for the third movie):

_./src/console-runners/queries.runner.ts_

```diff
import { ObjectId } from 'mongodb';
import { envConstants } from '#core/constants/index.js';
import {
  connectToDBServer,
  disconnectFromDBServer,
} from '#core/servers/index.js';
- import { getMovieContext } from '#dals/movie/movie.context.js';
+ import { getCommentContext } from '#dals/comment/comment.context.js';

const runQueries = async () => {
- const result = await getMovieContext().findOneAndUpdate(
+ const result = await getCommentContext().findOne(
    {
-     _id: new ObjectId('573a1390f29313caabcd4135'),
+     _id: new ObjectId('5a9427648b0beebeb69579e7'),
    },
-   {
-     $set: {
-       'imdb.rating': 8.2,
-     },
-   },
    {
-     returnDocument: 'after',
      projection: {
        _id: 1,
-       title: 1,
-       genres: 1,
-       imdb: 1,
-       'tomatoes.viewer.rating': 1,
+       name: 1,
+       email: 1,
+       movie_id: 1,
+       text: 1,
+       date: 1,
      },
    }
  );
};

...

```

> [Mongoose populate](https://mongoosejs.com/docs/api.html#query_Query-populate): it's not recommended because it could impact in performance.

We will use `aggregations` for include necessary fields:

_./src/console-runners/queries.runner.ts_

```diff
...

const runQueries = async () => {
- const result = await getCommentContext().findOne(
-   {
-     _id: new ObjectId('5a9427648b0beebeb69579e7'),
-   },
-   {
-     projection: {
-       _id: 1,
-       name: 1,
-       email: 1,
-       movie_id: 1,
-       text: 1,
-       date: 1,
-     },
-   }
- );

+ const result = await getCommentContext()
+   .aggregate([
+     {
+       $match: {
+         _id: new ObjectId('5a9427648b0beebeb69579e7'),
+       },
+     },
+     {
+       $lookup: {
+         from: 'movies',
+         localField: 'movie_id',
+         foreignField: '_id',
+         as: 'movies',
+       },
+     },
+     {
+       $project: {
+         _id: 1,
+         name: 1,
+         email: 1,
+         movie_id: 1,
+         movies: 1,
+         text: 1,
+         date: 1,
+       },
+     },
+   ])
+   .toArray();
};

...

```

> NOTE: Aggregate method always returns an array.
> [Reference aggregations](https://docs.mongodb.com/manual/reference/operator/aggregation-pipeline/)

If we want fill the `movie` field instead of `movies` list, we could use [$addFields](https://docs.mongodb.com/manual/reference/operator/aggregation/addFields/) and [$arrayElemAt](https://docs.mongodb.com/manual/reference/operator/aggregation/arrayElemAt/):

_./src/console-runners/queries.runner.ts_

```diff
...
const runQueries = async () => {
  const result = await getCommentContext()
    .aggregate([
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
+     {
+       $addFields: {
+         movie: { $arrayElemAt: ['$movies', 0] },
+       },
+     },
      {
        $project: {
          _id: 1,
          name: 1,
          email: 1,
          movie_id: 1,
-         movies: 1,
+         movie: 1,
          text: 1,
          date: 1,
        },
      },
    ])
    .toArray();
};

...

```

In this case, we could use too [$unwind](https://docs.mongodb.com/manual/reference/operator/aggregation/unwind/):

_./src/console-runners/queries.runner.ts_

```diff
...

const runQueries = async () => {
  const result = await getCommentContext()
    .aggregate([
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
-         as: 'movies',
+         as: 'movie',
        },
      },
-     {
-       $addFields: {
-         movie: { $arrayElemAt: ['$movies', 0] },
-       },
-     },
+     {
+       $unwind: '$movie'
+     },
      {
        $project: {
          _id: 1,
          name: 1,
          email: 1,
          movie_id: 1,
          movie: 1,
          text: 1,
          date: 1,
        },
      },
    ])
    .toArray();
};

...
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
