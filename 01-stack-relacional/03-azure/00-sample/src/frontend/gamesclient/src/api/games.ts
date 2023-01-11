import { ApiMethods } from '../constants/apiMethods';
import { IGame } from '../models/IGame';
import { apiFetch } from '../services/apiClient';

const baseURL = process.env.REACT_APP_API_URL;

export class GamesApi {
    async get(): Promise<IGame[]> {
        const url = `${baseURL}/tests`;
        return (await apiFetch(ApiMethods.GET, url)).execute<IGame[]>();
    }
}