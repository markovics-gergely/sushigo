import { ProviderToken, inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn } from '@angular/router';
import { FriendHubService } from '../hubs/friend-hub.service';
import { HubService } from '../services/abstract/hub.service';
import { LobbyHubService } from '../../app/lobby/hubs/lobby-hub.service';
import { LobbyListHubService } from '../hubs/lobby-list-hub.service';
import { GameHubService } from '../../app/game/hubs/game-hub.service';
import { HandHubService } from 'src/app/game/hubs/hand-hub.service';

const hubs = {
  friend: FriendHubService,
  lobby: LobbyHubService,
  lobbyList: LobbyListHubService,
  game: GameHubService,
  hand: HandHubService,
} as Record<string, ProviderToken<HubService>>;

export const HubGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const hubNames = route.data['hub'];
  Object.entries(hubs).forEach(([name, hubProvider]) => {
    const hub = inject(hubProvider);
    if (hubNames.includes(name)) {
      hub.startConnection();
    } else {
      hub.stopConnection();
    }
  });
  return true;
};
