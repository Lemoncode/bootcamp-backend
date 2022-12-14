export interface User {
  email: string;
  role: string;
  avatar: string;
}

export const createEmptyUser = (): User => ({
  email: '',
  role: '',
  avatar: '',
});
