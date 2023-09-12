import { Component, Input } from '@angular/core';
import { CardService } from 'src/app/services/card.service';
import { GameSelectService } from 'src/app/services/game-select.service';
import { GameService } from 'src/app/services/game.service';
import { IBoardViewModel, IHandViewModel } from 'src/shared/game.models';

@Component({
  selector: 'app-hand',
  templateUrl: './hand.component.html',
  styleUrls: ['./hand.component.scss']
})
export class HandComponent {
  private _hand: IHandViewModel | undefined;
  public get hand(): IHandViewModel | undefined {
    return this._hand;
  }

  protected selectedHand = ["hand"];

  @Input() board: IBoardViewModel | undefined;

  constructor(private cardService: CardService, private gameService: GameService, private gameSelectService: GameSelectService) {
    this.cardService.handEventEmitter.subscribe((hand: IHandViewModel | undefined) => {
      this._hand = hand;
    });
    gameSelectService.boardSelectEventEmitter.subscribe((value: boolean) => {
      if (value) {
        this.selectedHand = ["hand"];
      }
    });
  }

  protected get canPlay() {
    return this.gameService.canPlayCard;
  }

  protected get canPlayAfter() {
    return this.gameService.canPlayAfterCard;
  }
}
