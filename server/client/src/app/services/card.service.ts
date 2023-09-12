import { Injectable } from '@angular/core';
import { BaseServiceService } from './abstract/base-service.service';
import { IHandViewModel, IPlayAfterTurnDTO, IPlayCardDTO } from 'src/shared/game.models';
import { BehaviorSubject, Observable, map } from 'rxjs';

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
