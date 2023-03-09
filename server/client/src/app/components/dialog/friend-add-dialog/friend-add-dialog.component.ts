import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-friend-add-dialog',
  templateUrl: './friend-add-dialog.component.html',
  styleUrls: ['./friend-add-dialog.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FriendAddDialogComponent implements OnInit {
  private _form: FormGroup | undefined;

  constructor(
    public dialogRef: MatDialogRef<FriendAddDialogComponent>,
  ) { }

  ngOnInit(): void {
    this._form = new FormGroup({
      name: new FormControl('', [Validators.required]),
    });
  }

  public cancel(): void {
    this.dialogRef.close();
  }

  public add(): void {
    if (this._form?.valid) {
      this.dialogRef.close(this._form?.get('name')?.value);
    }
  }

  public get form(): FormGroup | undefined {
    return this._form;
  }

  public get valid(): boolean {
    return this._form?.valid || false;
  }
}
