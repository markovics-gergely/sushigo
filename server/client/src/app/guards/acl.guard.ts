import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { AclService } from '../services/acl.service';

export const AclGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
  ) => {
  const aclService = inject(AclService);
  const router = inject(Router);
  const routeName = route.data['name'];

  if (routeName.length === 0 || !aclService.can(routeName)) {
    router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
  return true;
}
