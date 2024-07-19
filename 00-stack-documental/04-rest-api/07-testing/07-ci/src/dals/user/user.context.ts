import { dbServer } from '#core/servers/index.js';
import { User } from './user.model.js';

export const getUserContext = () => dbServer?.db.collection<User>('users');
