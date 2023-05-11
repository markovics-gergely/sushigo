import { DeckType } from "./deck.models";

export interface IPlayerViewModel {
    id: string;
    userName: string;
    imagePath?: string;
    imageLoaded?: boolean;
    ready: boolean;
}

export interface ILobbyViewModel {
    id: string;
    name: string;
    password: string;
    creatorUserId: string;
    creatorUserName: string;
    created: Date;
    deckType: DeckType;
    players: Array<IPlayerViewModel>;
}

export interface ILobbyItemViewModel {
    id: string;
    name: string;
}

export interface ICreateLobbyDTO {
    name: string;
    password: string;
    creatorImagePath?: string;
}

export interface IPlayerDTO {
    userId: string;
    userName: string;
    imagePath?: string;
    lobbyId: string;
}

export interface IRemovePlayerDTO {
    playerId: string;
    lobbyId: string;
}

export interface IUpdateLobbyDTO {
    lobbyId: string;
    deckType: DeckType;
}

export interface IJoinLobbyDTO {
    id: string;
    password: string;
}

export interface IPlayerReadyDTO {
    playerId: string;
    ready: boolean;
}
