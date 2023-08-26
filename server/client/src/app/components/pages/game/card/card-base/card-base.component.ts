import { Component, inject } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CARD } from 'src/shared/game.models';

@Component({
  selector: 'app-card-base',
  templateUrl: './card-base.component.html',
  styleUrls: ['./card-base.component.scss']
})
export class CardBaseComponent {
  protected card = inject(CARD);
  protected cardLoaded = false;

  protected get imagePath(): string {
    return `/gamefiles/files/images/${this.card.cardType.toString()}.png`;
  }
}
