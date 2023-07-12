import { Role } from './role.js';

export interface UserSession {
  id: string;
  role: Role;
}
