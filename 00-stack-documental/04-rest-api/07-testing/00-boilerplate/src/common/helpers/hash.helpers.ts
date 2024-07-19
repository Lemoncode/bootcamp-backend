import crypto from 'node:crypto';

const saltLenght = 32; // 16 bytes min recommended
const hashLength = 64; // 64 bytes = 512 bits

const hashSaltAndPassword = (salt: string, password: string): Promise<string> =>
  new Promise((resolve, reject) => {
    // The default options values are using 2^14 = 16384 iterations and 16 MB of memory.
    crypto.scrypt(password, salt, hashLength, (error, hashedPassword) => {
      if (error) {
        reject(error);
      }

      // salt:hash
      resolve(`${salt}:${hashedPassword.toString('hex')}`);
    });
  });

export const hash = async (password: string): Promise<string> => {
  const salt = crypto.randomBytes(saltLenght).toString('hex');

  return await hashSaltAndPassword(salt, password);
};

// Recommended comparison to protect against [timing attacks](https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html#compare-password-hashes-using-safe-functions)
const areEquals = (hashA: string, hashB: string): boolean =>
  crypto.timingSafeEqual(Buffer.from(hashA, 'hex'), Buffer.from(hashB, 'hex'));

export const verifyHash = async (
  password: string,
  hashedPassword: string
): Promise<boolean> => {
  const [salt, hash] = hashedPassword.split(':');

  const [, newHash] = (await hashSaltAndPassword(salt, password)).split(':');
  return areEquals(hash, newHash);
};
