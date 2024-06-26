import { ENV } from '#core/constants/index.js';
import { mockRepository } from './comment.mock-repository.js';
import { mongoDBRepository } from './comment.mongodb-repository.js';

export const commentRepository = ENV.IS_API_MOCK
  ? mockRepository
  : mongoDBRepository;
