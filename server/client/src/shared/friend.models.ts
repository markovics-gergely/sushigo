import { IUserNameViewModel } from "./user.models";

export interface IFriendListViewModel {
    friends: Array<IUserNameViewModel>;
    sent: Array<IUserNameViewModel>;
    received: Array<IUserNameViewModel>;
}

export interface IFriendStatusViewModel {
    id: string;
    status: boolean;
}