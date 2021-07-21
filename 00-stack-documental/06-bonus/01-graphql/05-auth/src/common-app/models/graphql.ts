import { GraphQLResolveInfo } from 'graphql';
import { Request, Response } from 'express';
import { UserSession } from './user-session';

export interface GraphQLContext {
  req: Request;
  res: Response;
  userSession?: UserSession;
}

export type GraphQLResolver<ReturnedType, Args = any> = (
  rootObject: any,
  args: Args,
  context: GraphQLContext,
  info: GraphQLResolveInfo
) => Promise<ReturnedType>;
