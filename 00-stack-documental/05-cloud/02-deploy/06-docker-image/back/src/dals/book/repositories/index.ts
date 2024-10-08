import { mockRepository } from "./book.mock-repository.js";
import { mongoDBRepository } from "./book.mongodb-repository.js";
import { ENV } from "#core/constants/index.js";

export const bookRepository = ENV.IS_API_MOCK
  ? mockRepository
  : mongoDBRepository;
