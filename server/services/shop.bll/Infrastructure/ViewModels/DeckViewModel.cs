﻿using shared.Models;

namespace shop.bll.Infrastructure.ViewModels
{
    public class DeckViewModel
    {
        public DeckType DeckType { get; set; }
        public long Cost { get; set; }
        public bool Claimed { get; set; }
        public IEnumerable<CardType> CardTypes { get; set; } = Enumerable.Empty<CardType>();
    }
}