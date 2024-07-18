import { Router } from 'express';
import jwt from 'jsonwebtoken';
import { UserSession } from '#core/models/index.js';
import { ENV } from '#core/constants/index.js';
import { userRepository } from '#dals/index.js';

export const securityApi = Router();

securityApi
  .post('/login', async (req, res, next) => {
    try {
      const { email, password } = req.body;
      const user = await userRepository.getUser(email, password);
      if (user) {
        const userSession: UserSession = {
          id: user._id.toHexString(),
          role: user.role,
        };
        const token = jwt.sign(userSession, ENV.AUTH_SECRET, {
          expiresIn: '1d',
          algorithm: 'HS256',
        });
        // TODO: Move to constants
        res.cookie('authorization', `Bearer ${token}`, {
          httpOnly: true,
          secure: ENV.IS_PRODUCTION,
        });
        res.sendStatus(204);
      } else {
        res.clearCookie('authorization');
        res.sendStatus(401);
      }
    } catch (error) {
      next(error);
    }
  })
  .post('/logout', async (req, res) => {
    // NOTE: We cannot invalidate token using jwt libraries.
    // Different approaches:
    // - Short expiration times in token
    // - Black list tokens on DB
    res.clearCookie('authorization');
    res.sendStatus(200);
  });
