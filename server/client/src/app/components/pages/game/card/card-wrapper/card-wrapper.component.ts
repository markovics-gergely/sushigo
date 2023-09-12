import { Component, Injector, Input } from '@angular/core';
import { Additional, AdditionalUtil, CARD, HAND, ICardViewModel } from 'src/shared/game.models';
import { CardBaseComponent } from '../card-base/card-base.component';
import { PointCardComponent } from '../point-card/point-card.component';
import { CardService } from 'src/app/services/card.service';
import { GameSelectService } from 'src/app/services/game-select.service';

@Component({
  selector: 'app-card-wrapper',
  templateUrl: './card-wrapper.component.html',
  styleUrls: ['./card-wrapper.component.scss'],
})
export class CardWrapperComponent {
  @Input() hand: boolean | undefined;
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
    if (this.card && AdditionalUtil.getFromRecord(this.card.additionalInfo, Additional.Points) !== undefined) {
      return PointCardComponent;
    }
    return CardBaseComponent;
  }

  protected cardInjector: any;

  protected loadInjector(injectable: ICardViewModel): void {
    this.cardInjector = Injector.create({
      providers: [{ provide: CARD, useValue: injectable }, { provide: HAND, useValue: this.hand }],
      parent: this.injector,
    });
  }
}
