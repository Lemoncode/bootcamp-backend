import mongoose, { Schema, SchemaDefinition } from 'mongoose';
import { Movie, IMDB, Tomatoes, TomatoesViewer } from './movie.model';

const imdbSchema = new Schema({
  rating: { type: Schema.Types.Number, required: true },
  votes: { type: Schema.Types.Number, required: true },
  id: { type: Schema.Types.Number, required: true },
} as SchemaDefinition<IMDB>);

const tomatoesSchema = new Schema({
  viewer: {
    type: new Schema({
      rating: { type: Schema.Types.Number, required: true },
      numReviews: { type: Schema.Types.Number, required: true },
    } as SchemaDefinition<TomatoesViewer>),
    required: true,
  },
  lastUpdated: { type: Schema.Types.Date, required: true },
} as SchemaDefinition<Tomatoes>);

const movieSchema = new Schema({
  title: { type: Schema.Types.String, required: true },
  year: { type: Schema.Types.Number, required: true },
  runtime: { type: Schema.Types.Number, required: true },
  released: { type: Schema.Types.Date, required: true },
  poster: { type: Schema.Types.String },
  plot: { type: Schema.Types.String, required: true },
  fullplot: { type: Schema.Types.String, required: true },
  lastupdated: { type: Schema.Types.Date, required: true },
  type: { type: Schema.Types.String, required: true },
  directors: [{ type: Schema.Types.String }],
  countries: [{ type: Schema.Types.String }],
  genres: [{ type: Schema.Types.String }],
  cast: [{ type: Schema.Types.String }],
  num_mflix_comments: { type: Schema.Types.Number, required: true },
  imdb: { type: imdbSchema },
  tomatoes: { type: tomatoesSchema },
} as SchemaDefinition<Movie>);

export const movieContext = mongoose.model<Movie>('Movie', movieSchema);
