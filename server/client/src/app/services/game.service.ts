import { Injectable, Injector } from '@angular/core';
import { BaseServiceService } from './abstract/base-service.service';
import { ICreateGameDTO, IGameViewModel } from 'src/shared/game.models';
import { BehaviorSubject } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ConfirmService } from './confirm.service';
import { LoadingService } from './loading.service';
import { TokenService } from './token.service';
import { CardService } from './card.service';

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
        const player = game.players.find((p) => this.tokenService.isOwnPlayer(p.id));
        console.log(player);
        
        if (player) {
          this.cardService.loadHand(player.handId);
        }
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
        this.client.delete(this.baseUrl);
      }
    });
  }

  public createGame(dto: ICreateGameDTO): void {
    this.client.post(`${this.baseUrl}/create`, dto);
  }

  public endTurn(): void {
    this.client.post(`${this.baseUrl}/end-turn`, {});
  }

  public endRound(): void {
    this.client.post(`${this.baseUrl}/end-round`, {});
  }

  public refreshGame(game: IGameViewModel): void {
    this._gameEventEmitter.next(game);
  }
}
