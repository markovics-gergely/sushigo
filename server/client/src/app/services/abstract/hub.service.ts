import { Injectable, Injector } from '@angular/core';
import { TokenService } from '../token.service';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export abstract class HubService {
  protected abstract readonly baseUrl: string;
  protected abstract addListeners(): void;
  protected abstract onHubConnected?(): void;
  protected abstract onStartConnection?(): void;

  protected tokenService: TokenService;
  private _hubConnection: signalR.HubConnection | undefined;
  private _connected: boolean = false;
  private _connecting: boolean = false;

  constructor(injector: Injector) {
    this.tokenService = injector.get(TokenService);
  }

  public startConnection(): void {
    this.onStartConnection?.();
    if (this._connected) return;
    this._connecting = true;
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}?token=${this.tokenService.token}`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => this.tokenService.token,
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.addListeners();
    this._hubConnection
      .start()
      .then(() => this.onHubConnected?.())
      .catch((err) => {
        console.log('Error while starting connection: ' + err);
        this._connecting = false;
      });
  }

  public stopConnection(): void {
    if (!this._connected) return;
    this._hubConnection?.stop().then(() => {
      this._connected = false;
    });
  }

  protected get hubConnection(): signalR.HubConnection | undefined {
    return this._hubConnection;
  }

  public get connected(): boolean {
    return this._connected;
  }

  protected set connected(value: boolean) {
    this._connected = value;
  }

  public get connecting(): boolean {
    return this._connecting;
  }

  protected set connecting(value: boolean) {
    this._connecting = value;
  }
}
