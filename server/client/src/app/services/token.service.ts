import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AppRole, IUser, IUserTokenViewModel } from 'src/shared/user.models';
import jwt_decode from 'jwt-decode';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { DeckType } from 'src/shared/deck.models';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly cookieName = environment.token_name;
  private readonly rCookieName = environment.refresh_token_name;
  private readonly langCookieName = environment.language_token_name;
  private readonly themeCookieName = environment.theme_token_name;
  private readonly DEFAULT_REFRESH_TIME = 30;

  constructor(private cookieService: CookieService, private router: Router) { }

  public set userToken(user: IUser) {
    this.setToken(user.access_token, user.expires_in);
    this.refreshToken = user.refresh_token;
  }

  private setToken(token: string, expires_in: number) {
    this.cookieService.set(this.cookieName, token, new Date(Date.now() + expires_in * 1000));
  }

  public get token(): string {
    return this.cookieService.get(this.cookieName);
  }

  private set refreshToken(token: string) {
    this.cookieService.set(this.rCookieName, token, this.DEFAULT_REFRESH_TIME);
  }

  public get refreshToken(): string {
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
    if (lang !== undefined) {
      this.cookieService.set(this.langCookieName, lang, 365);
    }
  }

  public get theme(): string | undefined {
    return this.cookieService.get(this.themeCookieName);
  }

  public set theme(theme: string | undefined) {
    if (theme !== undefined) {
      this.cookieService.set(this.themeCookieName, theme, 365);
    }
  }

  public clearCookies() {
    this.cookieService.delete(this.cookieName);
    this.cookieService.delete(this.rCookieName);
    this.router.navigateByUrl('login');
  }

  public clearAllCookies() {
    this.cookieService.deleteAll();
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

  public get decks(): DeckType[] {
    return this.user?.decks ?? [];
  }
}
