import { trigger, transition, style, animate } from '@angular/animations';
import { Component } from '@angular/core';
import { CardService } from 'src/app/services/card.service';
import { GameService } from 'src/app/services/game.service';
import { LoadingService } from 'src/app/services/loading.service';
import { CardType, CardTypeUtil } from 'src/shared/deck.models';
import {
  IGameViewModel,
  PhaseUtil,
  Phase,
  IPlayerViewModel,
  IHandViewModel,
  IHandCardViewModel,
  Additional,
  AdditionalUtil,
  ICardViewModel,
} from 'src/shared/game.models';

@Component({
  selector: 'app-menu-card-select',
  templateUrl: './menu-card-select.component.html',
  styleUrls: ['./menu-card-select.component.scss'],
  animations: [
    trigger('slideInOut', [
      transition(':enter', [
        style({ transform: 'translateY(-120%)' }),
        animate('200ms {{delay}}ms ease-in', style({ transform: 'translateY(0%)' })),
      ], { params: { delay: 0 } }),
      transition(':leave', [
        style({ transform: 'translateY(0%)' }),
        animate('200ms ease-out', style({ transform: 'translateY(120%)' })),
      ], { params: { delay: 0 } }),
    ]),
  ],
})
export class MenuCardSelectComponent {
  private _game: IGameViewModel | undefined;
  public get game(): IGameViewModel | undefined {
    return this._game;
  }
  protected hand: IHandViewModel | undefined;
  protected selectables: IHandCardViewModel[] = [];
  constructor(
    private gameService: GameService,
    private cardService: CardService,
    private loadingService: LoadingService
  ) {
    gameService.gameEventEmitter.subscribe(
      (game: IGameViewModel | undefined) => {
        this._game = game;
        if (!game) return;
        cardService.refreshHand();
      }
    );
    cardService.handEventEmitter.subscribe(
      (hand: IHandViewModel | undefined) => {
        this.hand = hand;
        if (!this.selectedMenuCard) return;
        this.initSelectables(this.selectedMenuCard);
      }
    );
  }

  private initSelectables(menuCard: IHandCardViewModel) {
    const cards = JSON.parse(
      AdditionalUtil.getFromRecord(
        menuCard.additionalInfo,
        Additional.MenuCards
      ) ?? '[]',
      function (key, value) {
        const lowerKey = key.charAt(0).toLowerCase() + key.slice(1);
        if (key === 'CardType') {
          this[lowerKey] = CardTypeUtil.getString(value);
        } else if (key === 'AdditionalInfo') {
          this[lowerKey] = JSON.parse(JSON.stringify(value)) as Record<Additional, string>;
        } else this[lowerKey] = value;
        return value;
      }
    ) as IHandCardViewModel[];
    this.selectables = cards;
  }

  get show() {
    const cardType = this.gameService.ownPlayer?.selectedCardType;
    return (
      PhaseUtil.equals(this._game?.phase, Phase.AfterTurn) &&
      cardType &&
      CardTypeUtil.equals(cardType, CardType.Menu)
    );
  }

  get players() {
    return this.game?.players.sort((a, b) => b.points - a.points) ?? [];
  }

  getPlacement(player: IPlayerViewModel) {
    return this.players.findIndex((p) => p.points === player.points) + 1;
  }

  get selectedMenuCard(): IHandCardViewModel | undefined {
    return this.hand?.cards.find(
      (c) => c.id === this.gameService.ownPlayer?.selectedCardId
    );
  }

  protected getImageUrl(card: ICardViewModel): string {
    let points = AdditionalUtil.getFromRecord(
      card.additionalInfo,
      Additional.Points
    );
    if (CardTypeUtil.equals(card.cardType, CardType.Fruit)) {
      points = points?.padStart(3, '0');
    }
    const imageName =
      CardTypeUtil.getString(card.cardType) + (points ?? '') + '.png';
    return `/gamefiles/files/images/${imageName}`;
  }

  protected selectCard(card: IHandCardViewModel) {
    this.loadingService.start();
    card.additionalInfo[Additional.CardIds] = card.id;
    this.cardService.playAfterTurn({
      handCardId: this.selectedMenuCard?.id ?? '',
      additionalInfo: card.additionalInfo,
    }).subscribe({}).add(() => {
      this.loadingService.stop();
      this.selectables = [];
    });
  }
}
