import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  IFriendListCounter,
  IFriendListViewModel,
  IFriendStatusViewModel,
} from 'src/shared/friend.models';
import { IUserNameViewModel } from 'src/shared/user.models';

@Injectable({
  providedIn: 'root',
})
export class FriendService {
  private readonly baseUrl: string = `${environment.baseUrl}/friend`;
  private _friends: IFriendListViewModel | undefined;
  private _friendsCounter: IFriendListCounter = {
    friends: [],
    sent: [],
    received: [],
  };

  constructor(private client: HttpClient) {}

  public loadFriends(): Subscription {
    return this.client
      .get<IFriendListViewModel>(this.baseUrl)
      .subscribe((friends: IFriendListViewModel) => {
        this._friends = friends;
      });
  }

  public get friends(): IFriendListViewModel | undefined {
    return this._friends;
  }

  public get friendsCounter(): IFriendListCounter {
    return this._friendsCounter;
  }

  private addFriend(userName: string): Observable<IUserNameViewModel> {
    return this.client.post<IUserNameViewModel>(
      `${this.baseUrl}/${userName}`,
      {}
    );
  }

  public receiveFriendRequestSuccess(friend: IUserNameViewModel): void {
    if (!this._friends) {
      return;
    }
    const index = this._friends.sent.findIndex((f) => f.id === friend.id);
    if (index >= 0) {
      this._friends.sent.splice(index, 1);
      this._friends.friends.push(friend);
      this._friendsCounter.friends.push(friend.id);
    } else {
      this._friends.received.push(friend);
      this._friendsCounter.received.push(friend.id);
    }
  }

  private sendFriendRequestSuccess(friend: IUserNameViewModel): void {
    if (!this._friends) {
      return;
    }
    const index = this._friends.received.findIndex((f) => f.id === friend.id);
    if (index >= 0) {
      this._friends.received.splice(index, 1);
      this._friends.friends.push(friend);
      this._friendsCounter.friends.push(friend.id);
    } else {
      this._friends.sent.push(friend);
      this._friendsCounter.sent.push(friend.id);
    }
  }

  public addFriendAndRefresh(userName: string): void {
    this.addFriend(userName).subscribe({
      next: (user: IUserNameViewModel) => this.sendFriendRequestSuccess(user),
      error: (err) => console.log(err),
    });
  }

  private removeFriend(id: string): Observable<any> {
    return this.client.delete(`${this.baseUrl}/${id}`);
  }

  public removeFriendFromList(id: string): void {
    Object.keys(this._friends!).forEach((k) => {
      const key = k as keyof IFriendListViewModel;
      this._friends![key] = this.filterFriends(this._friends![key], id);
    });
    Object.keys(this._friendsCounter).forEach((k) => {
      const key = k as keyof IFriendListCounter;
      this._friendsCounter[key] = this.filterFriendsCounter(
        this._friendsCounter[key],
        id
      );
    });
  }

  private filterFriends(
    list: Array<IUserNameViewModel>,
    id: string
  ): Array<IUserNameViewModel> {
    return list.filter((f) => f.id !== id);
  }

  private filterFriendsCounter(list: Array<string>, id: string): Array<string> {
    return list.filter((f) => f !== id);
  }

  public removeFriendAndRefresh(id: string): void {
    this.removeFriend(id).subscribe({
      next: () => this.removeFriendFromList(id),
      error: (err) => console.log(err),
    });
  }

  public loadStatuses(statuses: Array<IFriendStatusViewModel>): void {
    if (!this._friends) return;
    const friends = [
      ...this._friends.friends,
      ...this._friends.sent,
      ...this._friends.received,
    ].flat();
    friends.forEach((friend) => {
      const status = statuses.find((s) => s.id === friend.id);
      friend.status = status?.status;
    });
  }
}
