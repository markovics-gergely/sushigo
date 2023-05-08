import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-lobby',
  templateUrl: './create-lobby.component.html',
  styleUrls: ['./create-lobby.component.scss']
})
export class CreateLobbyComponent implements OnInit {
  private _createForm: FormGroup | undefined;

  constructor(
    public dialogRef: MatDialogRef<CreateLobbyComponent>
  ) {}

  ngOnInit(): void {
    this._createForm = new FormGroup({
      name: new FormControl(undefined, Validators.required),
      password: new FormControl(undefined, Validators.required),
    });
  }

  public get createForm(): FormGroup | undefined {
    return this._createForm;
  }

  public get valid(): boolean {
    return this._createForm?.valid || false;
  }

  public submit() {
    this.dialogRef.close(this._createForm?.value);
  }

  public cancel() {
    this.dialogRef.close();
  }
}
