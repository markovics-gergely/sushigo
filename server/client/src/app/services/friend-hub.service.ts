import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { IFriendStatusViewModel } from 'src/shared/friend.models';
import { IUserNameViewModel } from 'src/shared/user.models';
import { FriendService } from './friend.service';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class FriendHubService {
  private readonly baseUrl: string = `${environment.baseUrl}/friend-hub`;
  private _hubConnection: signalR.HubConnection | undefined;
  private _connected: boolean = false;

  constructor(private tokenService: TokenService, private friendService: FriendService) { }

  public startConnection(): void {
    if (this._connected) return;
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
        this._connected = true;
      })
      .catch((err) => console.log('Error while starting connection: ' + err));
  }

  public addListeners(): void {
    this._hubConnection?.on('FriendRequest', (data: IUserNameViewModel) => this.friendService.receiveFriendRequestSuccess(data));
    this._hubConnection?.on('FriendRemove', (data: { sender: string }) => this.friendService.removeFriendFromList(data.sender));
    this._hubConnection?.on('FriendStatuses', (statuses: Array<IFriendStatusViewModel>) => { this.friendService.loadStatuses(statuses); });
    this._hubConnection?.on('FriendStatus', (status: IFriendStatusViewModel) => { this.friendService.loadStatuses([status]); });
  }

  public get connected(): boolean {
    return this._connected;
  }
}
