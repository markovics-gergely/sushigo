import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import {
  IFriendListCounter,
  IFriendListViewModel,
  IFriendStatusViewModel,
} from 'src/shared/models/friend.models';
import { IUserNameViewModel } from 'src/shared/models/user.models';
import { BaseServiceService } from '../../../shared/services/abstract/base-service.service';

@Injectable({
  providedIn: 'root',
})
export class FriendService extends BaseServiceService {
  protected override readonly basePath: string = 'friend';
  private _friends: IFriendListViewModel | undefined;
  private _friendsCounter: IFriendListCounter = {
    friends: [],
    sent: [],
    received: [],
  };
  private _online: Set<string> = new Set();

  public loadFriends(): Subscription {
    return this.client
      .get<IFriendListViewModel>(this.baseUrl)
      .subscribe((friends: IFriendListViewModel) => {
        this._friends = friends;
      });
  }

  public get friends(): IFriendListViewModel | undefined {
    return {
      friends: this._friends?.friends.map((f) => ({ ...f, status: this._online.has(f.id) })) ?? [],
      sent: this._friends?.sent.map((f) => ({ ...f, status: this._online.has(f.id) })) ?? [],
      received: this._friends?.received.map((f) => ({ ...f, status: this._online.has(f.id) })) ?? [],
    };
  }

  public get friendsCounter(): IFriendListCounter {
    return this._friendsCounter;
  }

  public get online(): Set<string> {
    return this._online;
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
    if (!this._friends) return;
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
    statuses.forEach((status) => {
      if (status.status) {
        this._online.add(status.id);
      } else {
        this._online.delete(status.id);
      }
    });
  }
}
