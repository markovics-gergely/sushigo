import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { DeckType, IBuyDeckDTO, IDeckItemViewModel, IDeckViewModel } from 'src/shared/models/deck.models';
import { BaseServiceService } from '../../shared/services/abstract/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class ShopService extends BaseServiceService {
  protected override readonly basePath = 'shop';

  constructor(injector: Injector) { super(injector); }

  public get decks(): Observable<IDeckViewModel[]> {
    return this.client.get<IDeckViewModel[]>(`${this.baseUrl}`);
  }

  public getDeck(deckType: DeckType): Observable<IDeckItemViewModel> {
    return this.client.get<IDeckItemViewModel>(`${this.baseUrl}/${deckType}`);
  }

  public claimParty(): Observable<Object> {
    return this.client.post(`${this.baseUrl}/party`, {});
  }

  public claimDeck(buyDeckDTO: IBuyDeckDTO): Observable<Object> {
    return this.client.post(`${this.baseUrl}/deck`, buyDeckDTO);
  }
}
