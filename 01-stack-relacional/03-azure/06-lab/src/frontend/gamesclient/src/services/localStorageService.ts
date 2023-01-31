export enum LocalStorageKey {
    SCHOOL_ID = 'school',
    LANGUAGE = 'i18nextLng',
    ROLE = 'role',
    OPENED_TABS = 'openedTab',
}

export class LocalStorageService {
    setItem(key: LocalStorageKey, item: any): void {
        localStorage.setItem(key, JSON.stringify(item));
    }

    setPlainItem(key: LocalStorageKey, item: any): void {
        localStorage.setItem(key, item);
    }

    getItem(key: LocalStorageKey): any {
        const data = localStorage.getItem(key);
        if (!data) {
            return null;
        }
        const plainTypesAllowed = ['string', 'number', 'boolean'];
        if (plainTypesAllowed.includes(typeof data)) {
            return data;
        }
        return JSON.parse(data);
    }
}
