import { User } from '../user.model';

export interface UserRepository {
  getUserByEmailAndPassword: (email: string, password: string) => Promise<User>;
  getUserById: (id: string) => Promise<User>;
}
