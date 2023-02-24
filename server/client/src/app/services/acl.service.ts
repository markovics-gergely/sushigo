import { Injectable } from '@angular/core';
import { ACL } from 'src/shared/acl.models';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class AclService {

  constructor(private tokenService: TokenService) {}

  public can(role: string): boolean {
    const roles = ACL[role];
    const userRoles = this.tokenService.user.roles;
    return roles && (roles.includes('*') || userRoles.some(role => roles.includes(role)));
  }

  public hasRoles(roles: string[]): boolean {
    const userRoles = this.tokenService.user.roles;
    return userRoles.some(role => roles.includes(role));
  }
}
