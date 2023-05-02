import { DeckType } from "./deck.models";

export interface IPlayerViewModel {
    id: string;
    userName: string;
}

export interface ILobbyViewModel {
    id: string;
    name: string;
    creatorId: string;
    created: Date;
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

export interface RemovePlayerDTO {
    playerId: string;
    lobbyId: string;
}

export interface IUpdateLobbyDTO {
    lobbyId: string;
    deckType: DeckType;
}