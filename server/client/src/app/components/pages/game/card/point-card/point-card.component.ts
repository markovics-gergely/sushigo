import { Component } from '@angular/core';
import { CardBaseComponent } from '../card-base/card-base.component';

@Component({
  selector: 'app-point-card',
  templateUrl: './point-card.component.html',
  styleUrls: ['./point-card.component.scss']
})
export class PointCardComponent extends CardBaseComponent {


  protected override get imagePath(): string {
    return `/gamefiles/files/images/${this.card.cardType.toString()}${this.card.additionalInfo['Points']}.png`;
  }
}
