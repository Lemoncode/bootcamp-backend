import jwt from 'jsonwebtoken';
import { GraphQLResolver, UserSession } from '#core/models/index.js';
import { logger } from '#core/logger/index.js';
import { ENV } from '#core/constants/index.js';
import { userRepository } from '#dals/index.js';

interface SecurityResolvers {
  login: GraphQLResolver<{ email: string; password: string }, void>;
  logout: GraphQLResolver<never, void>;
}

export const securityResolvers: SecurityResolvers = {
  login: async ({ email, password }, context) => {
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
      context.res.cookie('authorization', `Bearer ${token}`, {
        httpOnly: true,
        secure: ENV.IS_PRODUCTION,
      });
    } else {
      const message = `Invalid credentials for email ${email}`;
      logger.warn(message);
      throw new Error(JSON.stringify({ message, statusCode: 401 }));
    }
  },
  logout: async (_, content) => {
    // NOTE: We cannot invalidate token using jwt libraries.
    // Different approaches:
    // - Short expiration times in token
    // - Black list tokens on DB
    content.res.clearCookie('authorization');
  },
};
