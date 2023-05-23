import { Injectable } from '@angular/core';
import { TokenService } from './token.service';
import { MatDialog } from '@angular/material/dialog';
import { RefreshComponent } from '../components/dialog/refresh/refresh.component';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpBackend, HttpClient, HttpHeaders } from '@angular/common/http';
import { IRefreshViewModel } from 'src/shared/user.models';
import { LoadingService } from './loading.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class RefreshService {
  /** Route of the user related endpoints */
  private readonly baseUrl: string = `${environment.baseUrl}/user`;
  /** Flag to display refresh needed screen */
  //private _refresh: boolean = false;
  //private _counter: number = 30;W
  private client: HttpClient;

  constructor(
    private tokenService: TokenService,
    private dialog: MatDialog,
    handler: HttpBackend,
    private loadingService: LoadingService,
    private router: Router
  ) {
    this.client = new HttpClient(handler);
  }

  /*private decrementCounter() {
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
  }*/

  /**
   * Refresh stored token with refresh token
   * @returns
   */
  private refreshToken(): Observable<IRefreshViewModel> {
    let headers = new HttpHeaders().set(
      'Content-Type',
      'application/x-www-form-urlencoded'
    ).set('Authorization', 'Bearer ' + this.tokenService.token);
    let body = new URLSearchParams();

    const token = this.tokenService.refreshToken;
    body.set('refresh_token', token);
    body.set('grant_type', 'refresh_token');
    body.set('client_id', environment.client_id);
    body.set('client_secret', environment.client_secret);

    return this.client.post<IRefreshViewModel>(`${this.baseUrl}/refresh`, body.toString(), {
      headers: headers,
    });
  }

  public refreshUser(): void {
    this.loadingService.start();
    this.refreshToken()
      .subscribe({
        next: (response) => {
          this.tokenService.userToken = response;
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
