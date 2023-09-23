import { Injectable, Injector } from '@angular/core';
import { BaseServiceService } from './abstract/base-service.service';
import { ICreateGameDTO, IGameViewModel, Phase, PhaseUtil } from 'src/shared/game.models';
import { BehaviorSubject, Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ConfirmService } from './confirm.service';
import { LoadingService } from './loading.service';
import { TokenService } from './token.service';
import { CardService } from './card.service';
import { DeckType } from 'src/shared/deck.models';

@Injectable({
  providedIn: 'root'
})
export class GameService extends BaseServiceService {
  protected override readonly basePath: string = 'game';

  private _gameEventEmitter = new BehaviorSubject<IGameViewModel | undefined>(undefined);

  constructor(
    injector: Injector,
    private dialog: MatDialog,
    private loadingService: LoadingService,
    private confirmService: ConfirmService,
    private tokenService: TokenService,
    private cardService: CardService,
    private router: Router
  ) { super(injector); }

  public loadGame(): void {
    this.loadingService.start();
    this._gameEventEmitter.next(undefined);
    this.client
      .get<IGameViewModel>(this.baseUrl)
      .subscribe((game: IGameViewModel) => {
        this._gameEventEmitter.next(game);
        this.cardService.loadHand();
      }).add(() => {
        this.loadingService.stop();
      });
  }

  public get gameEventEmitter(): BehaviorSubject<IGameViewModel | undefined> {
    return this._gameEventEmitter;
  }

  public removeGame(): void {
    this.confirmService.confirm('game.delete').subscribe((result: boolean) => {
      if (result) {
        this.loadingService.start();
        this.client.delete(this.baseUrl).subscribe({
        }).add(() => {
          this.loadingService.stop();
        });
      }
    });
  }

  public createGame(dto: ICreateGameDTO): Observable<void> {
    return this.client.post<void>(`${this.baseUrl}/create`, dto);
  }

  public endTurn(): void {
    this.client.post(`${this.baseUrl}/end-turn`, {});
  }

  public endRound(): void {
    this.client.post(`${this.baseUrl}/end-round`, {});
  }

  public refreshGame(game: IGameViewModel): void {
    this._gameEventEmitter.next(game);
    this.cardService.refreshHand();
  }

  public proceedEndTurn(): Observable<void> {
    return this.client.post<void>(`${this.baseUrl}/end-turn`, {});
  }

  public proceedEndRound(): Observable<void> {
    return this.client.post<void>(`${this.baseUrl}/end-round`, {});
  }

  public proceedEndGame(): Observable<void> {
    return this.client.post<void>(`${this.baseUrl}/end-game`, {});
  }

  public get canEndTurn(): boolean {
    return this.tokenService.isOwnPlayer(this._gameEventEmitter.value?.actualPlayerId) && PhaseUtil.equals(this._gameEventEmitter.value?.phase, Phase.EndTurn);
  }

  public get canEndRound(): boolean {
    return this.tokenService.isOwnPlayer(this._gameEventEmitter.value?.actualPlayerId) && PhaseUtil.equals(this._gameEventEmitter.value?.phase, Phase.EndRound);
  }

  public get canEndGame(): boolean {
    return this.tokenService.isOwnPlayer(this._gameEventEmitter.value?.actualPlayerId) && PhaseUtil.equals(this._gameEventEmitter.value?.phase, Phase.EndGame);
  }

  public get canPlayCard(): boolean {
    return this.tokenService.isOwnPlayer(this._gameEventEmitter.value?.actualPlayerId) && PhaseUtil.equals(this._gameEventEmitter.value?.phase, Phase.Turn);
  }

  public get canPlayAfterCard(): boolean {
    return this.tokenService.isOwnPlayer(this._gameEventEmitter.value?.actualPlayerId) && PhaseUtil.equals(this._gameEventEmitter.value?.phase, Phase.AfterTurn);
  }

  public get isOver(): boolean {
    return PhaseUtil.equals(this._gameEventEmitter.value?.phase, Phase.Result);
  }

  public get deckType(): DeckType {
    return this._gameEventEmitter.value?.deckType ?? DeckType.SushiGo;
  }

  public get isFirst(): boolean {
    return this.tokenService.isOwnPlayer(this._gameEventEmitter.value?.actualPlayerId);
  }
}
