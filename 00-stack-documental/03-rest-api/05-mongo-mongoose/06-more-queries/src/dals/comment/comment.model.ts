import { ObjectId } from 'mongodb';
import { Movie } from 'dals/movie';

// https://docs.atlas.mongodb.com/sample-data/sample-mflix/#sample_mflix.comments

export interface Comment {
  _id: ObjectId;
  name: string;
  email: string;
  movie_id: ObjectId;
  movie?: Movie;
  text: string;
  date: Date;
}
