import { mockRepository } from "./movie.mock-repository";
import { dbRepository } from "./movie.db-repository";
import { envConstants } from "core/constants";

export const movieRepository = envConstants.isApiMock
  ? mockRepository
  : dbRepository;
