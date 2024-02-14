import { Injectable } from '@angular/core';
import { isEqual } from 'lodash';
import { BehaviorSubject, Observable } from 'rxjs';
import { BaseServiceService } from 'src/shared/services/abstract/base-service.service';
import { IHandViewModel, IPlayAfterTurnDTO, IPlayCardDTO, ISelectedCardInfo, SelectType } from '../models/game.models';

@Injectable({
  providedIn: 'root'
})
export class HandService extends BaseServiceService {
  protected override readonly basePath: string = 'card';
  private _handEventEmitter = new BehaviorSubject<IHandViewModel | undefined>(undefined);
  private _selectedCardEventEmitter = new BehaviorSubject<ISelectedCardInfo | undefined>(undefined);

  public loadHand(): void {
    this.client
      .get<IHandViewModel>(`${this.baseUrl}/hand`)
      .subscribe((hand: IHandViewModel) => {
        this._handEventEmitter.next(hand);
      });
  }


  public refreshHand(hand: IHandViewModel): void {
    if (!isEqual(hand, this._handEventEmitter.value)) {
      this._handEventEmitter.next(hand);
    }
  }

  public get handEventEmitter(): Observable<IHandViewModel | undefined> {
    return this._handEventEmitter;
  }

  public get selectedCardEventEmitter(): Observable<ISelectedCardInfo | undefined> {
    return this._selectedCardEventEmitter;
  }

  public get selectedCard(): ISelectedCardInfo | undefined {
    return this._selectedCardEventEmitter.value;
  }

  public isSelectedInTypes(types: SelectType[]): boolean {
    const selected = this._selectedCardEventEmitter.value;
    return Boolean(selected?.selectType && types.includes(selected.selectType));
  }

  public selectCard(info: ISelectedCardInfo | undefined): void {
    this._selectedCardEventEmitter.next(info);
  }

  public playCard(dto: IPlayCardDTO): Observable<void> {
    this._selectedCardEventEmitter.next(undefined);
    return this.client.post<void>(this.baseUrl, dto);
  }

  public playAfterTurn(dto: IPlayAfterTurnDTO): Observable<void> {
    return this.client.post<void>(`${this.baseUrl}/after-turn`, dto);
  }

  public skipAfterTurn(): Observable<void> {
    return this.client.post<void>(`${this.baseUrl}/skip-after`, {});
  }
}
