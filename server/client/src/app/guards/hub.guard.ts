import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, RouterStateSnapshot } from '@angular/router';
import { FriendHubService } from '../services/hubs/friend-hub.service';
import { HubService } from '../services/hubs/abstract/hub.service';
import { LobbyHubService } from '../services/hubs/lobby-hub.service';
import { LobbyListHubService } from '../services/hubs/lobby-list-hub.service';

export const HubGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
  ) => {
  const hubs = {
    friend: inject(FriendHubService),
    lobby: inject(LobbyHubService),
    lobbyList: inject(LobbyListHubService)
  } as Record<string, HubService>;
  const hubNames = route.data['hub'];
  Object.entries(hubs).forEach(([name, hub]) => {
    if (hubNames.includes(name)) {
      hub.startConnection();
    } else {
      hub.stopConnection();
    }
  });
  return true;
}
