import { Injectable, Injector } from '@angular/core';
import { BehaviorSubject, Observable, of, switchMap } from 'rxjs';
import { ConfirmService } from 'src/shared/services/confirm.service';
import { LoadingService } from 'src/shared/services/loading.service';
import { TokenService } from 'src/app/services/token.service';
import { DeckType } from 'src/shared/models/deck.models';
import { IGameViewModel, ICreateGameDTO, Phase, IPlayerViewModel, IBoardViewModel } from 'src/shared/models/game.models';
import { BaseServiceService } from 'src/shared/services/abstract/base-service.service';
import { HandService } from './hand.service';

@Injectable({
  providedIn: 'root'
})
export class GameService extends BaseServiceService {
  protected override readonly basePath: string = 'game';

  private _gameEventEmitter = new BehaviorSubject<IGameViewModel | undefined>(undefined);
  public get gameEventEmitter(): Observable<IGameViewModel | undefined> {
    return this._gameEventEmitter;
  }

  private _gameCountEventEmitter = new BehaviorSubject<number>(-1);
  public get gameCountEventEmitter(): Observable<number> {
    return this._gameCountEventEmitter;
  }

  constructor(
    injector: Injector,
    private loadingService: LoadingService,
    private confirmService: ConfirmService,
    private tokenService: TokenService,
    private handService: HandService,
  ) { super(injector); }

  public loadGame(): void {
    this.loadingService.start();
    this._gameEventEmitter.next(undefined);
    this.client
      .get<IGameViewModel>(this.baseUrl)
      .subscribe((game: IGameViewModel) => {
        console.log(game);
        
        this._gameEventEmitter.next(game);
        this._gameCountEventEmitter.next(-1);
      }).add(() => {
        this.loadingService.stop();
      });
  }

  public getGame(): Observable<IGameViewModel | undefined> {
    this.loadingService.start();
    return this.client.get<IGameViewModel>(this.baseUrl).pipe(
      switchMap((game: IGameViewModel) => {
        this._gameEventEmitter.next(game);
        this._gameCountEventEmitter.next(-1);
        this.loadingService.stop();
        return of(game);
      }));
  }

  private gameCounterFunction(): void {
    setTimeout(() => {
      if (this._gameCountEventEmitter.value > 0) {
        this._gameCountEventEmitter.next(this._gameCountEventEmitter.value - 1);
        this.gameCounterFunction();
      }
    }, 1000);
  }

  public refreshCounter(count: number = 30): void {
    this._gameCountEventEmitter.next(count);
    this.gameCounterFunction();
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
    this._gameCountEventEmitter.next(-1);
    this.handService.loadHand();
  }

  public proceedEndTurn(): Observable<void> {
    this._gameCountEventEmitter.next(-1);
    return this.client.post<void>(`${this.baseUrl}/end-turn`, {});
  }

  public proceedEndRound(): Observable<void> {
    this._gameCountEventEmitter.next(-1);
    return this.client.post<void>(`${this.baseUrl}/end-round`, {});
  }

  public proceedEndGame(): Observable<void> {
    return this.client.post<void>(`${this.baseUrl}/end-game`, {});
  }

  public get deckType(): DeckType {
    return this._gameEventEmitter.value?.deckType ?? DeckType.SushiGo;
  }

  public get board(): IBoardViewModel | undefined {
    return this._gameEventEmitter.value?.players.find(
      (player) => this.tokenService.ownPlayer(player.id)
    )?.board;
  }

  public get phase(): Phase {
    return this._gameEventEmitter.value?.phase ?? Phase.None;
  }

  public get isFirst(): boolean {
    return this.tokenService.ownPlayer(this._gameEventEmitter.value?.actualPlayerId);
  }

  public get ownPlayer(): IPlayerViewModel | undefined {
    return this._gameEventEmitter.value?.players.find(p => p.id === this.tokenService.player);
  }
}
