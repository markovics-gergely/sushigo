import { IUserNameViewModel } from "./user.models";

export interface IFriendListViewModel {
    friends: Array<IUserNameViewModel>;
    sent: Array<IUserNameViewModel>;
    received: Array<IUserNameViewModel>;
}

export interface IFriendListCounter {
    friends: string[];
    sent: string[];
    received: string[];
}

export interface IFriendStatusViewModel {
    id: string;
    status: boolean;
}