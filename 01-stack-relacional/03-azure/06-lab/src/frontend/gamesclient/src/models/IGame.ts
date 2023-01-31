export interface IGame {
    id?: number,
    title?: string,
    info?: string,
    year?: number,
    posterUrl?: string,
    genre?: string,
    downloadUrl?: string,
    ageGroup?: string,
    playability?: number,
    rating?: number
    screenshots: IScreenshot[]
}

export interface IScreenshot {
    id?: number,
    url?: string,
    thumbnailurl?: string,
    gameid?: number,
    filename?: string
}