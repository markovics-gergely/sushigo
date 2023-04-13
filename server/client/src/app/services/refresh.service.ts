import { Injectable } from '@angular/core';
import { TokenService } from './token.service';
import { MatDialog } from '@angular/material/dialog';
import { RefreshComponent } from '../components/dialog/refresh/refresh.component';
import { Observable } from 'rxjs';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class RefreshService {
  /** Flag to display refresh needed screen */
  private _refresh: boolean = false;
  private _counter: number = 30;

  constructor(private tokenService: TokenService, private dialog: MatDialog, private userService: UserService) { }

  get refresh() {
    return this._refresh;
  }
  set refresh(value: boolean) {
    this._refresh = value;
    if (this._refresh) {
      this._counter = 30;
      this.openRefresh().subscribe(result => {
        this._refresh = false;
        if (result) {
          this.userService.refreshUser();
        } else {
          this.tokenService.clearCookies();
        }
      });
    } else {
      this.dialog.closeAll();
    }
  }

  get counter() {
    return this._counter;
  }

  private decrementCounter() {
    if (!this._refresh) return;
    this._counter--;
    if (this._counter <= 0 || !this.tokenService.loggedIn) {
      this._refresh = false;
      this.tokenService.clearCookies();
    } else {
      setTimeout(() => this.decrementCounter(), 1000);
    }
  }

  private openRefresh(size: string = '40%'): Observable<boolean> {
    const dialogRef = this.dialog.open(RefreshComponent, {
      width: size,
    });
    return dialogRef.afterClosed();
  }
}
