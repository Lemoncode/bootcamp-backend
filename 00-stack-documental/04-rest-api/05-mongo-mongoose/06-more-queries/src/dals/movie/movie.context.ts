import { db } from 'core/servers';
import { Movie } from './movie.model';

export const getMovieContext = () => db?.collection<Movie>('movies');
