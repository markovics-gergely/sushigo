import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { CardService } from 'src/app/services/card.service';
import { GameService } from 'src/app/services/game.service';
import { LoadingService } from 'src/app/services/loading.service';
import { TokenService } from 'src/app/services/token.service';
import { CardType, CardTypeUtil } from 'src/shared/deck.models';
import { Additional, AdditionalUtil, IBoardViewModel, ICardViewModel, IGameViewModel, IPlayerViewModel, Phase, PhaseUtil } from 'src/shared/game.models';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss'],
})
export class GameComponent {
  private _game: IGameViewModel | undefined;
  public get game(): IGameViewModel | undefined {
    return this._game;
  }

  protected selectedPlayer: IPlayerViewModel | undefined;

  constructor(
    private gameService: GameService,
    private tokenService: TokenService,
    private loadingService: LoadingService,
    private translate: TranslateService,
    private cardService: CardService
  ) {
    gameService.gameEventEmitter.subscribe((game: IGameViewModel | undefined) => {
      this._game = game;
      this.selectedPlayer = game?.players[0];
    });
  }

  public get board(): IBoardViewModel | undefined {
    return this.game?.players.find(
      (player) => this.tokenService.isOwnPlayer(player.id)
    )?.board;
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
    this.cardService.skipAfterTurn().subscribe({
      next: () => this.loadingService.stop(),
      error: () => this.loadingService.stop()
    });
  }

  protected get canEndTurn(): boolean {
    return this.gameService.canEndTurn;
  }

  protected get canEndRound(): boolean {
    return this.gameService.canEndRound;
  }

  protected get canEndGame(): boolean {
    return this.gameService.canEndGame;
  }

  protected get canPlayCard(): boolean {
    return this.gameService.canPlayCard;
  }

  protected get canPlayAfterCard(): boolean {
    return this.gameService.canPlayAfterCard;
  }

  protected getImageUrl(card: ICardViewModel): string {
    let points = AdditionalUtil.getFromRecord(card.additionalInfo, Additional.Points);
    if (CardTypeUtil.equals(card.cardType, CardType.Fruit)) {
      points = points?.padStart(3, '0');
    }
    const imageName = CardTypeUtil.getString(card.cardType) + (points ?? "") + ".png";
    return `/gamefiles/files/images/${imageName}`;
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
    return Boolean(AdditionalUtil.getFromRecord(card.additionalInfo, Additional.Tagged));
  }

  protected get actualPlayer(): IPlayerViewModel | undefined {
    return this.game?.players.find(
      (player) => this.game?.actualPlayerId === player.id
    );
  }
}
