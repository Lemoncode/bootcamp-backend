import { mockRepository } from './book.mock-repository';
import { dbRepository } from './book.db-repository';
import { envConstants } from 'core/constants';

export const bookRepository = envConstants.isApiMock
  ? mockRepository
  : dbRepository;
