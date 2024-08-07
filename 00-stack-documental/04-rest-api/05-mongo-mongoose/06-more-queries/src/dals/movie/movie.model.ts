import { ObjectId } from 'mongodb';

// https://docs.atlas.mongodb.com/sample-data/sample-mflix/#sample_mflix.movies

export interface Genre {
  id: ObjectId;
  name: string;
}

export interface Award {
  id: ObjectId;
  name: string;
  year: number;
}

export interface Director {
  id: ObjectId;
  name: string;
  awards: Award[];
}

export interface Movie {
  _id: ObjectId;
  title: string;
  year: number;
  runtime: number;
  released: Date;
  poster?: string;
  plot: string;
  fullplot: string;
  lastupdated: Date;
  type: MovieType;
  directors?: string[] | Director[];
  countries?: string[];
  genres?: string[] | Genre[];
  cast?: string[];
  num_mflix_comments: number;
  imdb?: IMDB;
  tomatoes?: Tomatoes;
}

export type MovieType = 'movie' | 'series';

export interface IMDB {
  rating: number;
  votes: number;
  id: number;
}

export interface Tomatoes {
  viewer: TomatoesViewer;
  lastUpdated: Date;
}

export interface TomatoesViewer {
  rating: number;
  numReviews: number;
}
