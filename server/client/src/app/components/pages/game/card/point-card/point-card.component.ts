import { Component } from '@angular/core';
import { CardBaseComponent } from '../card-base/card-base.component';
import { Additional, AdditionalUtil } from 'src/shared/game.models';
import { CardType, CardTypeUtil } from 'src/shared/deck.models';

@Component({
  selector: 'app-point-card',
  templateUrl: './point-card.component.html',
  styleUrls: ['./point-card.component.scss']
})
export class PointCardComponent extends CardBaseComponent {


  protected override get imagePath(): string {
    return `/gamefiles/files/images/${this.cardType}${this.points}.png`;
  }

  private get points() {
    const points = AdditionalUtil.getFromRecord(this.card.additionalInfo, Additional.Points);
    if (CardTypeUtil.equals(this.card.cardType, CardType.Fruit)) {
      return points?.padStart(3, '0');
    }
    return points;
  }
}
