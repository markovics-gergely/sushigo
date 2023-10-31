import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { TokenService } from '../services/token.service';

export const LoginGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const tokenService = inject(TokenService);
  const router = inject(Router);
  if (tokenService.loggedIn) {
    return router.parseUrl(route.queryParams['returnUrl'] || 'home');
  }
  return true;
};
