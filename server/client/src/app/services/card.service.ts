import { Injectable } from '@angular/core';
import { BaseServiceService } from './abstract/base-service.service';
import { IHandViewModel, IPlayAfterTurnDTO, IPlayCardDTO } from 'src/shared/game.models';
import { BehaviorSubject, Observable } from 'rxjs';
import { isEqual } from 'lodash';

@Injectable({
  providedIn: 'root'
})
export class CardService extends BaseServiceService {
  protected override readonly basePath: string = 'card';
  private _handEventEmitter = new BehaviorSubject<IHandViewModel | undefined>(undefined);

  public loadHand(): void {
    this.client
      .get<IHandViewModel>(`${this.baseUrl}/hand`)
      .subscribe((hand: IHandViewModel) => {
        this._handEventEmitter.next(hand);
      });
  }


  public refreshHand(): void {
    this.client
      .get<IHandViewModel>(`${this.baseUrl}/hand`)
      .subscribe((hand: IHandViewModel) => {
        const map = new Map(hand.cards.map((card) => [card.id, card]));
        this._handEventEmitter.value?.cards.forEach((card) => {
          const newCard = map.get(card.id);
          if (newCard) {
            card.isSelected = newCard.isSelected;
          }
        });
        if (!isEqual(hand, this._handEventEmitter.value)) {
          this._handEventEmitter.next(hand);
        }
      });
  }

  public get handEventEmitter(): Observable<IHandViewModel | undefined> {
    return this._handEventEmitter;
  }

  public playCard(dto: IPlayCardDTO): Observable<void> {
    return this.client.post<void>(this.baseUrl, dto);
  }

  public playAfterTurn(dto: IPlayAfterTurnDTO): Observable<void> {
    return this.client.post<void>(`${this.baseUrl}/after-turn`, dto);
  }

  public skipAfterTurn(): Observable<void> {
    return this.client.post<void>(`${this.baseUrl}/skip-after`, {});
  }
}
