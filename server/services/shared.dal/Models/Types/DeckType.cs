﻿namespace shared.dal.Models.Types
{
    public enum DeckType
    {
        MyFirstMeal,
        SushiGo,
        PartySampler,
        MasterMenu,
        PointsPlatter,
        CutThroatCombo,
        BigBanquet,
        DinnerForTwo
    }

    public static class DeckExtensions
    {
        private static IDictionary<DeckType, int[]> DeckCards { get; } = new Dictionary<DeckType, int[]>()
        {
            { DeckType.MyFirstMeal, new int[] { 3, 12, 11, 10, 21, 20, 22 } },
            { DeckType.SushiGo, new int[] { 3, 12, 11, 6, 14, 21, 24 } },
            { DeckType.PartySampler, new int[] { 4, 12, 6, 13, 21, 15, 22 } },
            { DeckType.MasterMenu, new int[] { 4, 9, 13, 11, 17, 19, 23 } },
            { DeckType.PointsPlatter, new int[] { 5, 9, 6, 7, 18, 20, 22 } },
            { DeckType.CutThroatCombo, new int[] { 4, 8, 13, 10, 17, 16, 24 } },
            { DeckType.BigBanquet, new int[] { 3, 12, 6, 8, 17, 14, 22 } },
            { DeckType.DinnerForTwo, new int[] { 5, 9, 13, 10, 15, 18, 23 } }
        };
        private static IEnumerable<CardType> NigiriList { get; } = new List<CardType> { CardType.EggNigiri, CardType.SalmonNigiri, CardType.SquidNigiri };
        public static ICollection<CardType> GetCardTypes(this DeckType deckType)
        {
            return NigiriList.Concat(DeckCards[deckType].Select(d => (CardType)d)).ToList();
        }

        public static int GetMinPlayer(this DeckType deckType)
        {
            return deckType switch
            {
                DeckType.BigBanquet => 6,
                DeckType.MasterMenu or DeckType.PointsPlatter or DeckType.CutThroatCombo => 3,
                _ => 2,
            };
        }

        public static int GetMaxPlayer(this DeckType deckType)
        {
            return deckType switch
            {
                DeckType.DinnerForTwo => 2,
                DeckType.MyFirstMeal or DeckType.SushiGo => 5,
                DeckType.PartySampler or DeckType.PointsPlatter => 6,
                _ => 8,
            };
        }

        public static DeckTypeWrapper GetDeckTypeWrapper(this DeckType deck)
        {
            return deck switch
            {
                DeckType.MyFirstMeal => new MyFirstMeal(),
                DeckType.SushiGo => new SushiGo(),
                DeckType.PartySampler => new PartySampler(),
                DeckType.MasterMenu => new MasterMenu(),
                DeckType.PointsPlatter => new PointsPlatter(),
                DeckType.CutThroatCombo => new CutThroatCombo(),
                DeckType.BigBanquet => new BigBanquet(),
                DeckType.DinnerForTwo => new DinnerForTwo(),
                _ => throw new ArgumentOutOfRangeException(nameof(deck)),
            };
        }

        public static Dictionary<CardType, Queue<int>> GetShuffledPoints(this DeckType deck)
        {
            return deck.GetCardTypes().Where(x => x.HasPoints()).ToDictionary(x => x, x => x.GetShuffledPoints()!);
        }

        public static Queue<CardTypePoint> GetShuffledCards(this DeckType deckType, int round = 0, int count = 2)
        {
            var shuffledPoints = deckType.GetShuffledPoints();
            var cardList = new List<CardTypePoint>();
            foreach (var card in deckType.GetCardTypes())
            {
                for (int i = 0; i < card.SushiType().GetCount(round, count); i++)
                {
                    if (!shuffledPoints.ContainsKey(card) || shuffledPoints[card].Count == 0)
                    {
                        cardList.Add(new CardTypePoint { CardType = card });
                    }
                    else
                    {
                        cardList.Add(new CardTypePoint { CardType = card, Point = shuffledPoints[card].Dequeue() });
                    }
                }
            }
            return new Queue<CardTypePoint>(cardList.OrderBy(x => Guid.NewGuid()));
        }
    }
}
