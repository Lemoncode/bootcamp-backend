import jwt from 'jsonwebtoken';

export const verifyJWT = <Payload>(
  token: string,
  secret: string
): Promise<Payload> =>
  new Promise((resolve, reject) => {
    jwt.verify(token, secret, (error, payload: Payload) => {
      if (error) {
        reject(error);
      }

      if (payload) {
        resolve(payload);
      } else {
        reject();
      }
    });
  });
