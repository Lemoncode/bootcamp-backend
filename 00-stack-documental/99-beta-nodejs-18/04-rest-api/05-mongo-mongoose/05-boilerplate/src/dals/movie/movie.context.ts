import { db } from '#core/servers/index.js';
import { Movie } from './movie.model.js';

export const getMovieContext = () => db?.collection<Movie>('movies');
