﻿using shared.Models;

namespace shop.bll.Infrastructure.ViewModels
{
    public class DeckItemViewModel
    {
        public DeckType DeckType { get; set; }
        public required string ImagePath { get; set; }
        public int MinPlayer { get; set; }
        public int MaxPlayer { get; set; }
    }
}