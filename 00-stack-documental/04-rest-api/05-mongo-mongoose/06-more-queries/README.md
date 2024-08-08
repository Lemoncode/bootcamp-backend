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
>
> Docker container name: you can see this name in `docker-compose.yml` file.
>
> Database name: you can see this name in `.env.example` or `.env` files.

We could check this data for example with `Mongo Compass`.

We will start with `movies` collection, we have defined the model in `./src/dals/movie/movie.model.ts`.

Let's start `queries` console runner:

```bash
npm run start:console-runners

> queries

```

> Execute in JavaScript Debug Terminal

## Simple queries

First, let's start with a simple query, same that we did in the mongo module:

_./src/console-runners/queries.runner.ts_

```diff
+ import { getMovieContext } from '#dals/movie/movie.context.js';

export const run = async () => {
+ const result = await getMovieContext()
+   .find({
+     runtime: { $lte: 15 },
+   })
+   .toArray();
+ console.log({ result });
};

```

Even, we could use `projections`:

_./src/console-runners/queries.runner.ts_

```diff
...

const run = async () => {
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

## Queries over arrays subdocuments

Let's try complex queries related with insert/update/get subdocuments, arrays and relationships with external collections.

### Supporting genres model to accept string array and object array

This time, we will refactor the `genres` field to support both types: string array and object array:

_./src/dals/movie/movie.model.ts_

```diff
...

+ export interface Genre {
+   id: ObjectId;
+   name: string;
+ }

export interface Movie {
- genres?: string[];
+ genres?: string[] | Genre[];
  cast?: string[];
  ...
}
```

### Insert object element in array subdocument

We want to insert a new `genre` for a movie with `_id` equals `573a1390f29313caabcd4135` (the first movie with title `Blacksmith Scene`):

_./src/console-runners/queries.runner.ts_

```diff
+ import { ObjectId } from 'mongodb';
...

const run = async () => {
  const result = await getMovieContext()
-   .find(
+   .findOneAndUpdate(
      {
-       runtime: { $lte: 15 },
+       _id: new ObjectId('573a1390f29313caabcd4135'),
      },
+     {
+       $push: {
+         genres: {
+           id: new ObjectId(),
+           name: 'Drama',
+         },
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
>
> `findOneAndUpdate` returns the object after updated it.

### Update object element in array subdocument

We want rename `Drama` genre in this document by `Fantasy`:

_./src/console-runners/queries.runner.ts_

```diff
...
const run = async () => {
  const result = await getMovieContext().findOneAndUpdate(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
    },
    {
-     $push: {
-       genres: {
-         id: new ObjectId(),
-         name: 'Drama',
-       },
-     },
+     $set: {
+       'genres.$[genre].name': 'Fantasy',
+     },
    },
    {
+     arrayFilters: [{ 'genre.name': 'Drama' }],
      returnDocument: 'after',
...

```

> [.$[<identifier>] + arrayFilters Reference](https://docs.mongodb.com/v5.0/reference/operator/update/positional-filtered/#mongodb-update-up.---identifier--)
>
> 'genres.$[]': for all items in array

### Delete object element in array subdocument

We want delete `Fantasy` genre in this document:

_./src/console-runners/queries.runner.ts_

```diff
...
const run = async () => {
  const result = await getMovieContext().findOneAndUpdate(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
    },
    {
-     $set: {
-       'genres.$[genre].name': 'Fantasy',
-     },
+     $pull: {
+       genres: {
+         name: 'Fantasy',
+       },
+     },
    },
    {
-     arrayFilters: [{ 'genre.name': 'Drama' }],
      returnDocument: 'after',
...

```

> NOTE: You cannot use `'genres.name': 'Fantasy'` because it will throw an error ( Cannot use the part (name) of (genres.name) to traverse the element).
>
> [Reference $pull](https://docs.mongodb.com/manual/reference/operator/update/pull/#mongodb-update-up.-pull)
>
> $pull vs [$pullAll](https://www.mongodb.com/docs/manual/reference/operator/update/pullAll/#mongodb-update-up.-pullAll): $pull remove all elements that match condition or value, $pullAll remove all elements that match the listed values.
>
> `{ $pullAll: { genres: [ 'Fantasy', 'Drama', 'Thriller' ] } }`
>
> [Reference query array of documents](https://docs.mongodb.com/manual/tutorial/query-array-of-documents/)
>
> [Update arrayFilters with object](https://www.mongodb.com/docs/manual/reference/operator/update/positional-filtered/#update-all-array-elements-that-match-multiple-conditions) > [Array projections](https://docs.mongodb.com/manual/tutorial/project-fields-from-query-results/#project-specific-array-elements-in-the-returned-array)
>
> [Array query limitations](https://docs.mongodb.com/manual/reference/operator/projection/positional/#array-field-limitations)

### Get object element from array subdocument

In the scenario we want to retrieve the desired `genre` from `movies.genres`.

First, we will add a few genres to the movie (in this case we are replacing all genres, that is, we are removing the `Short` genre string format):

_./src/console-runners/queries.runner.ts_

```diff
...
    {
-     $pull: {
-       genres: {
-         name: 'Fantasy',
-       },
-     },
+     $set: {
+       genres: [
+         { id: new ObjectId(), name: 'Short' },
+         { id: new ObjectId(), name: 'Drama' },
+         { id: new ObjectId(), name: 'Fantasy' },
+         { id: new ObjectId(), name: 'Thriller' },
+       ],
+     },
    },
...
```

Now, we will retrieve the desired genre, for example `Fantasy`. A common mistake is to make a query like:

_./src/console-runners/queries.runner.ts_

```diff
...
- const result = await getMovieContext().findOneAndUpdate(
+ const result = await getMovieContext().findOne(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
+     'genres.name': 'Fantasy',
    },
-   {
-     $set: {
-       genres: [
-         { id: new ObjectId(), name: 'Short' },
-         { id: new ObjectId(), name: 'Drama' },
-         { id: new ObjectId(), name: 'Fantasy' },
-         { id: new ObjectId(), name: 'Thriller' },
-       ],
-     },
-   },
    {
-     returnDocument: 'after',
      projection: {
        _id: 1,
-       title: 1,
        genres: 1,
-       imdb: 1,
-       'tomatoes.viewer.rating': 1,
      },
    }
  );
```

In the previous query, we are filtering `movies` that match with `_id` and it has a `genres.name` equals `Fantasy`, but we are not filtering the `genres` array. To filter the `genres` array, we need to use the [$ operator](https://www.mongodb.com/docs/manual/reference/operator/projection/positional/) in projection:

_./src/console-runners/queries.runner.ts_

```diff
...
    {
      projection: {
        _id: 1,
-       genres: 1,
+       'genres.$': 1,
      },
    }
...

```

> Use $ in the projection document of the find() method or the findOne() method when you only need one particular array element in selected documents.

But this is only valid for the first level, if we want to filter in a nested array, we need to use [aggregations](https://docs.mongodb.com/manual/reference/operator/aggregation/).

For example, we can update the `directors` model:

_./src/dals/movie/movie.model.ts_

```diff
...
export interface Genre {
  id: ObjectId;
  name: string;
}

+ export interface Award {
+   id: ObjectId;
+   name: string;
+   year: number;
+ }
+
+ export interface Director {
+   id: ObjectId;
+   name: string;
+   awards: Award[];
+ }

export interface Movie {
...
- directors?: string[];
+ directors?: string[] | Director[];
...
```

And we will insert a new `directors` with `awards`:

_./src/console-runners/queries.runner.ts_

```diff
...
export const run = async () => {
- const result = await getMovieContext().findOne(
+ const result = await getMovieContext().findOneAndUpdate(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
-     'genres.name': 'Fantasy',
    },
+   {
+     $set: {
+       directors: [
+         {
+           id: new ObjectId(),
+           name: 'Jane Doe',
+           awards: [
+             {
+               id: new ObjectId(),
+               name: 'Academy Awards',
+               year: 2020,
+             },
+             {
+               id: new ObjectId(),
+               name: 'Golden Globe Awards',
+               year: 2021,
+             },
+           ],
+         },
+         {
+           id: new ObjectId(),
+           name: 'John Doe',
+           awards: [
+             {
+               id: new ObjectId(),
+               name: 'Academy Awards',
+               year: 2020,
+             },
+             {
+               id: new ObjectId(),
+               name: 'Golden Globe Awards',
+               year: 2021,
+             },
+           ],
+         },
+       ],
+     },
+   },
    {
+     returnDocument: 'after',
      projection: {
        _id: 1,
-       'genres.$': 1,
+       directors: 1,
      },
    }
  );
  console.log({ result });
};

```

We will retrieve the desired `director` with `awards`:

_./src/console-runners/queries.runner.ts_

```diff
...
export const run = async () => {
- const result = await getMovieContext().findOneAndUpdate(
+ const result = await getMovieContext().findOne(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
+     'directors.name': 'Jane Doe',
    },
-   {
-     $set: {
-       directors: [
-         {
-           id: new ObjectId(),
-           name: 'Jane Doe',
-           awards: [
-             {
-               id: new ObjectId(),
-               name: 'Academy Awards',
-               year: 2020,
-             },
-             {
-               id: new ObjectId(),
-               name: 'Golden Globe Awards',
-               year: 2021,
-             },
-           ],
-         },
-         {
-           id: new ObjectId(),
-           name: 'John Doe',
-           awards: [
-             {
-               id: new ObjectId(),
-               name: 'Academy Awards',
-               year: 2020,
-             },
-             {
-               id: new ObjectId(),
-               name: 'Golden Globe Awards',
-               year: 2021,
-             },
-           ],
-         },
-       ],
-     },
-   },
    {
-     returnDocument: 'after',
      projection: {
        _id: 1,
-       directors: 1,
+       'directors.$': 1,
      },
    }
  );
  console.log({ result });
};

```

But if we want to filter by `awards` and get only the `Golden Globe Awards`:

_./src/console-runners/queries.runner.ts_

```diff
...
export const run = async () => {
  const result = await getMovieContext().findOne(
    {
      _id: new ObjectId('573a1390f29313caabcd4135'),
      'directors.name': 'Jane Doe',
+     'directors.awards.name': 'Golden Globe Awards',
    },
    {
      projection: {
        _id: 1,
-       'directors.$': 1,
+       'directors.awards.$': 1,
      },
    }
  );
  console.log({ result });
};

```

The previous operation is over `directors` array, but if we want to filter over `awards` array, we can do it in a simple way with [aggregations - $unwind](https://docs.mongodb.com/manual/reference/operator/aggregation/unwind/):

_./src/console-runners/queries.runner.ts_

```diff
...

export const run = async () => {
- const result = await getMovieContext().findOne(
-   {
-     _id: new ObjectId('573a1390f29313caabcd4135'),
-     'directors.name': 'Jane Doe',
-     'directors.awards.name': 'Golden Globe Awards',
-   },
-   {
-     projection: {
-       _id: 1,
-       'directors.awards.$': 1,
-     },
-   }
- );
+ const results = await getMovieContext()
+   .aggregate([
+     {
+       $unwind: '$directors',
+     },
+     {
+       $unwind: '$directors.awards',
+     },
+     {
+       $match: {
+         _id: new ObjectId('573a1390f29313caabcd4135'),
+         'directors.name': 'Jane Doe',
+         'directors.awards.name': 'Golden Globe Awards',
+       },
+     },
+     {
+       $project: {
+         _id: 1,
+         directors: 1,
+       },
+     },
+   ])
+   .toArray();
+ const result = results[0];

  console.log({ result });
};

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
