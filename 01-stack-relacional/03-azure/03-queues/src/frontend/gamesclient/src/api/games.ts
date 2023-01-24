import { ApiMethods } from '../constants/apiMethods';
import { IGame } from '../models/IGame';
import { apiFetch } from '../services/apiClient';

const baseURL = 'https://lemoncode-azure-api.azurewebsites.net/api/';

export class GamesApi {
    async get(): Promise<IGame[]> {
        const url = `${baseURL}/tests`;
        return (await apiFetch(ApiMethods.GET, url)).execute<IGame[]>();
    }
}
