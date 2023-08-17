import { Component, Injector, Input } from '@angular/core';
import { CARD, ICardViewModel } from 'src/shared/game.models';
import { CardBaseComponent } from '../card-base/card-base.component';
import { PointCardComponent } from '../point-card/point-card.component';

@Component({
  selector: 'app-card-wrapper',
  templateUrl: './card-wrapper.component.html',
  styleUrls: ['./card-wrapper.component.scss']
})
export class CardWrapperComponent {
  @Input() card: ICardViewModel = {} as ICardViewModel;

  protected get component(): typeof CardBaseComponent {
    if (this.card.additionalInfo['points']) {
      return PointCardComponent;
    }
    return CardBaseComponent;
  }

  protected get injector(): any {
    return Injector.create({
      providers: [{ provide: CARD, useValue: this.card }],
      parent: this.injector,
    });
  }
}
