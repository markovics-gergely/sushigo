import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IBuyDeckDTO, IDeckViewModel } from 'src/shared/deck.models';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  /** Route of the shop related endpoints */
  private readonly baseUrl: string = `${environment.baseUrl}/shop`;

  constructor(private client: HttpClient) { }

  public get decks(): Observable<IDeckViewModel[]> {
    return this.client.get<IDeckViewModel[]>(`${this.baseUrl}`);
  }

  public claimParty(): Observable<Object> {
    return this.client.post(`${this.baseUrl}/party`, {});
  }

  public claimDeck(buyDeckDTO: IBuyDeckDTO): Observable<Object> {
    return this.client.post(`${this.baseUrl}/deck`, buyDeckDTO);
  }
}
