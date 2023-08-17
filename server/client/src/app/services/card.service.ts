import { Injectable } from '@angular/core';
import { BaseServiceService } from './abstract/base-service.service';
import { IHandViewModel, IPlayCardDTO } from 'src/shared/game.models';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CardService extends BaseServiceService {
  protected override readonly basePath: string = 'card';
  private _handEventEmitter = new BehaviorSubject<IHandViewModel | undefined>(undefined);

  public loadHand(handId: string): void {
    this.client
      .get<IHandViewModel>(`${this.baseUrl}/hand/${handId}`)
      .subscribe((hand: IHandViewModel) => {
        this._handEventEmitter.next(hand);
      });
  }

  public get handEventEmitter(): Observable<IHandViewModel | undefined> {
    return this._handEventEmitter;
  }

  public playCard(dto: IPlayCardDTO): void {
    this.client.post(this.baseUrl, dto);
  }
}
