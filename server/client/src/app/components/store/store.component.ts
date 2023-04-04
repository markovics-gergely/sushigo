import { Component, OnInit } from '@angular/core';
import { ShopService } from 'src/app/services/shop.service';
import { IDeckViewModel } from 'src/shared/deck.models';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.scss'],
})
export class StoreComponent implements OnInit {
  private _decks: IDeckViewModel[] = [];

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.shopService.decks.subscribe((decks) => (this._decks = decks));
  }

  public get decks(): IDeckViewModel[] {
    return this._decks;
  }

  public calcRow(scale: number): number {
    return (1 / scale) * this._decks.length * 3 + 1;
  }
}
