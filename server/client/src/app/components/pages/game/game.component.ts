import { Component } from '@angular/core';
import { GameService } from 'src/app/services/game.service';
import { DeckType } from 'src/shared/deck.models';
import { IGameViewModel, Phase } from 'src/shared/game.models';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent {
  private _game: IGameViewModel | undefined = {
    id: '',
    name: '',
    players: Array.from({ length: 10 }, () =>
      ({
        id: '',
        userId: '',
        userName: 'name',
        points: 0,
        board: { cards: [] },
        handId: ''
      })
    ),
    deckType: DeckType.SushiGo,
    round: 0,
    actualPlayerId: '',
    firstPlayerId: '',
    phase: Phase.Turn,
  };
  public get game(): IGameViewModel | undefined { return this._game; }

  constructor(
    private gameService: GameService
  ) {
    gameService.gameEventEmitter.subscribe((game: IGameViewModel | undefined) => {
      //this._game = game;
    });
  }
}
