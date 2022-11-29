import axios from 'axios';

export const login = async (email: string, password: string) =>
  await axios.post('/api/security/login', { email, password });
