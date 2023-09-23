import { Injectable } from '@angular/core';
import { TokenService } from './token.service';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpBackend, HttpClient, HttpHeaders } from '@angular/common/http';
import { IRefreshViewModel } from 'src/shared/user.models';
import { LoadingService } from './loading.service';

@Injectable({
  providedIn: 'root',
})
export class RefreshService {
  /** Route of the user related endpoints */
  private readonly baseUrl: string = `${environment.baseUrl}/user`;
  private client: HttpClient;

  constructor(
    handler: HttpBackend,
    private tokenService: TokenService,
    private loadingService: LoadingService,
  ) {
    this.client = new HttpClient(handler);
  }

  /**
   * Refresh stored token with refresh token
   * @returns
   */
  private refreshToken(): Observable<IRefreshViewModel> {
    const headers = new HttpHeaders()
    .set('Content-Type', 'application/x-www-form-urlencoded')
    .set('Authorization', 'Bearer ' + this.tokenService.token);
  
    const body = new URLSearchParams();
    body.set('refresh_token', this.tokenService.refreshToken);
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

  public refreshUserWithoutLoading() {
    return this.refreshToken()
      .subscribe({
        next: (response) => {
          this.tokenService.userToken = response;
        },
        error: (err) => {
          console.log(err);
          this.tokenService.clearCookies();
        },
      });
  }
}
