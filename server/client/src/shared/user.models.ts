export type AppRole = 'Classic' | 'Party' | 'CanClaimParty';
export type GameType = 'FirstMeal' | 'SushiGo' | 'ExperimentalParty' | 'MasterfulMenu' | 'BowlOfPlenty' | 'RaceForBestSnacks' | 'Banquet' | 'DinnerForTwo';

export interface IUser {
    access_token: string;
    refresh_token: string;
    detail: string;
    expires_in: number;
}

export interface IUserViewModel {
    sub: string;
    role?: Array<AppRole> | AppRole;
    experience: number;
    games?: Array<GameType>;
    exp: number;
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