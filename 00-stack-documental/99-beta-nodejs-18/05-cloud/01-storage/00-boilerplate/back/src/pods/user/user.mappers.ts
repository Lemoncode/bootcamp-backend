import * as model from 'dals/user';
import * as apiModel from './user.api-model';

export const mapUserFromModelToApi = (user: model.User): apiModel.User => ({
  email: user.email,
  role: user.role,
});
