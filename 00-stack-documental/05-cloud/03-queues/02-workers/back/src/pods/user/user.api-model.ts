import { Role } from '#core/models/index.js';

export interface User {
  email: string;
  role: Role;
  avatar: string;
}
