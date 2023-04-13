import { Component, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FriendHubService } from 'src/app/services/friend-hub.service';
import { FriendService } from 'src/app/services/friend.service';
import { IFriendListCounter } from 'src/shared/friend.models';
import { IUserNameViewModel } from 'src/shared/user.models';
import { FriendAddDialogComponent } from '../../dialog/friend-add-dialog/friend-add-dialog.component';

@Component({
  selector: 'app-friend',
  templateUrl: './friend.component.html',
  styleUrls: ['./friend.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class FriendComponent {
  private _open: boolean = false;

  constructor(
    private friendHubService: FriendHubService,
    private friendService: FriendService,
    private dialog: MatDialog
  ) {}

  public get connected(): boolean {
    return this.friendHubService.connected;
  }

  public get connecting(): boolean {
    return this.friendHubService.connecting;
  }

  public get open(): boolean {
    return this._open;
  }

  public set open(value: boolean) {
    this._open = value;
  }

  public switchOpen(): void {
    this._open = !this._open;
  }

  public get friends(): IUserNameViewModel[] {
    return this.friendService.friends?.friends || [];
  }

  public get sent(): IUserNameViewModel[] {
    return this.friendService.friends?.sent || [];
  }

  public get received(): IUserNameViewModel[] {
    return this.friendService.friends?.received || [];
  }

  public get friendsCount(): number {
    return this.friendService.friendsCounter.friends.length;
  }

  public get sentCount(): number {
    return this.friendService.friendsCounter.sent.length;
  }

  public get receivedCount(): number {
    return this.friendService.friendsCounter.received.length;
  }

  public declineFriend(friend: IUserNameViewModel): void {
    this.friendService.removeFriendAndRefresh(friend.id);
  }

  public addFriend(userName: string): void {
    this.friendService.addFriendAndRefresh(userName);
  }

  public openAddFriend(): void {
    const dialogRef = this.dialog.open(FriendAddDialogComponent);
    dialogRef.afterClosed().subscribe((name: string) => {
      if (name) {
        this.addFriend(name);
      }
    });
  }

  public getHeaderClass(counter: number | undefined): string {
    const open = this._open ? 'open' : '';
    if (counter === undefined) {
      const notified =
        this.friendsCount + this.sentCount + this.receivedCount > 0
          ? 'notified'
          : '';
      return `${notified} ${open}`;
    }
    const notified = counter > 0 ? 'notified' : '';
    return `${notified} ${open}`;
  }

  public clearCount(key: string): void {
    this.friendService.friendsCounter[key as keyof IFriendListCounter] = [];
  }
}
