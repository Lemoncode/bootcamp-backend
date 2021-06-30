import axios from 'axios';
import { User } from './user.api-model';

export const getUser = async (): Promise<User> => {
  const { data } = await axios.get<User>('/api/users');
  return data;
};
