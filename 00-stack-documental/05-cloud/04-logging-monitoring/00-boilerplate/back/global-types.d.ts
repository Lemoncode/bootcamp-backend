declare namespace Express {
  export type Role = 'admin' | 'standard-user';

  export interface UserSession {
    id: string;
    role: Role;
  }

  export interface Request {
    userSession?: UserSession;
  }
}
