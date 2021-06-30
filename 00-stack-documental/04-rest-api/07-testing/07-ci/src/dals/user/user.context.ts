import mongoose, { Schema, SchemaDefinition } from 'mongoose';
import { User } from './user.model';

const userSchema = new Schema({
  email: { type: Schema.Types.String, required: true },
  password: { type: Schema.Types.String, required: true },
  salt: { type: Schema.Types.String, required: true },
  role: { type: Schema.Types.String, required: true },
} as SchemaDefinition<User>);

export const userContext = mongoose.model<User>('User', userSchema);
