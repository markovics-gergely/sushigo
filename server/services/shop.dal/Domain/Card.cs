﻿using shared.dal.Models.Types;

namespace shop.dal.Domain
{
    public class Card
    {
        public Guid Id { get; set; }
        public CardType Type { get; set; }
        public SushiType SushiType { get; set; }
        public required string ImagePath { get; set; }
    }
}
