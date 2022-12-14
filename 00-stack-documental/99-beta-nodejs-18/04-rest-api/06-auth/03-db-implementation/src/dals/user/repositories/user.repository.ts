import { User } from '../user.model.js';

export interface UserRepository {
  getUserByEmailAndPassword: (email: string, password: string) => Promise<User>;
}
