import { AuthenticationError } from 'apollo-server-express';
import { DirectiveResolverFn, IDirectiveResolvers } from '@graphql-tools/utils';
import { GraphQLContext, UserSession } from 'common-app/models';
import { verifyJWT } from 'common/helpers';
import { envConstants } from 'core/constants';

interface SecurityDirectives extends IDirectiveResolvers {
  isAuthenticated: DirectiveResolverFn<any, GraphQLContext>;
}

export const securityDirectives: SecurityDirectives = {
  isAuthenticated: async (next, source, args, context) => {
    try {
      const [, token] = context.req.cookies.authorization?.split(' ') || [];
      const userSession = await verifyJWT<UserSession>(
        token,
        envConstants.AUTH_SECRET
      );
      context.userSession = userSession;
      return next();
    } catch (error) {
      throw new AuthenticationError(error);
    }
  },
};
