import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackService {
  private readonly defaultConfig: MatSnackBarConfig = {
    duration: 5 * 1000,
    horizontalPosition: 'end',
    verticalPosition: 'bottom',
    panelClass: 'snack-class',
  };

  constructor(private snackBar: MatSnackBar) { }

  /**
   * Display a snackbar wih the given text
   * @param message Message to display
   * @param action Display text of the button
   * @param config Configuration of the snackbar
   */
  openSnackBar(message: string, action: string = 'Close', config: MatSnackBarConfig = {}) {
    this.snackBar.open(message, action, { ...this.defaultConfig, ...config });
  }
}
