import { gamesAdapter } from "../reducers/games";
import { RootState } from "../store";

export const gamesSelector = (state: RootState) => { return state.games }

// export const getGamesSelector = (state: RootState) => {
//     const games = gamesSelector(state);

//     return games;
// }

export const getGamesSelector = gamesAdapter.getSelectors<RootState>(
    (state) => state.games
);
