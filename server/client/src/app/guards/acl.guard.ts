import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { AclService } from '../services/acl.service';
import { TokenService } from '../services/token.service';
import { AclPage } from 'src/shared/acl.models';

export const AclGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
  ) => {
  const aclService = inject(AclService);
  const tokenService = inject(TokenService);
  const router = inject(Router);
  const routeName = route.data['name'] as AclPage;
  if (!routeName) return true;
  if (tokenService.lobby) {
    if (routeName === 'lobby' && route.params['id'] === tokenService.lobby) {
      return true;
    }
    return router.parseUrl(`/lobby/${tokenService.lobby}`);
  }
  if (!aclService.can(routeName)) {
    const url = router.parseUrl('login');
    url.queryParams = { returnUrl: state.url };
    return url;
  }
  return true;
}

