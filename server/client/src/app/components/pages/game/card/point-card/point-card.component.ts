import { Component } from '@angular/core';
import { CardBaseComponent } from '../card-base/card-base.component';
import { Additional, AdditionalUtil } from 'src/shared/game.models';
import { CardType } from 'src/shared/deck.models';

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
    return AdditionalUtil.getFromRecord(this.card.additionalInfo, Additional.Points);
  }
}
