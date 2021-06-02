import { ObjectId } from 'mongodb';

// https://docs.atlas.mongodb.com/sample-data/sample-mflix/#sample_mflix.comments

export interface Comment {
  _id: ObjectId;
  name: string;
  email: string;
  movie_id: ObjectId;
  text: string;
  date: Date;
}
