import { Role } from '#common-app/models/index.js';

export interface User {
  email: string;
  role: Role;
  avatar: string;
}
