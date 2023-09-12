import { InjectionToken } from '@angular/core';
import { CardType, DeckType } from './deck.models';

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

const isNumeric = (value: string): boolean =>
  !new RegExp(/[^\d]/g).test(value.trim());

export class PhaseUtil {
  public static equals(
    enum1: string | number | undefined,
    enum2: string | number | undefined
  ): boolean {
    if (typeof enum1 === 'number') {
      enum1 = Phase[enum1];
    } else if (enum1 && isNumeric(enum1)) {
      enum1 = Phase[parseInt(enum1)];
    }
    if (typeof enum2 === 'number') {
      enum2 = Phase[enum2];
    } else if (enum2 && isNumeric(enum2)) {
      enum2 = Phase[parseInt(enum2)];
    }
    return enum1 === enum2;
  }
  public static getString(phase: number | string): string {
    if (typeof phase === 'number') {
      return Phase[phase];
    } else if (isNumeric(phase)) {
      return Phase[parseInt(phase)];
    }
    return phase;
  }
}
export class AdditionalUtil {
  public static equals(
    enum1: string | number | undefined,
    enum2: string | number | undefined
  ): boolean {
    if (typeof enum1 === 'number') {
      enum1 = Additional[enum1];
    } else if (enum1 && isNumeric(enum1)) {
      enum1 = Additional[parseInt(enum1)];
    }
    if (typeof enum2 === 'number') {
      enum2 = Additional[enum2];
    } else if (enum2 && isNumeric(enum2)) {
      enum2 = Additional[parseInt(enum2)];
    }
    return enum1 === enum2;
  }
  public static getFromRecord(
    record: Record<string | number, string>,
    key: Additional
  ): string | undefined {
    const enumKey = Additional[key];
    return record[enumKey] ?? record[key];
  }
}

export interface ICardViewModel {
  id: string;
  cardType: CardType;
  additionalInfo: Record<Additional, string>;
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
  imageLoaded?: boolean;
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

export const CARD = new InjectionToken<IHandCardViewModel>('CARD');
export const HAND = new InjectionToken<boolean>('HAND');

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
  additionalInfo: Record<Additional, string>;
}

export interface IPlayCardDTO {
  handCardId: string;
  additionalInfo: Record<Additional, string>;
}
