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
  private expires: Date | undefined;

  constructor(private cookieService: CookieService) {}

  public set userToken(user: IUser) {
    this.token = user.access_token;
    this.refreshToken = user.refresh_token;
    this.expiresAt = user.expires_in;
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

  public get refreshToken(): string {
    return this.cookieService.get(this.rCookieName);
  }

  private set expiresAt(expires: number) {
    this.expires = new Date(new Date().getTime() + expires * 1000);
  }

  public get user(): IUserViewModel {
    return jwt_decode(this.token);
  }

  public clearCookies() {
    this.cookieService.delete(this.cookieName);
    this.cookieService.delete(this.rCookieName);
  }
}
