export interface IDeckViewModel {
  deckType: DeckType;
  cost: number;
  claimed?: boolean;
  imagePath: string;
  imageLoaded?: boolean;
  minPlayer: number;
  maxPlayer: number;
  cardTypes: CardType[];
}

export interface IDeckItemViewModel {
  deckType: DeckType;
  imagePath: string;
  imageLoaded?: boolean;
  minPlayer: number;
  maxPlayer: number;
  cardTypes: CardType[];
}

export interface IBuyDeckDTO {
  deckType: DeckType;
}

export enum DeckType {
  MyFirstMeal,
  SushiGo,
  PartySampler,
  MasterMenu,
  PointsPlatter,
  CutThroatCombo,
  BigBanquet,
  DinnerForTwo
}

export enum CardType {
  EggNigiri,
  SalmonNigiri,
  SquidNigiri,
  MakiRoll,
  Temaki,
  Uramaki,
  Dumpling,
  Edamame,
  Eel,
  Onigiri,
  MisoSoup,
  Sashimi,
  Tempura,
  Tofu,
  Chopsticks,
  Menu,
  SoySauce,
  Spoon,
  SpecialOrder,
  TakeoutBox,
  Tea,
  Wasabi,
  GreenTeaIceCream,
  Fruit,
  Pudding,
}
export enum SelectType {
  None,
  OwnHandCard,
  OwnBoardCard,
  OwnBoardCards,
  CardType,
}

const isNumeric = (value: string): boolean => !new RegExp(/[^\d]/g).test(value.trim());

export class CardTypeUtil {
  public static getSelectType(cardType: CardType): SelectType {
    switch (CardType[cardType.toString() as keyof typeof CardType]) {
      case CardType.SpecialOrder:
        return SelectType.OwnBoardCard;
      case CardType.TakeoutBox:
        return SelectType.OwnBoardCards;
      default:
        return SelectType.None;
    }
  }
  public static getSelectTypeAfterPlay(cardType: CardType): SelectType {
    switch (CardTypeUtil.getString(cardType)) {
      case CardTypeUtil.getString(CardType.Chopsticks):
        return SelectType.OwnHandCard;
      case CardTypeUtil.getString(CardType.Spoon):
        return SelectType.CardType;
      default:
        return SelectType.None;
    }
  }
  public static getString(cardType: number | string): string {
    if (typeof cardType === 'number') {
      return CardType[cardType];
    } else if (isNumeric(cardType)) {
      return CardType[parseInt(cardType)];
    }
    return cardType;
  }
  public static hasAfterTurn(cardType: CardType): boolean {
    switch (CardType[CardTypeUtil.getString(cardType) as keyof typeof CardType]) {
      case CardType.Chopsticks:
      case CardType.Spoon:
        return true;
      default:
        return false;
    }
  }
  public static equals(cardType: CardType, cardType2: CardType): boolean {
    return CardTypeUtil.getString(cardType) === CardTypeUtil.getString(cardType2);
  }
}

export class DeckTypeUtil {
  public static getString(deckType: number | string): string {
    if (typeof deckType === 'number') {
      return DeckType[deckType];
    } else if (isNumeric(deckType)) {
      return DeckType[parseInt(deckType)];
    }
    return deckType;
  }
  public static equals(deckType: DeckType, deckType2: DeckType): boolean {
    return DeckTypeUtil.getString(deckType) === DeckTypeUtil.getString(deckType2);
  }
}

export enum SushiType {
  Nigiri,
  Roll,
  Appetizer,
  Special,
  Dessert
}