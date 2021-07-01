import { UserRepository } from './user.repository';
import { db } from '../../mock-data';

export const mockRepository: UserRepository = {
  getUserByEmailAndPassword: async (email: string, password: string) =>
    db.users.find((u) => u.email === email && u.password === password),
  getUserById: async (id: string) =>
    db.users.find((u) => u._id.toHexString() === id),
};
