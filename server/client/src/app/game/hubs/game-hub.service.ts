import { Injectable, Injector } from '@angular/core';
import { HubService } from '../../../shared/services/abstract/hub.service';
import { environment } from 'src/environments/environment';
import { IGameViewModel } from 'src/shared/models/game.models';
import { SnackService } from '../../../shared/services/snack.service';
import { TranslateService } from '@ngx-translate/core';
import { GameService } from 'src/app/game/services/game.service';

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
      this.gameService.refreshCounter();
    });
    this.hubConnection?.on('EndRound', () => {
      this.gameService.refreshCounter();
    });
    this.hubConnection?.on('RemoveGame', () => {
      this.snackService.openSnackBar(this.translateService.instant('game.over'));
      this.gameService.loadGame();
    });
  }

  protected override onHubConnected?(): void {
    this.gameService.loadGame();
  }

  protected override onStartConnection?(): void {}
}
