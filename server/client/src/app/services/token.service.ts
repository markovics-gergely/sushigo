import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { IUser, IUserViewModel } from 'src/shared/user.models';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly cookieName = 'sushitoken';
  private readonly rCookieName = 'rsushitoken';

  constructor(private cookieService: CookieService) {}

  public set userToken(user: IUser) {
    this.token = user.access_token;
    this.refreshToken = user.refresh_token;
  }

  private set token(token: string) {
    this.cookieService.set(this.cookieName, token, this.expires);
  }

  public get token(): string {
    return this.cookieService.get(this.cookieName);
  }

  private set refreshToken(token: string) {
    this.cookieService.set(this.rCookieName, token, this.expires);
  }

  public get refreshToken1(): string {
    return this.cookieService.get(this.rCookieName);
  }

  public get user(): IUserViewModel {
    return jwt_decode(this.token);
  }

  public get expires(): Date | undefined {
    return this.user?.exp ? new Date(this.user.exp * 1000) : undefined;
  }

  public get userId(): string {
    return this.user.sub;
  }

  public clearCookies() {
    this.cookieService.delete(this.cookieName);
    this.cookieService.delete(this.rCookieName);
  }

  private get notExpired(): boolean {
    return this.expires !== undefined && this.expires > new Date();
  }

  public get loggedIn(): boolean {
    return this.token !== '' && this.notExpired;
  }
}
