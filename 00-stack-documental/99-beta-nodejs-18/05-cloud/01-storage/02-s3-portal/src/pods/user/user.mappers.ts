import * as model from '#dals/index.js';
import * as apiModel from './user.api-model.js';

export const mapUserFromModelToApi = (user: model.User): apiModel.User => ({
  email: user.email,
  role: user.role,
  avatar: user.avatar,
});
