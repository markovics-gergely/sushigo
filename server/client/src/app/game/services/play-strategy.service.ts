import { Injectable, Injector, ProviderToken } from '@angular/core';
import { CardType } from 'src/shared/models/deck.models';
import { HandService } from './hand.service';
import { Observable, of } from 'rxjs';
import { ICardViewModel, Phase, SelectType } from '../models/game.models';
import { MatDialog } from '@angular/material/dialog';
import { CardTypeSelectComponent } from 'src/app/components/dialog/card-type-select/card-type-select.component';
import { LoadingService } from 'src/shared/services/loading.service';
import { GameService } from './game.service';

@Injectable({
  providedIn: 'any',
})
export class PlayStrategy {
  protected handService: HandService;

  constructor(injector: Injector) {
    this.handService = injector.get(HandService);
  }
  onSelectFromHand(card: ICardViewModel): Observable<void> {
    if ('isSelected' in card) {
      card.isSelected = true;
    }
    return this.handService.playCard({
      handCardId: card.id,
      cardInfo: card.cardInfo,
    });
  }
  onSelectFromBoard(card: ICardViewModel): Observable<void> {
    return of();
  }
  onSelectFromHandAfterTurn(card: ICardViewModel, ...args: any): Observable<void> {
    return of();
  }
  onSelectFromBoardAfterTurn(card: ICardViewModel, ...args: any): Observable<void> {
    return of();
  }
}

@Injectable({
  providedIn: 'any',
})
export class TakeoutBoxStrategy extends PlayStrategy {
  override onSelectFromHand(card: ICardViewModel): Observable<void> {
    this.handService.selectCard({
      card: card,
      fromHand: true,
      selectType: SelectType.BoardMulti,
    });
    return of();
  }
  override onSelectFromBoard(card: ICardViewModel): Observable<void> {
    if (!this.handService.selectedCard) return of();
    const cardIds = this.handService.selectedCard.card.cardInfo.cardIds ?? [];
    if (cardIds.includes(card.id)) {
      cardIds.splice(cardIds.indexOf(card.id), 1);
    } else {
      cardIds.push(card.id);
    }
    this.handService.selectedCard.card.cardInfo.cardIds = cardIds;
    return of();
  }
}

@Injectable({
  providedIn: 'any',
})
export class SpecialOrderStrategy extends PlayStrategy {
  override onSelectFromHand(card: ICardViewModel): Observable<void> {
    this.handService.selectCard({
      card: card,
      fromHand: true,
      selectType: SelectType.Board,
    });
    return of();
  }
  override onSelectFromBoard(card: ICardViewModel): Observable<void> {
    if (!this.handService.selectedCard) return of();
    const selectedCard = this.handService.selectedCard.card;
    selectedCard.cardInfo.cardIds = [card.id];
    return this.handService.playCard({
      handCardId: selectedCard.id,
      cardInfo: selectedCard.cardInfo,
    });
  }
}

@Injectable({
  providedIn: 'any',
})
export class SpoonStrategy extends PlayStrategy {
  constructor(
    injector: Injector,
    private loadingService: LoadingService,
    private dialog: MatDialog
  ) {
    super(injector);
  }
  override onSelectFromBoardAfterTurn(card: ICardViewModel): Observable<void> {
    const dialogRef = this.dialog.open(CardTypeSelectComponent);
    dialogRef.afterClosed().subscribe((cardType: CardType | undefined) => {
      if (cardType) {
        this.loadingService.start();
        this.handService.playAfterTurn({
          isHandCard: false,
          handOrBoardCardId: card.id,
          cardType,
        }).subscribe({}).add(() => {
          this.loadingService.stop();
        });
      }
    });
    return of();
  }
}

@Injectable({
  providedIn: 'any',
})
export class ChopsticksStrategy extends PlayStrategy {
  override onSelectFromBoardAfterTurn(card: ICardViewModel, ...args: any): Observable<void> {
    this.handService.selectCard({ card, fromHand: false, selectType: SelectType.Hand, switchTo: 'hand' });
    return of();
  }
  override onSelectFromHandAfterTurn(card: ICardViewModel, ...args: any): Observable<void> {
    if (!this.handService.selectedCard) return of();
    const selectedCard = this.handService.selectedCard.card;
    return this.handService.playAfterTurn({
      isHandCard: false,
      handOrBoardCardId: selectedCard.id,
      targetCardId: card.id,
    });
  }
}

const PlayStrategyMap = new Map<CardType, ProviderToken<PlayStrategy>>([
  [CardType.TakeoutBox, TakeoutBoxStrategy],
  [CardType.SpecialOrder, SpecialOrderStrategy],
  [CardType.Spoon, SpoonStrategy],
  [CardType.Chopsticks, ChopsticksStrategy],
]);

@Injectable({
  providedIn: 'root',
})
export class PlayStrategyService {
  constructor(private injector: Injector, private gameService: GameService, private handService: HandService) {}

  public getStrategy(card: ICardViewModel): PlayStrategy {
    let cardType = card.cardInfo.cardType;
    if (this.handService.selectedCard) {
      cardType = this.handService.selectedCard.card.cardInfo.cardType;
    }
    return this.injector.get(
      PlayStrategyMap.get(cardType) ?? PlayStrategy
    );
  }

  public onSelectFromHand(card: ICardViewModel): Observable<void> {
    if (this.gameService.phase === Phase.Turn) {
      return this.getStrategy(card).onSelectFromHand(card);
    } else if (this.gameService.phase === Phase.AfterTurn) {
      return this.getStrategy(card).onSelectFromHandAfterTurn(card);
    }
    return of();
  }

  public onSelectFromBoard(card: ICardViewModel): Observable<void> {
    if (this.gameService.phase === Phase.Turn) {
      return this.getStrategy(card).onSelectFromBoard(card);
    } else if (this.gameService.phase === Phase.AfterTurn) {
      return this.getStrategy(card).onSelectFromBoardAfterTurn(card);
    }
    return of();
  }
}
