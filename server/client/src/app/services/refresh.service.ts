import { Injectable } from '@angular/core';
import { TokenService } from './token.service';
import { MatDialog } from '@angular/material/dialog';
import { RefreshComponent } from '../components/dialog/refresh/refresh.component';
import { Observable } from 'rxjs';
import { UserService } from './user.service';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IUser } from 'src/shared/user.models';
import { LoadingService } from './loading.service';

@Injectable({
  providedIn: 'root',
})
export class RefreshService {
  /** Route of the user related endpoints */
  private readonly baseUrl: string = `${environment.baseUrl}/user`;
  /** Flag to display refresh needed screen */
  private _refresh: boolean = false;
  private _counter: number = 30;

  constructor(
    private tokenService: TokenService,
    private dialog: MatDialog,
    private client: HttpClient,
    private loadingService: LoadingService
  ) {}

  get refresh() {
    return this._refresh;
  }
  set refresh(value: boolean) {
    this._refresh = value;
    if (this._refresh) {
      this._counter = 30;
      this.openRefresh().subscribe((result) => {
        this._refresh = false;
        if (result) {
          this.refreshUser();
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

  /**
   * Refresh stored token with refresh token
   * @returns
   */
  private refreshToken(): Observable<IUser> {
    let headers = new HttpHeaders().set(
      'Content-Type',
      'application/x-www-form-urlencoded'
    );
    let body = new URLSearchParams();

    const token = this.tokenService.refreshToken;
    body.set('refresh_token', token);
    body.set('grant_type', 'refresh_token');
    body.set('client_id', environment.client_id);
    body.set('client_secret', environment.client_secret);

    return this.client.post<IUser>(`${this.baseUrl}/refresh`, body.toString(), {
      headers: headers,
    });
  }

  public refreshUser(): void {
    this.loadingService.start();
    this.refreshToken()
      .subscribe({
        next: (response) => {
          this.tokenService.userToken = response;
          setTimeout(() => {
            this._refresh = true;
          }, (response.expires_in - 30) * 1000);
        },
        error: (err) => {
          console.log(err);
          this.tokenService.clearCookies();
        },
      })
      .add(() => {
        this.loadingService.stop();
      });
  }
}
