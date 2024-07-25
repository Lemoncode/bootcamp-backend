import { sign } from 'jsonwebtoken';
import { isLowerThan, max } from './business/index.js';

export const add = (a, b) => {
  const result = a + b;

  if (result < max) {
    isLowerThan(result, max);
    const token = sign(result, 'my-secret');
    console.log({ token });
  }

  return result;
};
