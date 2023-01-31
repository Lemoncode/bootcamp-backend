import { createEntityAdapter, createSlice } from "@reduxjs/toolkit";
import { IGame } from "../../models/IGame";

// const ApiURL = process.env.REACT_APP_API_URL;

export const gamesAdapter = createEntityAdapter<IGame>({
    selectId: (game) => game.id!,
    sortComparer: (a, b) => a.title!.localeCompare(b.title!)
})

// const setControlProps = (state: any, isLoading: boolean, hasErrors: boolean, errorMessage: string) => {
//     state.isLoading = isLoading;
//     state.hasErrors = hasErrors;
//     state.errorMessage = errorMessage;
// }

// export const fetchgames = createAsyncThunk(
//     'games/list',
//     async (dispatch, getState) => {
//         // setLoading(true);
//         const result = await fetch(ApiURL + 'games')
//             .then(response => {
//                 if (response.ok) {
//                     return response.json();
//                 }
//                 throw response;
//             })
//             .then(data => {
//                 gamesReceived(data);
//             })
//             .catch(error => {
//                 console.error('Error fetching data', error)
//                 setError(error);
//                 return error?.response
//             })
//             .finally(() => {
//                 // no hace falta porque si todo ha ido bien ya se ha hecho en el games received
//                 // dispatch(setLoading(false));
//             });
//         return result;
//     }
// )

export const gamesSlice = createSlice({
    name: 'games',
    initialState: gamesAdapter.getInitialState({
        isLoading: false, 
        hasErrors: false, 
        errorMessage: '',
        selected: -1
    }),
    reducers:{
        setSelected: (state, action) => {
          state.selected = action.payload;
        },
        gamesReceived: (state, action) => {
            gamesAdapter.setAll(state, action.payload);
            state.isLoading = false;
            state.hasErrors = false;
            state.errorMessage = '';
        },
        gameReceived: (state, action) => {
            gamesAdapter.setOne(state, action.payload);
            state.isLoading = false;
            state.hasErrors = false;
            state.errorMessage = '';
        },
        gamesCreated: (state, action) => {
            gamesAdapter.setOne(state, action.payload);
            state.isLoading = false;
            state.hasErrors = false;
            state.errorMessage = '';
        },
        gamesUpdated: (state, action) => {
            gamesAdapter.upsertOne(state, action.payload);
            state.isLoading = false;
            state.hasErrors = false;
            state.errorMessage = '';
        },        
        gameDeleted: (state, action) => {
            gamesAdapter.removeOne(state, action.payload);
            state.isLoading = false;
            state.hasErrors = false;
            state.errorMessage = '';
        },
        setLoading: (state, action) => {
            state.isLoading = action.payload;
        },
        setError: (state, action) => {
            state.isLoading = false;
            state.hasErrors = false;
            state.errorMessage = action.payload;
        }
    }
});

export const { gamesReceived, gameReceived, gameDeleted, setLoading, setError } = gamesSlice.actions;

export default gamesSlice.reducer;