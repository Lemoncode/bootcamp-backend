import mongoose, { Schema, SchemaDefinition } from "mongoose";
import { Movie } from "./movie.model";

const movieSchema = new Schema({} as SchemaDefinition<Movie>);

export const movieContext = mongoose.model<Movie>("Movie", movieSchema);
