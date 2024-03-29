﻿using lobby.bll.Infrastructure.ViewModels;
using MediatR;

namespace lobby.bll.Infrastructure.Events
{
    public class AddLobbyEvent : INotification
    {
        public LobbyViewModel Lobby { get; init; }
        public AddLobbyEvent(LobbyViewModel lobby)
        {
            Lobby = lobby;
        }
    }
}
