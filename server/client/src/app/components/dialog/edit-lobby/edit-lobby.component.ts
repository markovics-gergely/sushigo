import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DeckType, DeckTypeUtil, IDeckViewModel } from 'src/shared/deck.models';
import { ShopService } from 'src/app/services/shop.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AclService } from 'src/app/services/acl.service';

@Component({
  selector: 'app-edit-lobby',
  templateUrl: './edit-lobby.component.html',
  styleUrls: ['./edit-lobby.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EditLobbyComponent implements OnInit {
  private _editForm: FormGroup | undefined;
  private _decks: IDeckViewModel[] = [];

  constructor(
    private shopService: ShopService,
    private aclService: AclService,
    public dialogRef: MatDialogRef<EditLobbyComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: DeckType
  ) { }

  ngOnInit(): void {
    this._editForm = new FormGroup({
      deckType: new FormControl(DeckTypeUtil.getString(this._data), Validators.required),
    });
    this.shopService.decks.subscribe((decks) => (this._decks = decks));
  }

  public get editForm(): FormGroup | undefined {
    return this._editForm;
  }

  public submit() {
    this.dialogRef.close(this._editForm?.value);
  }

  public cancel() {
    this.dialogRef.close();
  }

  public get decks(): IDeckViewModel[] {
    return this._decks;
  }

  public disabled(deckType: DeckType): boolean {
    return !this.aclService.hasDeck(deckType);
  }

  public get selectedDeck(): IDeckViewModel {
    return this._decks.find((deck) => deck.deckType === this._editForm?.value.deckType)!;
  }

  public get deckType(): string {
    return DeckTypeUtil.getString(this._editForm?.value.deckType);
  }

  public original(deck: IDeckViewModel): boolean {
    return DeckTypeUtil.equals(deck.deckType, this._data);
  }
}
