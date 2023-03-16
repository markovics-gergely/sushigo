import { Component, OnInit } from '@angular/core';
import { AclService } from 'src/app/services/acl.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';
import { IUserViewModel } from 'src/shared/user.models';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  private _user: IUserViewModel | undefined;

  constructor(private userService: UserService, private tokenService: TokenService, private aclService: AclService) { }

  ngOnInit(): void {
    this.userService.user.subscribe((user: IUserViewModel) => {
      this._user = user;
      this._user.mode = this.tokenService.roles.includes('Party') ? 'party' : 'classic';
    });
  }

  public get user(): IUserViewModel | undefined {
    return this._user;
  }

  public get avatarUrl(): string | undefined {
    return this._user ? `${environment.baseUrl}${this._user.avatar}?token=${this.tokenService.token}` : undefined;
  }
}
