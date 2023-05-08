import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ILobbyItemViewModel } from 'src/shared/lobby.models';

@Component({
  selector: 'app-join-lobby',
  templateUrl: './join-lobby.component.html',
  styleUrls: ['./join-lobby.component.scss']
})
export class JoinLobbyComponent implements OnInit {
  private _joinForm: FormGroup | undefined;

  constructor(
    public dialogRef: MatDialogRef<JoinLobbyComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: ILobbyItemViewModel
  ) {}

  ngOnInit(): void {
    this._joinForm = new FormGroup({
      name: new FormControl({ value: this._data.name, disabled: true }),
      password: new FormControl(undefined, [Validators.required, Validators.minLength(4)]),
    });
  }

  public get joinForm(): FormGroup | undefined {
    return this._joinForm;
  }

  public get valid(): boolean {
    return this._joinForm?.valid || false;
  }

  public submit() {
    this.dialogRef.close(this._joinForm?.value);
  }

  public cancel() {
    this.dialogRef.close();
  }
}
