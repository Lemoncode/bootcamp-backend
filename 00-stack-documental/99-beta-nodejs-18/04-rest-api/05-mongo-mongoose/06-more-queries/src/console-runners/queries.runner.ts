import { ObjectId } from 'mongodb';
import { envConstants } from 'core/constants';
import { connectToDBServer, disconnectFromDBServer } from 'core/servers';
import { getCommentContext } from 'dals/comment/comment.context';

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
          as: 'movie',
        },
      },
      {
        $unwind: '$movie'
      },
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

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);
  await runQueries();
  await disconnectFromDBServer();
};
