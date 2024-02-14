import { Injectable } from '@angular/core';
import { TokenService } from 'src/app/services/token.service';
import { AFTER_TURN_FROM_BOARD, AFTER_TURN_FROM_HAND } from 'src/shared/models/deck.models';
import { HandService } from './hand.service';
import { IBoardCardViewModel, IGameViewModel, IHandCardViewModel, Phase, SelectType } from '../models/game.models';
import { GameService } from './game.service';

@Injectable({
  providedIn: 'root'
})
export class GamePermissionService {
  private _game: IGameViewModel | undefined;

  constructor(
    private gameService: GameService,
    private handService: HandService,
    private tokenService: TokenService,
  ) { 
    gameService.gameEventEmitter.subscribe((game: IGameViewModel | undefined) => {
      this._game = game;
    });
  }

  public canPlayFromHand(card: IHandCardViewModel): boolean {
    if (!this.tokenService.ownPlayer(this._game?.actualPlayerId)) return false;

    if (this._game?.phase === Phase.Turn) {
      return this.canPlayFromHandInTurn(card);
    } else if (this._game?.phase === Phase.AfterTurn) {
      return this.canPlayFromHandAfterTurn(card);
    }
    return false;
  }

  public canPlayFromBoard(card: IBoardCardViewModel): boolean {
    if (!this.tokenService.ownPlayer(this._game?.actualPlayerId)) return false;

    if (this._game?.phase === Phase.Turn) {
      return this.canPlayFromBoardInTurn(card);
    } else if (this._game?.phase === Phase.AfterTurn) {
      return this.canPlayFromBoardAfterTurn(card);
    }
    return false;
  }

  private canPlayFromHandInTurn(card: IHandCardViewModel): boolean {
    const selected = this.handService.selectedCard;
    if (selected) return !card.isSelected && selected.selectType === SelectType.Hand;

    return !card.isSelected;
  }

  private canPlayFromBoardInTurn(card: IBoardCardViewModel): boolean {
    return this.handService.isSelectedInTypes([SelectType.Board, SelectType.BoardMulti]);
  }

  private canPlayFromHandAfterTurn(card: IHandCardViewModel): boolean {
    const selected = this.handService.selectedCard;
    if (selected) return selected.selectType === SelectType.Hand && !card.isSelected;

    return !card.isSelected && AFTER_TURN_FROM_HAND.includes(card.cardInfo.cardType);
  }

  private canPlayFromBoardAfterTurn(card: IBoardCardViewModel): boolean {
    const selected = this.handService.selectedCard;
    if (selected) return this.handService.isSelectedInTypes([SelectType.Board, SelectType.BoardMulti]);
    
    return AFTER_TURN_FROM_BOARD.includes(card.cardInfo.cardType);
  }

  public anyCanPlayFromHand(cards: IHandCardViewModel[]): boolean {
    return cards.some((card) => this.canPlayFromHand(card));
  }

  public anyCanPlayFromBoard(cards: IBoardCardViewModel[]): boolean {
    return cards.some((card) => this.canPlayFromBoard(card));
  }

  public inPhaseAndActualPlayer(phase: Phase): boolean {
    return this._game?.phase === phase && this.tokenService.ownPlayer(this._game.actualPlayerId);
  }
}
