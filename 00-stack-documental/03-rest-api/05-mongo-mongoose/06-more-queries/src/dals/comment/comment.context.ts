import mongoose, { Schema, SchemaDefinition } from "mongoose";
import { Comment } from "./comment.model";

const commentSchema = new Schema({} as SchemaDefinition<Comment>);

export const commentContext = mongoose.model<Comment>("Comment", commentSchema);
