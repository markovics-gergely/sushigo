import { Injectable } from '@angular/core';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AclService } from '../services/acl.service';
import { TokenService } from '../services/token.service';

@Injectable({
  providedIn: 'root',
})
export class AclGuard implements CanActivate {
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private tokenService: TokenService,
    private aclService: AclService
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    const currentUser = this.tokenService.user;
    const routeName = next.data['name'];
    if (routeName.length === 0 || !this.aclService.can(routeName)) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }
    return true;
  }
}
