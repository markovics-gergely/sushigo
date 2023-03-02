import { Component, OnInit } from '@angular/core';
import { FriendHubService } from 'src/app/services/friend-hub.service';
import { FriendService } from 'src/app/services/friend.service';
import { IUserNameViewModel } from 'src/shared/user.models';

@Component({
  selector: 'app-friend',
  templateUrl: './friend.component.html',
  styleUrls: ['./friend.component.scss']
})
export class FriendComponent implements OnInit {
  private _open: boolean = false;

  constructor(private friendHubService: FriendHubService, private friendService: FriendService) { }

  ngOnInit(): void {
    this.friendHubService.startConnection();
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
}
