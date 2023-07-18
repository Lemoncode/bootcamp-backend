import { mapSchema, MapperKind, getDirective } from '@graphql-tools/utils';
import { GraphQLSchema, defaultFieldResolver } from 'graphql';
import { verifyJWT } from '#common/helpers/index.js';
import { UserSession } from '#common-app/models/index.js';
import { envConstants } from '#core/constants/index.js';
import { GraphQLContext } from '#common-app/models/graphql.js';

const isAuthenticatedResolver = async (
  _,
  context: GraphQLContext,
  info,
  next: () => void
) => {
  try {
    const [, token] = context.req.raw.cookies.authorization?.split(' ') || [];
    const userSession = await verifyJWT<UserSession>(
      token,
      envConstants.AUTH_SECRET
    );
    context.userSession = userSession;
    return next();
  } catch (error) {
    const message = 'User is not authenticated';
    throw new Error(JSON.stringify({ message, statusCode: 401 }));
  }
};

export const securityDirectives = (schema: GraphQLSchema) =>
  mapSchema(schema, {
    [MapperKind.OBJECT_FIELD]: (fieldConfig) => {
      const directive = getDirective(schema, fieldConfig, 'isAuthenticated');
      if (directive) {
        const { resolve = defaultFieldResolver } = fieldConfig;
        return {
          ...fieldConfig,
          resolve: async (source, args, context, info) => {
            const next = () => resolve(source, args, context, info);
            return await isAuthenticatedResolver(args, context, info, next);
          },
        };
      }
    },
  });
