﻿namespace lobby.dal.Domain
{
    public class Player
    {
        public required Guid Id { get; set; }
        public required string UserName { get; set; }
        public string? ImagePath { get; set; }
        public required Guid LobbyId { get; set; }
        public required Lobby Lobby { get; set; }
    }
}
