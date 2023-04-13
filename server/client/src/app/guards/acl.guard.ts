import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { AclService } from '../services/acl.service';
import { TokenService } from '../services/token.service';
import { FriendHubService } from '../services/friend-hub.service';

export const AclGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
  ) => {
  const aclService = inject(AclService);
  const tokenService = inject(TokenService);
  const friendHubService = inject(FriendHubService);
  const router = inject(Router);
  const routeName = route.data['name'];
  if (!routeName) {
    return true;
  }
  if (routeName === 'login') {
    if (tokenService.loggedIn) {
      router.navigate(['home']);
      return false;
    } else {
      friendHubService.stopConnection();
      return true;
    }
  }
  if (!aclService.can(routeName)) {
    friendHubService.stopConnection();
    router.navigate(['login'], { queryParams: { returnUrl: state.url } });
    return false;
  } else {
    friendHubService.startConnection();
  }
  return true;
}
