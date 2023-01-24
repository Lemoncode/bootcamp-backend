import { gamesAdapter } from "../reducers/games";
import { RootState } from "../store";

export const gamesSelector = (state: RootState) => { return state.games }

export const {
    selectById: selectGameById,
    selectIds: selectGameIds,
    selectEntities: selectGameEntities,
    selectAll: selectAllGames,
    selectTotal: selectTotalGames,
  } = gamesAdapter.getSelectors((state: RootState) => state.games)


export const getGamesSelector = gamesAdapter.getSelectors<RootState>(
    (state) => state.games
);

export const gameByIdSelector = (id: any) => {
    return (state: RootState) => selectGameById(state, id);
}
