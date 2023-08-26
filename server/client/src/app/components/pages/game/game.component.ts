import { Component } from '@angular/core';
import { GameService } from 'src/app/services/game.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { DeckType } from 'src/shared/deck.models';
import { IBoardViewModel, IGameViewModel, Phase } from 'src/shared/game.models';
import { IUserViewModel } from 'src/shared/user.models';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss'],
})
export class GameComponent {
  private _game: IGameViewModel | undefined = {
    id: '',
    name: '',
    players: Array.from({ length: 10 }, () => ({
      id: '',
      userId: '',
      userName: 'name',
      points: 0,
      board: { cards: [] },
      handId: '',
    })),
    deckType: DeckType.SushiGo,
    round: 0,
    actualPlayerId: '',
    firstPlayerId: '',
    phase: Phase.Turn,
  };
  public get game(): IGameViewModel | undefined {
    return this._game;
  }

  constructor(
    private gameService: GameService,
    private tokenService: TokenService,
  ) {
    gameService.gameEventEmitter.subscribe((game: IGameViewModel | undefined) => {
      this._game = game;
    });
  }

  public get isMyTurn(): boolean {
    return (
      this.game?.actualPlayerId === this.tokenService.user?.sub &&
      this.game?.phase === Phase.Turn
    );
  }

  public get board(): IBoardViewModel | undefined {
    return this.game?.players.find(
      (player) => player.userId === this.tokenService.user?.sub
    )?.board;
  }
}
