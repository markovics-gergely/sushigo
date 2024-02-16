import { Component, Input } from '@angular/core';
import { GamePermissionService } from '../../services/game-permission.service';
import { PlayStrategyService } from '../../services/play-strategy.service';
import { IBoardCardViewModel, IHandCardViewModel } from '../../models/game.models';
import { CardType } from 'src/shared/models/deck.models';
import { HandService } from '../../services/hand.service';

@Component({
  selector: 'app-game-card',
  templateUrl: './game-card.component.html',
  styleUrls: ['./game-card.component.scss']
})
export class GameCardComponent {
  @Input() inHand: boolean = true;
  @Input() card: IBoardCardViewModel | IHandCardViewModel | undefined;

  protected cardLoaded = false;

  protected get imagePath(): string {
    if (this.cardType === undefined || this.cardType === null) return '';
    const point = this.card?.cardInfo.point;
    return `/gamefiles/files/images/${CardType[this.cardType]}${point === null ? '' : point}.png`;
  }

  constructor(private gamePermissionService: GamePermissionService, private playStrategyService: PlayStrategyService, private handService: HandService) {
  }

  protected onSelect() {
    if (this.inHand) {
    console.log(this.card);
      this.playStrategyService.onSelectFromHand(this.card as IHandCardViewModel).subscribe({});
    } else {
      this.playStrategyService.onSelectFromBoard(this.card as IBoardCardViewModel).subscribe({});
    }
  }

  protected get cardType() {
    return this.card?.cardInfo.cardType;
  }

  protected get canPlay() {
    if (this.inHand) {
      return this.gamePermissionService.canPlayFromHand(this.card as IHandCardViewModel);
    }
    return this.gamePermissionService.canPlayFromBoard(this.card as IBoardCardViewModel);
  }

  protected get selectedInTurn() {
    return (this.card as IHandCardViewModel)?.isSelected || this.handService.selectedCard?.card.id == this.card?.id;
  }
}
