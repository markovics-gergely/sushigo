﻿namespace shared.dal.Models
{
    public class LobbyJoinedDTO
    {
        public Guid? LobbyId { get; set; }
        public Guid UserId { get; set; }
    }
}
