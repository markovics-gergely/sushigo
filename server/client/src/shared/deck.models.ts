export interface IDeckViewModel {
  deckType: DeckType;
  cost: number;
  claimed?: boolean;
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

export enum SushiType {
  Nigiri,
  Roll,
  Appetizer,
  Special,
  Dessert
}