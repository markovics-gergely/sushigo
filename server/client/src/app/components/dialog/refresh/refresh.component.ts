import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RefreshService } from 'src/app/services/refresh.service';

@Component({
  selector: 'app-refresh',
  templateUrl: './refresh.component.html',
  styleUrls: ['./refresh.component.scss']
})
export class RefreshComponent {
  private _counter: number = 30;
  constructor(
    public dialogRef: MatDialogRef<RefreshComponent>,
  ) { }

  public decline() {
    this.dialogRef.close(false);
  }

  public accept() {
    this.dialogRef.close(true);
  }

  public get counter(): number {
    return this._counter;
  }


  private decrementCounter() {
    this._counter--;
    if (this._counter <= 0) {
      this.decline();
    } else {
      setTimeout(() => this.decrementCounter(), 1000);
    }
  }
}
