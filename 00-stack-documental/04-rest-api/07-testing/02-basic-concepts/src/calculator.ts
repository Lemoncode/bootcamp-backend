import { sign } from 'jsonwebtoken';
import { isLowerThan, max } from './business/index.js';

export const add = (a, b) => {
  const result = a + b;

  if (result < max) {
    isLowerThan(result, max);
    sign(result, 'my-secret');
  }

  return result;
};
