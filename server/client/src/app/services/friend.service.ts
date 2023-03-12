import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IFriendListCounter, IFriendListViewModel, IFriendStatusViewModel } from 'src/shared/friend.models';
import { IUserNameViewModel } from 'src/shared/user.models';

@Injectable({
  providedIn: 'root',
})
export class FriendService {
  private readonly baseUrl: string = /*`${environment.baseUrl}/friend`*/ 'http://localhost:5200/friend';
  private _friends: IFriendListViewModel | undefined;
  private _friendsCounter: IFriendListCounter = { friends: 0, sent: 0, received: 0 };

  constructor(private client: HttpClient) { }

  public loadFriends(): Subscription {
    return this.client
      .get<IFriendListViewModel>(this.baseUrl)
      .subscribe((friends: IFriendListViewModel) => {this._friends = friends; console.log(friends);});
  }

  public get friends(): IFriendListViewModel | undefined {
    return this._friends;
  }

  private addFriend(userName: string): Observable<any> {
    return this.client.post(`${this.baseUrl}/${userName}`, {});
  }

  public addFriendToList(friend: IUserNameViewModel): void {
    if (!this._friends) { return; }
    const index = this._friends.received.findIndex((f) => f.id === friend.id);
    if (index >= 0) {
      this._friends.received.splice(index, 1);
      this._friends.friends.push(friend);
    }
  }

  public addFriendAndRefresh(userName: string): void {
    this.addFriend(userName).subscribe({
      next: (user: IUserNameViewModel) => this.addFriendToList(user),
      error: (err) => console.log(err),
    });
  }

  private removeFriend(id: string): Observable<any> {
    return this.client.delete(`${this.baseUrl}/${id}`);
  }

  public removeFriendFromList(id: string): void {
    if (!this._friends) { return; }
    this._friends.friends = this._friends.friends.filter((f) => f.id !== id);
    this._friends.sent = this._friends.sent.filter((f) => f.id !== id);
    this._friends.received = this._friends.received.filter((f) => f.id !== id);
  }

  public removeFriendAndRefresh(id: string): void {
    this.removeFriend(id).subscribe({
      next: () => this.removeFriendFromList(id),
      error: (err) => console.log(err),
    });
  }

  public loadStatuses(statuses: Array<IFriendStatusViewModel>): void {
    this.loadStatusToList(this._friends?.friends || [], statuses);
    this.loadStatusToList(this._friends?.sent || [], statuses);
    this.loadStatusToList(this._friends?.received || [], statuses);
  }

  public loadStatus(status: IFriendStatusViewModel): void {
    const array = new Array<IFriendStatusViewModel>();
    array.push(status);
    this.loadStatuses(array);
  }

  private loadStatusToList(list: Array<IUserNameViewModel>, statuses: Array<IFriendStatusViewModel>): void {
    list.forEach((friend) => {
      const status = statuses.find((s) => s.id === friend.id);
      friend.status = status?.status;
    });
  }
}
