import { db } from 'core/servers';
import { User } from './user.model';

export const getUserContext = () => db?.collection<User>('users');
