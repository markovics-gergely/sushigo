import { Injectable } from '@angular/core';
import { CardType, CardTypeUtil, SelectType } from 'src/shared/deck.models';
import { Additional, ICardViewModel, IHandCardViewModel } from 'src/shared/game.models';
import { CardService } from './card.service';
import { LoadingService } from './loading.service';
import { MatDialog } from '@angular/material/dialog';
import { CardTypeSelectComponent } from '../components/dialog/card-type-select/card-type-select.component';
import { GameService } from './game.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GameSelectService {
  private _selectedCard: ICardViewModel | undefined;
  private _selectedBoardCards: ICardViewModel[] = [];
  private _boardSelectEventEmitter = new BehaviorSubject<boolean>(false);
  public get boardSelectEventEmitter(): BehaviorSubject<boolean> { return this._boardSelectEventEmitter; }

  constructor(
    private cardService: CardService,
    private loadingService: LoadingService,
    private gameService: GameService,
    private dialog: MatDialog
  ) {}

  private get inTurn(): boolean {
    return this.gameService.canPlayCard;
  }

  private get afterTurn(): boolean {
    return this.gameService.canPlayAfterCard;
  }

  public get selectType(): SelectType | undefined {
    if (!this._selectedCard) return undefined;
    return this.inTurn
      ? CardTypeUtil.getSelectType(this._selectedCard.cardType)
      : CardTypeUtil.getSelectTypeAfterPlay(this._selectedCard.cardType);
  }

  public get selectedCard(): ICardViewModel | undefined {
    return this._selectedCard;
  }

  public get canSelectFromBoard(): boolean {
    if (!this._selectedCard) return false;
    if (this.selectType === SelectType.OwnBoardCard) return true;
    if (this.selectType === SelectType.OwnBoardCards) return true;
    return false;
  }

  public get canSelectFromHand(): boolean {
    if (!this._selectedCard) return false;
    if (this.selectType === SelectType.OwnHandCard) return true;
    return false;
  }

  public set selectedCard(value: ICardViewModel | undefined) {
    if (value === this._selectedCard) {
      this._selectedCard = undefined;
      return;
    }
    this._selectedCard = value;
    if (value && this.selectType === SelectType.None) {
      this.loadingService.start();
      this.cardService
        .playCard({
          handCardId: value.id,
          additionalInfo: value.additionalInfo,
        })
        .subscribe({})
        .add(() => {
          this._selectedCard = undefined;
          this.loadingService.stop();
        });
    }
  }

  public isSelectedBoardCard(value: ICardViewModel): boolean {
    return this._selectedCard == value && this.afterTurn;
  }

  public set selectedBoardCard(value: ICardViewModel | undefined) {
    if (value === this._selectedCard) {
      this._selectedCard = undefined;
      return;
    }
    this._selectedCard = value;
    if (value && this.selectType === SelectType.CardType) {
      const dialogRef = this.dialog.open(CardTypeSelectComponent);
      dialogRef.afterClosed().subscribe((card: CardType | undefined) => {
        if (card) {
          this.loadingService.start();
          value.additionalInfo[Additional.Tagged] = card.toString();
          this.cardService
            .playAfterTurn({
              boardCardId: value.id,
              additionalInfo: value.additionalInfo,
            })
            .subscribe({})
            .add(() => {
              this.loadingService.stop();
              this._selectedCard = undefined;
            });
        } else {
          this._selectedCard = undefined;
        }
      });
    }
  }

  public set ownBoardCard(value: ICardViewModel) {
    if (!this._selectedCard) return;
    this.loadingService.start();
    this._selectedCard.additionalInfo[Additional.CardIds] = value.id;
    this.cardService
      .playCard({
        handCardId: this._selectedCard.id,
        additionalInfo: this._selectedCard.additionalInfo,
      })
      .subscribe({
        next: () => this.loadingService.stop(),
        error: () => this.loadingService.stop(),
      })
      .add(() => {
        this._selectedCard = undefined;
      });
  }

  public selectOwnHandCard(value: ICardViewModel) {
    if (!this._selectedCard) return;
    this.loadingService.start();
    this._selectedCard.additionalInfo[Additional.Tagged] = value.id;
    this.cardService
      .playAfterTurn({
        boardCardId: this._selectedCard.id,
        additionalInfo: this._selectedCard.additionalInfo,
      })
      .subscribe({})
      .add(() => {
        this.loadingService.stop()
        this._selectedCard = undefined;
      });
  }

  public confirmOwnBoardCards() {
    if (!this._selectedCard || !this._selectedBoardCards.length) return;
    this.loadingService.start();
    this._selectedCard.additionalInfo[Additional.CardIds] =
      this._selectedBoardCards.map((v) => v.id).join(',');
    this.cardService
      .playCard({
        handCardId: this._selectedCard.id,
        additionalInfo: this._selectedCard.additionalInfo,
      })
      .subscribe({})
      .add(() => {
        this._selectedCard = undefined;
        this.loadingService.stop();
      });
  }

  public selectBoardCard(value: ICardViewModel | undefined): void {
    if (this.inTurn && value && this.selectType === SelectType.OwnBoardCard) {
      if (!this._selectedCard) return;
      this._selectedCard.additionalInfo[Additional.CardIds] = value.id;
      this.cardService
        .playCard({
          handCardId: this._selectedCard.id,
          additionalInfo: this._selectedCard.additionalInfo,
        })
        .subscribe({})
        .add(() => {
          this._selectedCard = undefined;
          this.loadingService.stop();
        });
    } else if (this.inTurn && value && this.selectType === SelectType.OwnBoardCards) {
      if (!this._selectedCard) return;
      if (this._selectedBoardCards.includes(value)) {
        this._selectedBoardCards = this._selectedBoardCards.filter(
          (v) => v !== value
        );
      } else {
        this._selectedBoardCards.push(value);
      }
    } else if (this.afterTurn && value && CardTypeUtil.hasAfterTurn(value.cardType)) {
      if (value.id === this._selectedCard?.id) {
        this._selectedCard = undefined;
        return;
      }
      this._selectedCard = value;
      console.log(this.selectType);
      if (this.selectType === SelectType.CardType) {
        const dialogRef = this.dialog.open(CardTypeSelectComponent);
        dialogRef.afterClosed().subscribe((card: CardType | undefined) => {
          if (card) {
            this.loadingService.start();
            value.additionalInfo[Additional.Tagged] = card.toString();
            this.cardService
              .playAfterTurn({
                boardCardId: value.id,
                additionalInfo: value.additionalInfo,
              })
              .subscribe({})
              .add(() => {
                this.loadingService.stop();
                this._selectedCard = undefined;
              });
          } else {
            this._selectedCard = undefined;
          }
        });
      }
    }
  }

  public selectHandCard(value: IHandCardViewModel) {
    if (value.isSelected) return;
    if (this.inTurn) {
      if (value === this._selectedCard) {
        this._selectedCard = undefined;
        return;
      }
      this._selectedCard = value;
      if (value && this.selectType === SelectType.None) {
        this.loadingService.start();
        this.cardService
          .playCard({
            handCardId: value.id,
            additionalInfo: value.additionalInfo,
          })
          .subscribe({})
          .add(() => {
            this._selectedCard = undefined;
            this.loadingService.stop();
          });
      }
    } else if (this.afterTurn && this.canSelectFromHand) {
      if (!this._selectedCard) return;
      this.loadingService.start();
      this._selectedCard.additionalInfo[Additional.Tagged] = value.id;
      this.cardService
      .playAfterTurn({
        boardCardId: this._selectedCard.id,
        additionalInfo: this._selectedCard.additionalInfo,
      })
      .subscribe({})
      .add(() => {
        this.loadingService.stop()
        this._selectedCard = undefined;
      });
    }
  }

  public isSelected(value: IHandCardViewModel): boolean {
    return this._selectedCard?.id === value.id || value.isSelected;
  }

  public isSelectable(value: IHandCardViewModel, hand: boolean): boolean {
    const boardFocus = Boolean(this.selectType && [SelectType.OwnBoardCard, SelectType.OwnBoardCards].includes(this.selectType));
    if (hand) {
      const handFocus = this.inTurn || this.selectType === SelectType.OwnHandCard;
      const notSelectedBefore = !value.isSelected;
      const handFocusOrDeselect = value.id === this._selectedCard?.id || !boardFocus;
      return handFocus && notSelectedBefore && handFocusOrDeselect;
    } else if (this.inTurn) {
      return boardFocus;
    } else if (this.afterTurn) {
      return CardTypeUtil.hasAfterTurn(value.cardType);
    }
    return false;
  }
}
