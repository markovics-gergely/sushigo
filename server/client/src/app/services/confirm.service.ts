import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { ConfirmComponent } from '../components/dialog/confirm/confirm.component';

@Injectable({
  providedIn: 'root',
})
export class ConfirmService {
  constructor(private dialog: MatDialog) {}
  /**
   * Create a confirmation dialog
   * @param type Type of dialog text to display
   * @returns Dialog result
   */
  public confirm(type: string, size: string = '40%'): Observable<boolean> {
    const dialogRef = this.dialog.open(ConfirmComponent, {
      width: size,
      data: {
        type,
      },
    });
    return dialogRef.afterClosed();
  }
}
