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
    if (!roles || roles.length === 0 || roles.includes('*')) {
      return true;
    }
    return this.hasRoles(roles);
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
    return this.tokenService.roles.some(role => roles.includes(role));
  }
}
