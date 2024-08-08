import { User } from '../user.model.js';

export interface UserRepository {
  getUser: (email: string, password: string) => Promise<User>;
}
