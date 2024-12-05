import { mockRepository } from './user.mock-repository.js';
import { mongoDBRepository } from './user.mongodb-repository.js';
import { ENV } from '#core/constants/index.js';

export const userRepository = ENV.IS_API_MOCK
  ? mockRepository
  : mongoDBRepository;
