import { UserRepository } from './user.repository.js';

export const mongoDBRepository: UserRepository = {
  getUser: async (email: string, password: string) => {
    throw new Error('Not implemented');
  },
};
