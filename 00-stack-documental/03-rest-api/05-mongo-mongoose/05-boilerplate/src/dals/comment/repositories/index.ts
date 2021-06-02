import { mockRepository } from "./comment.mock-repository";
import { dbRepository } from "./comment.db-repository";
import { envConstants } from "core/constants";

export const commentRepository = envConstants.isApiMock
  ? mockRepository
  : dbRepository;
