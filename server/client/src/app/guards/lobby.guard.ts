import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { TokenService } from '../services/token.service';

export const LobbyGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  ) => {
  const tokenService = inject(TokenService);
  const router = inject(Router);
  const lobbyId: string | undefined = route.params['id'];
  if (lobbyId && lobbyId !== tokenService.lobby) {
    return router.parseUrl('/lobby');
  }
  return true;
}
