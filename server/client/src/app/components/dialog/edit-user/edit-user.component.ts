import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IEditUserDTO } from 'src/shared/models/user.models';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnInit {
  private _editForm: FormGroup | undefined;
  private _avatar: string | undefined;

  constructor(
    public dialogRef: MatDialogRef<EditUserComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: IEditUserDTO
  ) {}

  ngOnInit(): void {
    this._editForm = new FormGroup({
      userName: new FormControl(this._data.userName, Validators.required),
      firstName: new FormControl(this._data.firstName, Validators.required),
      lastName: new FormControl(this._data.lastName, Validators.required),
      avatar: new FormControl(undefined),
    });
  }

  public get editForm(): FormGroup | undefined {
    return this._editForm;
  }

  public get valid(): boolean {
    return this._editForm?.valid || false;
  }

  public submit() {
    this.dialogRef.close(this._editForm?.value);
  }

  public cancel() {
    this.dialogRef.close();
  }

  public uploadAvatar(event: Event) {
    const file = (event.target as HTMLInputElement)!.files![0];
    const reader = new FileReader();
    reader.onload = (event: any) => {
      this._avatar = event.target.result;
    };
    reader.onerror = (event: any) => {
      console.log("File could not be read: " + event.target.error.code);
    };
    reader.readAsDataURL((event.target as HTMLInputElement)!.files![0]);
    this._editForm?.patchValue({ avatar: file });
  }

  public get avatar(): string | undefined {
    return (this._editForm?.value.avatar as File)?.name;
  }
}
