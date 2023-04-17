import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { IFriendStatusViewModel } from 'src/shared/friend.models';
import { IUserNameViewModel } from 'src/shared/user.models';
import { FriendService } from './friend.service';
import { TokenService } from './token.service';
import { RefreshService } from './refresh.service';

@Injectable({
  providedIn: 'root',
})
export class FriendHubService {
  private readonly baseUrl: string = `${environment.baseUrl}/user-hubs/friend-hub`;
  private _hubConnection: signalR.HubConnection | undefined;
  private _connected: boolean = false;
  private _connecting: boolean = false;

  constructor(
    private tokenService: TokenService,
    private friendService: FriendService,
    private refreshService: RefreshService
  ) {}

  public startConnection(): void {
    this.refreshService.refreshUser();
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
      .then(() => {
        this.friendService.loadFriends().add(() => {
          this._connected = true;
          this._connecting = false;
        });
      })
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

  public addListeners(): void {
    this._hubConnection?.on('FriendRequest', (data: IUserNameViewModel) =>
      this.friendService.receiveFriendRequestSuccess(data)
    );
    this._hubConnection?.on('FriendRemove', (data: IUserNameViewModel) =>
      this.friendService.removeFriendFromList(data.id)
    );
    this._hubConnection?.on(
      'FriendStatuses',
      (statuses: Array<IFriendStatusViewModel>) => {
        this.friendService.loadStatuses(statuses);
      }
    );
    this._hubConnection?.on(
      'FriendStatus',
      (status: IFriendStatusViewModel) => {
        this.friendService.loadStatuses([status]);
      }
    );
    this._hubConnection?.on('RefreshUser', () => {
      this.refreshService.refreshUser();
    });
  }

  public get connected(): boolean {
    return this._connected;
  }

  public get connecting(): boolean {
    return this._connecting;
  }
}
