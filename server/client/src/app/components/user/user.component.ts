import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmService } from 'src/app/services/confirm.service';
import { LoadingService } from 'src/app/services/loading.service';
import { ShopService } from 'src/app/services/shop.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';
import { IUserViewModel } from 'src/shared/user.models';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
})
export class UserComponent implements OnInit {
  private _user: IUserViewModel | undefined;

  constructor(
    private userService: UserService,
    private tokenService: TokenService,
    private confirmService: ConfirmService,
    private router: Router,
    private loadingService: LoadingService,
    private shopService: ShopService
  ) { }

  ngOnInit(): void {
    this.userService.user.subscribe((user: IUserViewModel) => {
      this._user = user;
    });
  }

  public get user(): IUserViewModel | undefined {
    return this._user;
  }

  public get avatarUrl(): string | undefined {
    return this._user
      ? `${environment.baseUrl}${this._user.avatar}?token=${this.tokenService.token}`
      : undefined;
  }

  public logout() {
    this.confirmService
      .confirm('logout', '250px')
      .subscribe((result: boolean) => {
        if (result) {
          this.tokenService.clearCookies();
        }
      });
  }

  public edit() {
    if (!this._user) return;
    this.userService.startEdit({
      userName: this._user.userName,
      firstName: this._user.name.split(" ")[0],
      lastName: this._user.name.split(" ")[1],
      avatar: undefined,
    }).subscribe({
      next: (user: IUserViewModel | undefined) => {
        if (!user) return;
        this._user = user;
      }
    });
  }

  public claimParty() {
    this.loadingService.start();
    this.shopService.claimParty().subscribe().add(() => this.loadingService.stop());
  }

  public get partyClaimed(): boolean {
    return this.tokenService.roles.includes('Party');
  }
}
