import { InjectionToken } from "@angular/core";
import { CardType, DeckType } from "./deck.models";

export enum Phase {
    None,
    Turn,
    AfterTurn,
    EndTurn,
    EndRound,
    EndGame,
}

export enum Additional {
    None,
    Points,
    Tagged,
    CardIds,
}

export interface ICardViewModel {
    id: string;
    cardType: CardType;
    additionalInfo: Record<string, string>;
}

export interface IBoardCardViewModel extends ICardViewModel {}

export interface IHandCardViewModel extends ICardViewModel {
    isSelected: boolean;
}

export interface IHandViewModel {
    cards: IHandCardViewModel[];
}

export interface IBoardViewModel {
    cards: IBoardCardViewModel[];
}

export interface IPlayerViewModel {
    id: string;
    userId: string;
    userName: string;
    imagePath?: string;
    points: number;
    selectedCardId?: string;
    selectedCardType?: CardType;
    board: IBoardViewModel;
    handId: string;
}

export interface IGameViewModel {
    id: string;
    name: string;
    deckType: DeckType;
    actualPlayerId: string;
    firstPlayerId: string;
    round: number;
    phase: Phase;
    players: IPlayerViewModel[];
}

export const CARD = new InjectionToken<ICardViewModel>('CARD');

export interface ICreatePlayerDTO {
    userId: string;
    userName: string;
    imagePath?: string;
}

export interface ICreateGameDTO {
    name: string;
    deckType: DeckType;
    players: ICreatePlayerDTO[];
}

export interface IPlayAfterTurnDTO {
    boardCardId: string;
    additionalInfo: Record<string, string>;
}

export interface IPlayCardDTO {
    handCardId: string;
    additionalInfo: Record<Additional, string>;
}