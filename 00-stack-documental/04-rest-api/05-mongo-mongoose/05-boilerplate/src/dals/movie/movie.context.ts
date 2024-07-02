import { dbServer } from '#core/servers/index.js';
import { Movie } from './movie.model.js';

export const getMovieContext = () => dbServer.db?.collection<Movie>('movies');
