import { Component } from '@angular/core';
import { CardService } from 'src/app/services/card.service';
import { IHandViewModel } from 'src/shared/game.models';

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

  constructor(private cardService: CardService) {
    this.cardService.handEventEmitter.subscribe((hand: IHandViewModel | undefined) => {
      this._hand = hand;
    });
  }
}
