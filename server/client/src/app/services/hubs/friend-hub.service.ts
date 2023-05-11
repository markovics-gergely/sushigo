import { Injectable, Injector } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IFriendStatusViewModel } from 'src/shared/friend.models';
import { IUserNameViewModel } from 'src/shared/user.models';
import { FriendService } from '../friend.service';
import { RefreshService } from '../refresh.service';
import { HubService } from './abstract/hub.service';

@Injectable({
  providedIn: 'root',
})
export class FriendHubService extends HubService {
  protected override readonly baseUrl: string = `${environment.baseUrl}/user-hubs/friend-hub`;

  constructor(
    injector: Injector,
    private friendService: FriendService,
    private refreshService: RefreshService
  ) {
    super(injector);
  }

  protected override onStartConnection(): void {
    this.refreshService.refreshUser();
  }

  protected override onHubConnected(): void {
    this.friendService.loadFriends().add(() => {
      this.connected = true;
      this.connecting = false;
    });
  }

  public override addListeners(): void {
    this.hubConnection?.on('FriendRequest', (data: IUserNameViewModel) =>
      this.friendService.receiveFriendRequestSuccess(data)
    );
    this.hubConnection?.on('FriendRemove', (data: IUserNameViewModel) =>
      this.friendService.removeFriendFromList(data.id)
    );
    this.hubConnection?.on(
      'FriendStatuses',
      (statuses: Array<IFriendStatusViewModel>) => {
        this.friendService.loadStatuses(statuses);
      }
    );
    this.hubConnection?.on(
      'FriendStatus',
      (status: IFriendStatusViewModel) => {
        this.friendService.loadStatuses([status]);
      }
    );
    this.hubConnection?.on('RefreshUser', () => {
      this.refreshService.refreshUser();
    });
  }
}
