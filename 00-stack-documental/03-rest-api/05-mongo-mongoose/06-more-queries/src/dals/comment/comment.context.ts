import mongoose, { Schema, SchemaDefinition } from 'mongoose';
import { Comment } from './comment.model';

const commentSchema = new Schema({
  name: { type: Schema.Types.String, required: true },
  email: { type: Schema.Types.String, required: true },
  movie_id: { type: Schema.Types.ObjectId, required: true },
  text: { type: Schema.Types.String, required: true },
  date: { type: Schema.Types.Date, required: true },
} as SchemaDefinition<Comment>);

export const commentContext = mongoose.model<Comment>('Comment', commentSchema);
