import { createAction } from "@reduxjs/toolkit";
// import { GamesApi } from "../../api/games";
// import { useGetPeopleQuery } from "../../api/people";
import { gamesReceived, gameReceived, setError, setLoading, gameDeleted } from "../reducers/games";
import { AppDispatch, AppThunk } from "../store"

const ApiURL = process.env.REACT_APP_API_URL;

export const getGamesActionToolkit = createAction(
    'games',
    function getGames() {
        setLoading(true);
        return (
            {
                payload: {
                }
            }
        )
    }
)

export const getGamesAction = (): AppThunk => (dispatch: AppDispatch) => {
    dispatch(setLoading(true));
    fetch(ApiURL + 'games/')
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            throw response;
        })
        .then(data => {
            dispatch(gamesReceived(data));
        })
        .catch(error => {
            console.error('Error fetching data', error)
            dispatch(setError(error));
        })
        .finally(() => {
            // no hace falta porque si todo ha ido bien ya se ha hecho en el games received
            // dispatch(setLoading(false));
        })

    // try
    //     setLoading
    //     fetch1.disptahc

    //     fetch2.dispatch

    // catch
    //     setloading

}

export const getGameAction = (id: string): AppThunk => (dispatch: AppDispatch) => {
    dispatch(setLoading(true));
    fetch(ApiURL + 'games/' + id)
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            throw response;
        })
        .then(data => {
            dispatch(gameReceived(data));
        })
        .catch(error => {
            console.error('Error fetching data', error)
            dispatch(setError(error));
        })
        .finally(() => {
        })
}

export const deleteGameAction = (id: string): AppThunk => (dispatch: AppDispatch) => {
    dispatch(setLoading(true));
    fetch(ApiURL + 'games/' + id, { method: 'DELETE' })
        .then(response => {
            if (response.ok) {
                return response.ok;
            }
            throw response;
        })
        .then(data => {
            dispatch(gameDeleted(id));
        })
        .catch(error => {
            console.error('Error removing data', error)
            dispatch(setError(error));
        })
        .finally(() => {
        })
}