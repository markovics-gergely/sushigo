import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ConfirmService } from 'src/shared/services/confirm.service';
import { LoadingService } from 'src/shared/services/loading.service';
import { ShopService } from 'src/app/services/shop.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { IUserViewModel } from 'src/shared/models/user.models';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class UserComponent implements OnInit {
  private _user: IUserViewModel | undefined;

  constructor(
    private userService: UserService,
    private tokenService: TokenService,
    private confirmService: ConfirmService,
    private loadingService: LoadingService,
    private shopService: ShopService
  ) {}

  ngOnInit(): void {
    this.userService.userEventEmitter.subscribe(
      (user: IUserViewModel | undefined) => {
        if (user && this._user) {
          user.avatarLoaded = this._user.avatarLoaded;
        }
        this._user = user;
      }
    );
    this.userService.refreshUser();
  }

  public get user(): IUserViewModel | undefined {
    return this._user;
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
    this.userService.startEdit();
  }

  public claimParty() {
    this.loadingService.start();
    this.shopService
      .claimParty()
      .subscribe()
      .add(() => { this.loadingService.stop(); });
  }

  public get partyClaimed(): boolean {
    return this.tokenService.roles.includes('Party');
  }
}
