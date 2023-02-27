import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { IFriendListViewModel, IFriendStatusViewModel } from 'src/shared/friend.models';
import { IUserNameViewModel } from 'src/shared/user.models';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class FriendService {
  private readonly baseUrl: string = `${environment.baseUrl}/friend`;
  private _friends: IFriendListViewModel | undefined;

  constructor(private client: HttpClient) {}

  public loadFriends(): void {
    this.client
      .get<IFriendListViewModel>(this.baseUrl)
      .subscribe((friends) => (this._friends = friends));
  }

  public get friends(): IFriendListViewModel | undefined {
    return this._friends;
  }

  public removeFriendFromList(id: string): void {
    if (!this._friends) { return; }
    this._friends.friends = this._friends.friends.filter((f) => f.id !== id);
    this._friends.sent = this._friends.sent.filter((f) => f.id !== id);
    this._friends.received = this._friends.received.filter((f) => f.id !== id);
  }

  public addFriendToList(friend: IUserNameViewModel): void {
    if (!this._friends) { return; }
    const index = this._friends.received.findIndex((f) => f.id === friend.id);
    if (index >= 0) {
      this._friends.received.splice(index, 1);
      this._friends.friends.push(friend);
    }
  }

  public loadStatus(status: IFriendStatusViewModel): void {
    this.loadStatuses([status]);
  }

  public loadStatuses(statuses: Array<IFriendStatusViewModel>): void {
    this.loadStatusToList(this._friends?.friends || [], statuses);
    this.loadStatusToList(this._friends?.sent || [], statuses);
    this.loadStatusToList(this._friends?.received || [], statuses);
  }

  private loadStatusToList(list: Array<IUserNameViewModel>, statuses: Array<IFriendStatusViewModel>): void {
    list.forEach((friend) => {
      const status = statuses.find((s) => s.id === friend.id);
      if (status) {
        friend.status = status.status;
      }
    });
  }
}
