import { disconnect } from 'mongoose';
import { envConstants } from 'core/constants';
import { connectToDBServer } from 'core/servers';
import { movieContext } from 'dals/movie/movie.context';

const runQueries = async () => {
  const result = await movieContext
    .find(
      {
        runtime: { $lte: 15 },
      },
      {
        _id: 1,
        title: 1,
        directors: 1,
      }
    )
    .lean();
};

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);
  await runQueries();
  await disconnect();
};
