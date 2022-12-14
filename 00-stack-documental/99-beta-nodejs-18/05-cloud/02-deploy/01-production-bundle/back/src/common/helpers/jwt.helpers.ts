import jwt from 'jsonwebtoken';

export const verifyJWT = <T>(token: string, secret: string): Promise<T> =>
  new Promise<T>((resolve, reject) => {
    jwt.verify(token, secret, (error, payload) => {
      if (error) {
        reject(error);
      }

      if (payload) {
        resolve(payload as unknown as T);
      } else {
        reject();
      }
    });
  });
