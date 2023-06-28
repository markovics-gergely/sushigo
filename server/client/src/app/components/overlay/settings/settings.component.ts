import { Component, ViewEncapsulation } from '@angular/core';
import { AbstractOpenableComponent } from '../abstract/abstract-openable.component';
import { Setting, Settings, SettingsType } from 'src/shared/settings.models';
import { ConfirmService } from 'src/app/services/confirm.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SettingsComponent extends AbstractOpenableComponent {
  constructor(
    private confirmService: ConfirmService,
    private tokenService: TokenService,
    private userService: UserService
  ) { super(); }

  public get closeStyle(): { [klass: string]: any; } {
    return this.getCloseStyle(this.settings);
  }

  public get settings(): SettingsType[] {
    return Settings;
  }

  public onSelect(val: Setting) {
    this.switchOpen();
    switch (val) {
      case 'logout':
        this.logout();
        break;
      case 'edit-user':
        this.edit();
        break;
      default:
        break;
    }
  }

  private logout() {
    this.confirmService
      .confirm('logout', '250px')
      .subscribe((result: boolean) => {
        if (result) {
          this.tokenService.clearCookies();
        }
      });
  }

  private edit() {
    this.userService.startEdit();
  }
}
