import { Role } from 'common-app/models';

export interface User {
  email: string;
  role: Role;
  avatar: string;
}
