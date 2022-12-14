import { Router } from 'express';
import { userRepository } from 'dals';
import { mapUserFromModelToApi } from './user.mappers';

export const userApi = Router();

userApi.get('/', async (req, res, next) => {
  try {
    const user = await userRepository.getUserById(req.userSession.id);
    const apiUser = await mapUserFromModelToApi(user);
    res.send(apiUser);
  } catch (error) {
    next(error);
  }
});
