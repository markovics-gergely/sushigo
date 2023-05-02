export interface IMessageDTO {
    text: string;
    lobbyId: string;
}

export interface IMessageViewModel {
    id: string;
    userName: string;
    text: string;
    dateTime: Date;
}