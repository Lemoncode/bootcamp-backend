import { mockRepository } from "./movie.mock-repository.js";
import { dbRepository } from "./movie.db-repository.js";
import { envConstants } from "#core/constants/index.js";

export const movieRepository = envConstants.isApiMock
  ? mockRepository
  : dbRepository;
