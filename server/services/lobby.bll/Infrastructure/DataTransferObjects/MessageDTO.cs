﻿namespace lobby.bll.Infrastructure.DataTransferObjects
{
    public class MessageDTO
    {
        public required string Text { get; set; }
        public required Guid LobbyId { get; set; }
    }
}
