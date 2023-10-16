import { Router } from 'express';
import jwt from 'jsonwebtoken';
import { UserSession } from '#common-app/models/index.js';
import { envConstants } from '#core/constants/index.js';
import { userRepository } from '#dals/index.js';
import { authenticationMiddleware } from './security.middlewares.js';

export const securityApi = Router();

securityApi
  .post('/login', async (req, res, next) => {
    try {
      const { email, password } = req.body;
      const user = await userRepository.getUserByEmailAndPassword(
        email,
        password
      );

      if (user) {
        const userSession: UserSession = {
          id: user._id.toHexString(),
          role: user.role,
        };
        const token = jwt.sign(userSession, envConstants.AUTH_SECRET, {
          expiresIn: '1d',
          algorithm: 'HS256',
        });
        // TODO: Move to constants
        res.cookie('authorization', `Bearer ${token}`, {
          httpOnly: true,
          secure: envConstants.isProduction,
        });
        res.sendStatus(204);
      } else {
        res.sendStatus(401);
      }
    } catch (error) {
      next(error);
    }
  })
  .post('/logout', authenticationMiddleware, async (req, res) => {
    // NOTE: We cannot invalidate token using jwt libraries.
    // Different approaches:
    // - Short expiration times in token
    // - Black list tokens on DB
    res.clearCookie('authorization');
    res.sendStatus(200);
  });
