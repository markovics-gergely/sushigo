﻿using shared.dal.Models;
using System.Collections;

namespace game.dal.Domain
{
    public class Deck
    {
        public Guid Id { get; set; }
        public DeckType DeckType { get; set; }
        public Game Game { get; set; } = null!;
        public ICollection<CardType> Cards { get; set; } = new List<CardType>();
        public Dictionary<CardType, string> AdditionalInfo { get; set; } = new Dictionary<CardType, string>();

        public void AddDeckInfo()
        {
            var cardTypes = DeckType.GetCardTypes();
            if (cardTypes.Contains(CardType.MakiRoll))
            {
                var points = string.Join(',', new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3 }.OrderBy(x => Guid.NewGuid()).ToList());
                AdditionalInfo[CardType.MakiRoll] = points;
            }
            if (cardTypes.Contains(CardType.Uramaki))
            {
                var points = string.Join(',', new int[] { 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5 }.OrderBy(x => Guid.NewGuid()).ToList());
                AdditionalInfo[CardType.Uramaki] = points;
            }
            if (cardTypes.Contains(CardType.Onigiri))
            {
                var points = string.Join(',', new int[] { 0, 0, 1, 1, 2, 2, 3, 3 }.OrderBy(x => Guid.NewGuid()).ToList());
                AdditionalInfo[CardType.Onigiri] = points;
            }
            if (cardTypes.Contains(CardType.Fruit))
            {
                var points = string.Join(',', new string[] { "200", "200", "020", "020", "002", "002", "110", "110", "110", "011", "011", "011", "101", "101", "101" }.OrderBy(x => Guid.NewGuid()).ToList());
                AdditionalInfo[CardType.Fruit] = points;
            }
        }

        public int? PopInfoItem(CardType cardType)
        {
            if (!AdditionalInfo.ContainsKey(cardType)) return null;

            var pointList = new Queue<string>(AdditionalInfo[cardType].Split(','));
            var item = pointList.Dequeue();
            AdditionalInfo[cardType] = string.Join(',', pointList.ToArray());
            return int.Parse(item);
        }
    }
}