import { Component } from '@angular/core';
import { GameService } from 'src/app/game/services/game.service';
import { IGameViewModel, IPlayerViewModel, Phase, PhaseUtil } from 'src/shared/models/game.models';

@Component({
  selector: 'app-game-result',
  templateUrl: './game-result.component.html',
  styleUrls: ['./game-result.component.scss']
})
export class GameResultComponent {
  private _game: IGameViewModel | undefined;
  public get game(): IGameViewModel | undefined {
    return this._game;
  }
  constructor(private gameService: GameService) {
    gameService.gameEventEmitter.subscribe((game: IGameViewModel | undefined) => {
      this._game = game;
    });
  }

  get show() {
    return PhaseUtil.equals(this._game?.phase, Phase.Result);
  }

  get canRemoveGame() {
    return this.gameService.isFirst;
  }

  get players() {
    return this.game?.players.sort((a, b) => b.points - a.points) ?? [];
  }

  getPlacement(player: IPlayerViewModel) {
    return this.players.findIndex(p => p.points === player.points) + 1;
  }

  deleteGame() {
    this.gameService.removeGame();
  }
}
