import { Injectable } from '@angular/core';
import { ACL, AclPage } from 'src/shared/models/acl.models';
import { AppRole } from 'src/shared/models/user.models';
import { TokenService } from '../../app/services/token.service';
import { DeckType } from 'src/shared/models/deck.models';

@Injectable({
  providedIn: 'root'
})
export class AclService {
  private roles: AppRole[] = [];

  constructor(private tokenService: TokenService) {
    this.tokenService.userEventEmitter.subscribe((user) => {
      const values = user?.role ?? [];
      this.roles = Array.isArray(values) ? values : [values];
    });
  }

  public can(role: AclPage | '*'): boolean {
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

  public hasRoles(roles: (AppRole | '*' | '!')[]): boolean {
    const has = this.roles.some(role => roles.includes(role));
    return roles.includes('!') ? !has : has;
  }
}
