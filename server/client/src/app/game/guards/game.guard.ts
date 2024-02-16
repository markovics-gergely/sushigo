import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { TokenService } from '../../services/token.service';
import { GameService } from '../services/game.service';
import { map } from 'rxjs';

export const gameGuard: CanActivateFn = () => {
  const tokenService = inject(TokenService);
  const gameService = inject(GameService);
  const router = inject(Router);
  if (!tokenService.game) {
    return router.parseUrl('/home');
  }
  return true;
  /*return gameService.getGame().pipe(map((game) => {
    if (game) {
      return true;
    }
    return router.parseUrl('/home');
  }));*/
};
