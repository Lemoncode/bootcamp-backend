import { Role } from './role.model.js';

export interface UserSession {
  id: string;
  role: Role;
}
