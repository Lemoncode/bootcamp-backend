import { IResolvers } from '@graphql-tools/utils';
import jwt from 'jsonwebtoken';
import { UserInputError } from 'apollo-server-express';
import { GraphQLResolver, UserSession } from 'common-app/models';
import { envConstants } from 'core/constants';
import { logger } from 'core/logger';
import { userRepository } from 'dals';

interface SecurityResolvers extends IResolvers {
  Mutation: {
    login: GraphQLResolver<void, { email: string; password: string }>;
    logout: GraphQLResolver<void>;
  };
}

export const securityResolvers: SecurityResolvers = {
  Mutation: {
    login: async (_, { email, password }, context) => {
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
        context.res.cookie('authorization', `Bearer ${token}`, {
          httpOnly: true,
          secure: envConstants.isProduction,
        });
      } else {
        const message = `Invalid credentials for email ${email}`;
        logger.warn(message);
        throw new UserInputError(message);
      }
    },
    logout: async (_, __, context) => {
      // NOTE: We cannot invalidate token using jwt libraries.
      // Different approaches:
      // - Short expiration times in token
      // - Black list tokens on DB
      context.res.clearCookie('authorization');
    },
  },
};
