import { User } from '../user.model.js';

// Just a dumb memory implementation just use for local dev purpose
// later on migrate to a MongoDb or whatever Db implementation

let lastId: number = 1;
let userCollection: User[] = [];

export const userProfileExists = async (
  googleProfileId: string
): Promise<boolean> => {
  const index =
    userCollection.findIndex(
      (user) => user.googleId === googleProfileId
    );

  return index !== -1;
};

export const addNewUser = async (user: User): Promise<User> => {
  const newUser = {
    ...user,
    id: lastId,
  };

  userCollection = [...userCollection, newUser];

  lastId++;

  return newUser;
};

export const getUser = async (id: number): Promise<User> => {
  const user = userCollection.find((user) => user.id === id);

  return user;
};

export const getUserByGoogleId = async (googleId: string): Promise<User> => {
  const user = userCollection.find((user) => user.googleId === googleId);

  return user;
};