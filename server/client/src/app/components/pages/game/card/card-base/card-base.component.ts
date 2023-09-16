import { Component, inject } from '@angular/core';
import { GameSelectService } from 'src/app/services/game-select.service';
import { GameService } from 'src/app/services/game.service';
import { CardType, CardTypeUtil, SelectType } from 'src/shared/deck.models';
import { CARD, HAND } from 'src/shared/game.models';

@Component({
  selector: 'app-card-base',
  templateUrl: './card-base.component.html',
  styleUrls: ['./card-base.component.scss']
})
export class CardBaseComponent {
  protected card = inject(CARD);
  protected hand = inject(HAND);
  protected cardLoaded = false;

  protected get imagePath(): string {
    return `/gamefiles/files/images/${this.cardType}.png`;
  }

  constructor(private gameSelectService: GameSelectService, private gameService: GameService) {}

  protected selectCard() {
    
    console.log('canPlay', this.card, this.hand, this.gameSelectService.selectType, this.canPlay);
    
    if (!this.canPlay) return;
    if (this.hand) {
      this.gameSelectService.selectHandCard(this.card);
    } else {
      this.gameSelectService.selectBoardCard(this.card);
      this.gameSelectService.boardSelectEventEmitter.next(Boolean(this.card));
    }
  }
  
  private isNumeric = (value: string): boolean => !new RegExp(/[^\d]/g).test(value.trim());
  protected get cardType() {
    const cardType = this.card.cardType as string | number;
    if (typeof cardType === 'number' || this.isNumeric(cardType)) return CardType[Number(cardType)];
    return cardType;
  }

  protected get canPlay() {
    return this.gameSelectService.isSelectable(this.card, this.hand);
  }

  protected get selectedInTurn() {
    return this.gameSelectService.isSelected(this.card);
  }
}
