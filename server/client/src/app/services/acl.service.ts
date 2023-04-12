import { Injectable } from '@angular/core';
import { ACL } from 'src/shared/acl.models';
import { AppRole } from 'src/shared/user.models';
import { TokenService } from './token.service';
import { DeckType } from 'src/shared/deck.models';

@Injectable({
  providedIn: 'root'
})
export class AclService {

  constructor(private tokenService: TokenService) { }

  public can(role: AppRole | '*'): boolean {
    const roles = ACL[role];
    const userRoles = this.tokenService.user?.role ?? [];
    return roles && (roles.includes('*') || [userRoles].flat().some(role => roles.includes(role)));
  }

  public hasDeck(deck: DeckType): boolean {
    const userDecks = this.tokenService.user?.decks ?? [];
    return userDecks.includes(deck);
  }

  public emptyDecks(): boolean {
    const userDecks = this.tokenService.user?.decks ?? [];
    return userDecks.length === 0;
  }

  public hasRoles(roles: (AppRole | '*')[]): boolean {
    const userRoles = this.tokenService.user?.role ?? [];
    return [userRoles].flat().some(role => roles.includes(role));
  }
}
