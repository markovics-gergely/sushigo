import { CardType, DeckType } from 'src/shared/models/deck.models';

export enum Phase {
  None,
  Turn,
  MenuSelect,
  AfterTurn,
  EndTurn,
  EndRound,
  EndGame,
  Result,
}

export enum CardTagType {
  None,
  Converted,
  Used,
  Wasabi,
}

export interface ICardInfo {
  cardType: CardType;
  point?: number;
  customTag?: CardTagType;
  customTagString?: string;
  cardIds?: string[];
}

export interface ICardViewModel {
  id: string;
  cardType: CardType;
  cardInfo: ICardInfo;
  imageLoaded?: boolean;
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
  selectedCardInfo?: ISelectedCardInfo;
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
  isHandCard: boolean;
  handOrBoardCardId: string;
  cardType?: CardType;
  targetCardId?: string;
}

export interface IPlayCardDTO {
  handCardId: string;
  cardInfo: ICardInfo;
}

export enum SelectType {
  None,
  Hand,
  Board,
  BoardMulti,
}

export interface ISelectedCardInfo {
  card: ICardViewModel;
  fromHand: boolean;
  selectType?: SelectType;
  switchTo?: string;
}