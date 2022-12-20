export interface User {
  email: string;
  password: string;
}

export const createEmptyUser = (): User => ({
  email: '',
  password: '',
});
