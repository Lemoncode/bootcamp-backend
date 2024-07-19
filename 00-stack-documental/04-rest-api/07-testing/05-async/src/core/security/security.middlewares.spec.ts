import { Request, Response } from 'express';
import * as helpers from '#common/helpers/index.js';
import { UserSession } from '#core/models/index.js';
import { authenticationMiddleware } from './security.middlewares.js';

describe('security.middlewares specs', () => {
  describe('authenticationMiddleware', () => {
    it('should send 401 status code if it feeds authorization cookie equals undefined', async () => {
      // Arrange
      const authorization = undefined;
      vi.spyOn(helpers, 'verifyJWT').mockRejectedValue('Not valid token');

      const cookies: Record<string, any> = {
        authorization,
      };
      const req = {
        cookies,
      } as Request;
      const res = {
        sendStatus: vi.fn() as any,
        clearCookie: vi.fn() as any,
      } as Response;
      const next = vi.fn();

      // Act
      await authenticationMiddleware(req, res, next);

      // Assert
      expect(res.sendStatus).toHaveBeenCalledWith(401);
      expect(res.clearCookie).toHaveBeenCalledWith('authorization');
      expect(helpers.verifyJWT).toHaveBeenCalled();
    });

    it('should call next function and assign userSession if it feeds authorization cookie with token', async () => {
      // Arrange
      const authorization = 'Bearer my-token';
      const userSession: UserSession = {
        id: '1',
        role: 'admin',
      };
      vi.spyOn(helpers, 'verifyJWT').mockResolvedValue(userSession);

      const cookies: Record<string, any> = {
        authorization,
      };
      const req = {
        cookies,
      } as Request;
      const res = {
        sendStatus: vi.fn() as any,
      } as Response;
      const next = vi.fn();

      // Act
      await authenticationMiddleware(req, res, next);

      // Assert
      expect(helpers.verifyJWT).toHaveBeenCalled();
      expect(next).toHaveBeenCalled();
      expect(req.userSession).toEqual(userSession);
    });
  });
});
