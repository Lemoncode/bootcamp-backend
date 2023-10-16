import { Router } from 'express';
import { userRepository } from '#dals/index.js';
import { mapUserFromModelToApi } from './user.mappers.js';

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
