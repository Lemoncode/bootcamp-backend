import { UserRepository } from './user.repository';

export const dbRepository: UserRepository = {
  getUserByEmailAndPassword: async (email: string, password: string) => null,
};
