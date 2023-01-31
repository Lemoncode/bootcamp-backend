import { Action, configureStore, ThunkAction } from "@reduxjs/toolkit";
import gamesReducer from './reducers/games'

export const store = configureStore({ 
    reducer: {
        games: gamesReducer
    }
});
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export type AppThunk = ThunkAction<any, RootState, any, Action<string>>;