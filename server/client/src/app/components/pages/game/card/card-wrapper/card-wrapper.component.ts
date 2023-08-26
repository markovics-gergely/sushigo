import { Component, Injector, Input } from '@angular/core';
import { CARD, ICardViewModel } from 'src/shared/game.models';
import { CardBaseComponent } from '../card-base/card-base.component';
import { PointCardComponent } from '../point-card/point-card.component';

@Component({
  selector: 'app-card-wrapper',
  templateUrl: './card-wrapper.component.html',
  styleUrls: ['./card-wrapper.component.scss'],
})
export class CardWrapperComponent {
  private _card: ICardViewModel | undefined;
  @Input() public set card(value: ICardViewModel | undefined) {
    if (value === undefined) {
      return;
    }
    this.loadInjector(value);
    this._card = value;
  }
  public get card(): ICardViewModel | undefined {
    return this._card;
  }

  constructor(private injector: Injector) {}

  protected get component(): typeof CardBaseComponent {
    if (this.card?.additionalInfo['Points'] !== undefined) {
      return PointCardComponent;
    }
    return CardBaseComponent;
  }

  protected cardInjector: any;

  protected loadInjector(injectable: ICardViewModel): void {
    this.cardInjector = Injector.create({
      providers: [{ provide: CARD, useValue: injectable }],
      parent: this.injector,
    });
  }
}
