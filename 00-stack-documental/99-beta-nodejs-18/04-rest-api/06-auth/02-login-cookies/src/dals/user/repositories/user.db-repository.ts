import { UserRepository } from './user.repository.js';

export const dbRepository: UserRepository = {
  getUserByEmailAndPassword: async (email: string, password: string) => null,
};
