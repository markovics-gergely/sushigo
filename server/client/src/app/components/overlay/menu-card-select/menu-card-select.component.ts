import { trigger, transition, style, animate } from '@angular/animations';
import { Component } from '@angular/core';
import { IHandCardViewModel, IHandViewModel, Phase } from 'src/app/game/models/game.models';
import { GamePermissionService } from 'src/app/game/services/game-permission.service';
import { GameService } from 'src/app/game/services/game.service';
import { HandService } from 'src/app/game/services/hand.service';
import { PlayStrategyService } from 'src/app/game/services/play-strategy.service';
import { LoadingService } from 'src/shared/services/loading.service';
import { CardType, CardTypeUtil } from 'src/shared/models/deck.models';
import {
  IGameViewModel,
  AdditionalUtil,
  ICardViewModel,
} from 'src/shared/models/game.models';

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
    private gamePermissionService: GamePermissionService,
    private gameService: GameService,
    private handService: HandService,
    private loadingService: LoadingService,
    private playStrategyService: PlayStrategyService
  ) {
    gameService.gameEventEmitter.subscribe(
      (game: IGameViewModel | undefined) => {
        this._game = game;
        if (!game || !this.show) return;
        handService.loadHand();
      }
    );
    handService.handEventEmitter.subscribe(
      (hand: IHandViewModel | undefined) => {
        this.hand = hand;
        if (!this.selectedMenuCard) return;
        this.initSelectables(this.selectedMenuCard);
      }
    );
  }

  private initSelectables(menuCard: IHandCardViewModel) {
    this.selectables = JSON.parse(menuCard.cardInfo.customTagString ?? '[]') as IHandCardViewModel[];
  }

  get show() {
    const cardType = this.gameService.ownPlayer?.selectedCardType;
    return (
      this.gamePermissionService.inPhaseAndActualPlayer(Phase.AfterTurn) &&
      cardType &&
      CardTypeUtil.equals(cardType, CardType.Menu)
    );
  }

  get selectedMenuCard(): IHandCardViewModel | undefined {
    return this.hand?.cards.find(
      (c) => c.id === this.gameService.ownPlayer?.selectedCardId
    );
  }

  protected getImageUrl(card: ICardViewModel): string {
    if (card.cardInfo.cardType === null) return '';
    const point = card?.cardInfo.point;
    return `/gamefiles/files/images/${CardType[card.cardInfo.cardType]}${point === null ? '' : point}.png`;
  }

  protected selectCard(card: IHandCardViewModel) {
    this.playStrategyService.onSelectFromHand(card);
  }
}
