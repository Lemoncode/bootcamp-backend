import { UserRepository } from './user.repository.js';
import { db } from '../../mock-data.js';

export const mockRepository: UserRepository = {
  getUserByEmailAndPassword: async (email: string, password: string) =>
    db.users.find((u) => u.email === email && u.password === password),
};
