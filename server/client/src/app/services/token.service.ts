import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AppRole, IUser, IUserTokenViewModel } from 'src/shared/user.models';
import jwt_decode from 'jwt-decode';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly cookieName = environment.token_name;
  private readonly rCookieName = environment.refresh_token_name;
  private readonly langCookieName = environment.language_token_name;
  private readonly themeCookieName = environment.theme_token_name;

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

  public get user(): IUserTokenViewModel | undefined {
    return this.token ? jwt_decode(this.token) : undefined;
  }

  public get expires(): Date | undefined {
    return this.user?.exp ? new Date(this.user.exp * 1000) : undefined;
  }

  public get userId(): string | undefined {
    return this.user?.sub;
  }

  public get language(): string | undefined {
    return this.cookieService.get(this.langCookieName);
  }

  public set language(lang: string | undefined) {
    this.cookieService.set(this.langCookieName, lang ?? "");
  }

  public get theme(): string | undefined {
    return this.cookieService.get(this.themeCookieName);
  }

  public set theme(theme: string | undefined) {
    this.cookieService.set(this.themeCookieName, theme ?? "");
  }

  public clearCookies() {
    this.cookieService.delete(this.cookieName);
    this.cookieService.delete(this.rCookieName);
    this.cookieService.delete(this.langCookieName);
    this.cookieService.delete(this.themeCookieName);
  }

  private get notExpired(): boolean {
    return this.expires !== undefined && this.expires > new Date();
  }

  public get loggedIn(): boolean {
    return this.cookieService.check(this.cookieName) && this.notExpired;
  }

  public get roles(): AppRole[] {
    const values = this.user?.role ?? [];
    return Array.isArray(values) ? values : [values];
  }
}
