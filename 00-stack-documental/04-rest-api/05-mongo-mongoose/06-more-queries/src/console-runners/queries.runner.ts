import { ObjectId } from 'mongodb';
import { getMovieContext } from '#dals/movie/movie.context.js';

export const run = async () => {
  const results = await getMovieContext()
    .aggregate([
      {
        $unwind: '$directors',
      },
      {
        $unwind: '$directors.awards',
      },
      {
        $match: {
          _id: new ObjectId('573a1390f29313caabcd4135'),
          'directors.name': 'Jane Doe',
          'directors.awards.name': 'Golden Globe Awards',
        },
      },
      {
        $project: {
          _id: 1,
          directors: 1,
        },
      },
    ])
    .toArray();
  const result = results[0];
  console.log({ result });
};
