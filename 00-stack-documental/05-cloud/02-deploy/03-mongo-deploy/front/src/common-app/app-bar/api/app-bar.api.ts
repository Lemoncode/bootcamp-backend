import axios from 'axios';

export const logout = async () => await axios.post('/api/security/logout');
