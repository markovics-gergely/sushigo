import { Component, OnInit } from '@angular/core';
import { AclService } from 'src/app/services/acl.service';
import { LoadingService } from 'src/app/services/loading.service';
import { ShopService } from 'src/app/services/shop.service';
import { TokenService } from 'src/app/services/token.service';
import { IDeckViewModel } from 'src/shared/deck.models';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.scss'],
})
export class StoreComponent implements OnInit {
  private _decks: IDeckViewModel[] = [];
  private readonly HEIGHT: number = 4;
  private readonly ADDED_HEIGHT: number = 3;

  constructor(
    private shopService: ShopService,
    private tokenService: TokenService,
    private loadingService: LoadingService,
    private aclService: AclService
  ) {}

  ngOnInit(): void {
    this.shopService.decks.subscribe((decks) => (this._decks = decks));
  }

  public get decks(): IDeckViewModel[] {
    return this._decks;
  }

  public calcr(scale: number): number {
    return (
      Math.ceil((1.0 / scale) * this._decks.length) * this.HEIGHT +
      this.ADDED_HEIGHT
    );
  }

  public get height(): number {
    return this.HEIGHT;
  }

  public canBuy(deck: IDeckViewModel): boolean {
    return !this.tokenService.decks.includes(deck.deckType) && this.aclService.hasRoles(['CanClaimDeck']);
  }

  public getButtonText(deck: IDeckViewModel): string {
    if (this.tokenService.decks.includes(deck.deckType)) {
      return 'shop.owned';
    }
    if (!this.aclService.hasRoles(['CanClaimDeck'])) {
      return 'shop.lowexp';
    }
    return `shop.buy`;
  }

  public buy(deck: IDeckViewModel): void {
    this.loadingService.loading = true;
    this.shopService
      .claimDeck({ deckType: deck.deckType })
      .subscribe({
        error: (err) => {
          console.error(err);
        },
      })
      .add(() => (this.loadingService.loading = false));
  }
}
