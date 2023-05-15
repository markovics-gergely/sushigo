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

function isActiveLobby(route: ActivatedRouteSnapshot, router: Router) {
  const tokenService = inject(TokenService);
  const aclService = inject(AclService);
  const lobbyId = route.params['id'];
  if (lobbyId !== tokenService.user?.lobby) {
    router.navigate(['lobby', tokenService.user?.lobby]);
    return false;
  }
  if (!aclService.can('lobby')) {
    router.navigate(['home']);
    return false;
  }
  return true;
}

export const AclGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
  ) => {
  const aclService = inject(AclService);
  const tokenService = inject(TokenService);
  const router = inject(Router);
  const routeName = route.data['name'] as AclPage;  
  if (!routeName) return true;
  console.log(route.params['id']);
  
  if (routeName === 'login' || routeName === 'register') {
    if (tokenService.loggedIn) {
      router.navigateByUrl(route.queryParams['returnUrl'] || 'home');
    }
    return !tokenService.loggedIn;
  }
  if (tokenService.user?.lobby) {
    return isActiveLobby(route, router);
  }
  if (!aclService.can(routeName)) {
    router.navigate(['login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
  return true;
}

