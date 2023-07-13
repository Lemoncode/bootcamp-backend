import { GraphQLResolveInfo } from 'graphql';
import { Request as GraphQLRequest } from 'graphql-http';
import { RequestContext } from 'graphql-http/lib/use/express';
import { Request, Response } from 'express';
import { UserSession } from './user-session.js';

export interface GraphQLContext {
  req: GraphQLRequest<Request, RequestContext>;
  res: Response;
  userSession?: UserSession;
}

export type GraphQLResolver<Args, ReturnType> = (
  args: Args,
  context: GraphQLContext,
  info: GraphQLResolveInfo
) => Promise<ReturnType>;
