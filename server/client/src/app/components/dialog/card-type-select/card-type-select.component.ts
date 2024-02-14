import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { GameService } from 'src/app/game/services/game.service';
import { ShopService } from 'src/app/services/shop.service';
import { CardType, IDeckItemViewModel } from 'src/shared/models/deck.models';

@Component({
  selector: 'app-card-type-select',
  templateUrl: './card-type-select.component.html',
  styleUrls: ['./card-type-select.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CardTypeSelectComponent implements OnInit {
  private _selectForm: FormGroup | undefined;
  public get selectForm(): FormGroup | undefined { return this._selectForm; }

  private _deck: IDeckItemViewModel | undefined;
  public get cardTypes(): CardType[] { return this._deck?.cardTypes ?? []; }

  constructor(
    public dialogRef: MatDialogRef<CardTypeSelectComponent>,
    private shopService: ShopService,
    private gameService: GameService
  ) { }

  ngOnInit(): void {
    this._selectForm = new FormGroup({
      card: new FormControl(undefined, Validators.required),
    });
    this.shopService.getDeck(this.gameService.deckType).subscribe((deck) => { this._deck = deck;});
  }

  public submit() {
    this.dialogRef.close(this._selectForm?.value.card);
  }

  public cancel() {
    this.dialogRef.close();
  }

  protected getImagePath(card: CardType): string {
    return `/gamefiles/files/images/${card}.png`;
  }
}
