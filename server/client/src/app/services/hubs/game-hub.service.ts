import { Injectable, Injector } from '@angular/core';
import { HubService } from './abstract/hub.service';
import { GameService } from '../game.service';
import { environment } from 'src/environments/environment';
import { IGameViewModel } from 'src/shared/game.models';
import { Router } from '@angular/router';
import { UserService } from '../user.service';
import { SnackService } from '../snack.service';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class GameHubService extends HubService {
  protected override readonly baseUrl: string = `${environment.baseUrl}/game-hubs/game-hub`;

  constructor(injector: Injector, private gameService: GameService, private snackService: SnackService, private translateService: TranslateService) {
    super(injector);
  }

  protected override addListeners(): void {
    this.hubConnection?.on('RefreshGame', (game: IGameViewModel) => {
      this.gameService.refreshGame(game);
    });
    this.hubConnection?.on('EndTurn', () => {
      
    });
    this.hubConnection?.on('EndRound', () => {
      
    });
    this.hubConnection?.on('RemoveGame', () => {
      this.snackService.openSnackBar(this.translateService.instant('game.over'));
      this.gameService.gameEventEmitter.next(undefined);
    });
  }

  protected override onHubConnected?(): void {
    this.gameService.loadGame();
  }

  protected override onStartConnection?(): void {}
}
