import { RequestHandler, Request, Response, NextFunction } from 'express';
import { verifyJWT } from '#common/helpers/index.js';
import { ENV } from '#core/constants/index.js';
import { UserSession, Role } from '#core/models/index.js';

export const authenticationMiddleware = async (
  req: Request,
  res: Response,
  next: NextFunction
) => {
  try {
    const [, token] = req.cookies.authorization?.split(' ') || [];
    const userSession = await verifyJWT<UserSession>(token, ENV.AUTH_SECRET);
    req.userSession = userSession;
    next();
  } catch (error) {
    res.clearCookie('authorization');
    res.sendStatus(401);
  }
};

const isAuthorized = (currentRole: Role, allowedRoles: Role[]) =>
  Boolean(currentRole) && allowedRoles?.some((role) => currentRole === role);

export const authorizationMiddleware =
  (allowedRoles: Role[]): RequestHandler =>
  async (req, res, next) => {
    if (isAuthorized(req.userSession?.role, allowedRoles)) {
      next();
    } else {
      res.sendStatus(403);
    }
  };
