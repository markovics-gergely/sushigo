import { Component, inject } from '@angular/core';
import { CARD } from 'src/shared/game.models';

@Component({
  selector: 'app-card-base',
  templateUrl: './card-base.component.html',
  styleUrls: ['./card-base.component.scss']
})
export class CardBaseComponent {
  protected card = inject(CARD);
}
