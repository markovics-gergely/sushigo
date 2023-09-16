import { Injectable, Injector } from '@angular/core';
import { HubService } from './abstract/hub.service';
import { GameService } from '../game.service';
import { environment } from 'src/environments/environment';
import { IGameViewModel } from 'src/shared/game.models';

@Injectable({
  providedIn: 'root'
})
export class GameHubService extends HubService {
  protected override readonly baseUrl: string = `${environment.baseUrl}/game-hubs/game-hub`;

  constructor(injector: Injector, private gameService: GameService) {
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
      console.log('RemoveGame');
      
    });
  }

  protected override onHubConnected?(): void {
    this.gameService.loadGame();
  }

  protected override onStartConnection?(): void {}
}
