import { mockRepository } from "./comment.mock-repository.js";
import { dbRepository } from "./comment.db-repository.js";
import { envConstants } from "#core/constants/index.js";

export const commentRepository = envConstants.isApiMock
  ? mockRepository
  : dbRepository;
