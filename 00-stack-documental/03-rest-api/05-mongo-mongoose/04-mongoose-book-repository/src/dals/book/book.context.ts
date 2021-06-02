import mongoose, { Schema, SchemaDefinition } from 'mongoose';
import { Book } from './book.model';

const bookSchema = new Schema({
  title: { type: Schema.Types.String, required: true },
  releaseDate: { type: Schema.Types.Date, required: true },
  author: { type: Schema.Types.String, required: true },
} as SchemaDefinition<Book>);

export const bookContext = mongoose.model<Book>('Book', bookSchema);
