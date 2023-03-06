import { db } from '#core/servers/index.js';
import { User } from './user.model.js';

export const getUserContext = () => db?.collection<User>('users');
