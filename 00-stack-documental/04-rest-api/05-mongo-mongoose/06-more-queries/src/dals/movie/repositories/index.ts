import { ENV } from '#core/constants/index.js';
import { mockRepository } from './movie.mock-repository.js';
import { mongoDBRepository } from './movie.mongodb-repository.js';

export const movieRepository = ENV.IS_API_MOCK
  ? mockRepository
  : mongoDBRepository;
