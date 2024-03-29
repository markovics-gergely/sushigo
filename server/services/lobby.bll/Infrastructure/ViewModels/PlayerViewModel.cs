﻿namespace lobby.bll.Infrastructure.ViewModels
{
    public class PlayerViewModel
    {
        public required Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string UserId { get; set; }
        public string? ImagePath { get; set; }
        public bool Ready { get; set; }
    }
}
