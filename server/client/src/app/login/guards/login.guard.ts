import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { TokenService } from 'src/app/services/token.service';

export const loginGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const tokenService = inject(TokenService);
  const router = inject(Router);
  if (tokenService.loggedIn) {
    return router.parseUrl(route.queryParams['returnUrl'] || 'home');
  }
  return true;
};
