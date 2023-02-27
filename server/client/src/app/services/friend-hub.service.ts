import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HttpClient } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { IFriendStatusViewModel } from 'src/shared/friend.models';
import { FriendService } from './friend.service';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class FriendHubService {
  private readonly baseUrl: string = `${environment.baseUrl}/friend-hub`;
  private _hubConnection: signalR.HubConnection | undefined;

  constructor(private tokenService: TokenService, private friendService: FriendService) {}

  public startConnection(): void {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}?token=${this.tokenService.token}`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => this.tokenService.token,
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this._hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  }

  public addListeners(): void {
    this._hubConnection?.on('FriendRequest', (data) => {
      console.log(data);
    });
    this._hubConnection?.on('FriendRemove', (data) => {
      console.log(data);
    });
    this._hubConnection?.on('FriendStatuses', this.friendService.loadStatuses);
    this._hubConnection?.on('FriendStatus', this.friendService.loadStatus);
  }
}
