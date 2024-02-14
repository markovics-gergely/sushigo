import { DeckType } from "./deck.models";

export type AppRole = 'Classic' | 'Party' | 'CanClaimParty' | 'CanClaimDeck';
//export type DeckType = 'FirstMeal' | 'SushiGo' | 'ExperimentalParty' | 'MasterfulMenu' | 'BowlOfPlenty' | 'RaceForBestSnacks' | 'Banquet' | 'DinnerForTwo';

export interface IUser {
    access_token: string;
    refresh_token: string;
    expires_in: number;
}

export interface IRefreshViewModel {
    access_token: string;
    refresh_token: string;
    expires_in: number;
}

export interface IUserTokenViewModel {
    sub: string;
    name: string;
    role?: Array<AppRole> | AppRole;
    experience: number;
    decks?: Array<DeckType>;
    exp: number;
    avatar?: string;
    lobby?: string;
    game?: string;
    player?: string;
}

export interface ILoginUserDTO {
    username: string;
    password: string;
    grant_type?: string;
    client_id?: string;
    client_secret?: string;
    scope?: string;
}

export interface IRegisterUserDTO {
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmedPassword: string;
}

export interface IUserNameViewModel {
    id: string;
    userName: string;
    status?: boolean;
}

export interface IUserViewModel {
    userName: string;
    name: string;
    email: string;
    avatar: string;
    avatarLoaded?: boolean;
    experience: number;
}

export interface IEditUserDTO {
    userName: string;
    firstName: string;
    lastName: string;
    avatar?: File;
}