import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { AclService } from '../services/acl.service';
import { TokenService } from '../services/token.service';

export const AclGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
  ) => {
  const aclService = inject(AclService);
  const tokenService = inject(TokenService);
  const router = inject(Router);
  const routeName = route.data['name'];  
  if (!routeName) return true;
  if (routeName === 'login' || routeName === 'register') {
    if (tokenService.loggedIn) {
      router.navigateByUrl(route.queryParams['returnUrl'] || 'home');
    }
    return !tokenService.loggedIn;
  }
  if (!aclService.can(routeName)) {
    router.navigate(['login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
  return true;
}
