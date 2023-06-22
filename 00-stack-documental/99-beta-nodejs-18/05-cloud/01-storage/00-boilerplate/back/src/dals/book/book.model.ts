import { ObjectId } from 'mongodb';

export interface Book {
  _id: ObjectId;
  title: string;
  releaseDate: Date;
  author: string;
}
