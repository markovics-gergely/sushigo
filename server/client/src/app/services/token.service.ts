import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AppRole, IUser, IUserTokenViewModel } from 'src/shared/models/user.models';
import jwt_decode from 'jwt-decode';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { DeckType } from 'src/shared/models/deck.models';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly cookieName = environment.token_name;
  private readonly rCookieName = environment.refresh_token_name;
  private readonly langCookieName = environment.language_token_name;
  private readonly themeCookieName = environment.theme_token_name;

  private _userEventEmitter = new BehaviorSubject<IUserTokenViewModel | undefined>(undefined);
  public get userEventEmitter(): Observable<IUserTokenViewModel | undefined> { return this._userEventEmitter; }
  
  constructor(
    private cookieService: CookieService,
    private router: Router,
  ) { }

  public set userToken(user: IUser) {
    this.token = user.access_token;
    this.refreshToken = user.refresh_token;
    if (this.user?.lobby) {
      this.router.navigateByUrl(`/lobby/${this.user.lobby}`);
    } else if(this.router.url.startsWith('/lobby/')) {
      this.router.navigateByUrl('/lobby');
    }
    if (this.user?.game) {
      this.router.navigateByUrl(`/game/${this.user.game}`);
    }
  }

  private set token(token: string) {
    this._userEventEmitter.next(jwt_decode(token));
    this.cookieService.set(this.cookieName, token);
  }

  public get token(): string {
    return this.cookieService.get(this.cookieName);
  }

  private set refreshToken(token: string) {
    this.cookieService.set(this.rCookieName, token);
  }

  public get refreshToken(): string {
    return this.cookieService.get(this.rCookieName);
  }

  public get user(): IUserTokenViewModel | undefined {
    if (this._userEventEmitter.value === undefined) {
      const token = this.token;
      if (token) {
        this._userEventEmitter.next(jwt_decode(token));
      }
    }
    return this._userEventEmitter.value;
  }

  public get expires(): Date | undefined {
    return this.user?.exp ? new Date(this.user.exp * 1000) : undefined;
  }

  public get userId(): string | undefined {
    return this.user?.sub;
  }

  public get userName(): string | undefined {
    return this.user?.name;
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
    this.router.navigateByUrl('login').catch(console.error);
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

  public get lobby(): string | undefined {
    return this.user?.lobby;
  }

  public ownLobby(lobbyId?: string): boolean {
    if (!lobbyId || !this.user?.lobby) return false;
    return this.user.lobby === lobbyId;
  }

  public ownPlayer(playerId?: string): boolean {
    return this.user?.player == playerId;
  }

  public get game(): string | undefined {
    return this.user?.game;
  }

  public get player(): string | undefined {
    return this.user?.player;
  }
}
