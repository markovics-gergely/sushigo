import { trigger, transition, style, animate } from '@angular/animations';
import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LoadingService } from 'src/shared/services/loading.service';
import { CardType } from 'src/shared/models/deck.models';
import { IGameViewModel, IBoardViewModel, ICardViewModel, PhaseUtil, Phase, IPlayerViewModel } from 'src/shared/models/game.models';
import { GameService } from '../services/game.service';
import { GamePermissionService } from '../services/game-permission.service';
import { HandService } from '../services/hand.service';

@Component({
  selector: 'app-game-page',
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.scss'],
  animations: [
    trigger('slideInOut', [
      transition(':enter', [
        style({ transform: 'translateX(-120%)' }),
        animate('300ms {{delay}}ms ease-in', style({ transform: 'translateX(0%)' })),
      ], { params: { delay: 0 } }),
      transition(':leave', [
        style({ transform: 'translateX(0%)' }),
        animate('300ms {{delay}}ms ease-out', style({ transform: 'translateX(120%)' })),
      ], { params: { delay: 0 } }),
    ]),
    trigger('appear', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms {{delay}}ms ease-in', style({ opacity: 1 })),
      ], { params: { delay: 0 } }),
      transition(':leave', [
        style({ opacity: 1 }),
        animate('200ms ease-out', style({ opacity: 0 })),
      ], { params: { delay: 0 } }),
    ]),
  ],
})
export class GamePageComponent {
  protected game: IGameViewModel | undefined;
  protected counter: number = 30;

  protected selectedPlayer: IPlayerViewModel | undefined;

  constructor(
    private gameService: GameService,
    private loadingService: LoadingService,
    private translate: TranslateService,
    private handService: HandService,
    private gamePermissionService: GamePermissionService
  ) {
    gameService.gameEventEmitter.subscribe((game: IGameViewModel | undefined) => {
      this.game = game;
      this.selectedPlayer = game?.players[0];
    });
    gameService.gameCountEventEmitter.subscribe((count: number) => {
      this.counter = count;
    });
  }

  public get board(): IBoardViewModel | undefined {
    return this.gameService.board;
  }

  protected endTurn() {
    this.loadingService.start();
    this.gameService.proceedEndTurn().subscribe({
      next: () => this.loadingService.stop(),
      error: () => this.loadingService.stop()
    });
  }

  protected endRound() {
    this.loadingService.start();
    this.gameService.proceedEndRound().subscribe({
      next: () => this.loadingService.stop(),
      error: () => this.loadingService.stop()
    });
  }

  protected endGame() {
    this.loadingService.start();
    this.gameService.proceedEndGame().subscribe({
      next: () => this.loadingService.stop(),
      error: () => this.loadingService.stop()
    });
  }

  protected skipAfter() {
    this.loadingService.start();
    this.handService.skipAfterTurn().subscribe({
      next: () => this.loadingService.stop(),
      error: () => this.loadingService.stop()
    });
  }

  protected getImageUrl(card: ICardViewModel): string {
    if (card.cardInfo.cardType === null) return '';
    const point = card?.cardInfo.point;
    return `/gamefiles/files/images/${CardType[card.cardInfo.cardType]}${point === null ? '' : point}.png`;
  }

  protected get phaseTranslatable(): string {
    return 'game.phase.' + PhaseUtil.getString(this.game?.phase ?? Phase.None);
  }

  protected get gameInfo(): string {
    const phase = this.translate.instant(this.phaseTranslatable);
    const name = this.translate.instant('game.name', { name: this.game?.name });
    const deck = this.translate.instant('game.deck', { deck: this.translate.instant('shop.' + this.game?.deckType + '.title') });
    return Array.of(phase, name, deck).join('\n');
  }

  protected isTagged(card: ICardViewModel): boolean {
    return Boolean(card.cardInfo.customTag);
  }

  protected get actualPlayer(): IPlayerViewModel | undefined {
    return this.game?.players.find(
      (player) => this.game?.actualPlayerId === player.id
    );
  }

  protected get canEndTurn(): boolean {
    return this.gamePermissionService.inPhaseAndActualPlayer(Phase.EndTurn);
  }

  protected get canEndRound(): boolean {
    return this.gamePermissionService.inPhaseAndActualPlayer(Phase.EndRound);
  }

  protected get canEndGame(): boolean {
    return this.gamePermissionService.inPhaseAndActualPlayer(Phase.EndGame);
  }

  protected get canPlayCard(): boolean {
    return this.gamePermissionService.inPhaseAndActualPlayer(Phase.Turn);
  }

  protected get canPlayAfterCard(): boolean {
    return this.gamePermissionService.inPhaseAndActualPlayer(Phase.AfterTurn);
  }
}
