import { RequestHandler, Request, Response, NextFunction } from 'express';
import { verifyJWT } from 'common/helpers';
import { envConstants } from 'core/constants';
import { UserSession, Role } from 'common-app/models';

export const authenticationMiddleware = async (
  req: Request,
  res: Response,
  next: NextFunction
) => {
  try {
    const [, token] = req.cookies.authorization?.split(' ') || [];
    const userSession = await verifyJWT<UserSession>(
      token,
      envConstants.AUTH_SECRET
    );
    req.userSession = userSession;
    next();
  } catch (error) {
    res.sendStatus(401);
  }
};

const isAuthorized = (currentRole: Role, allowedRoles?: Role[]) =>
  !Boolean(allowedRoles) ||
  (Boolean(currentRole) && allowedRoles.some((role) => currentRole === role));

export const authorizationMiddleware =
  (allowedRoles?: Role[]): RequestHandler =>
  async (req, res, next) => {
    if (isAuthorized(req.userSession?.role, allowedRoles)) {
      next();
    } else {
      res.sendStatus(403);
    }
  };
