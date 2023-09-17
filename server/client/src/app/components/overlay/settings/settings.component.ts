import { Component, ViewEncapsulation } from '@angular/core';
import { AbstractOpenableComponent } from '../abstract/abstract-openable.component';
import { Setting, Settings, SettingsType } from 'src/shared/settings.models';
import { ConfirmService } from 'src/app/services/confirm.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { GameService } from 'src/app/services/game.service';

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
    private userService: UserService,
    private gameService: GameService,
  ) { super(); }

  public get closeStyle(): { [klass: string]: any; } {
    return this.getCloseStyle(this.settings);
  }

  public get settings(): SettingsType[] {
    return Settings.filter((setting) => this.isAllowed(setting.settings));
  }

  public isAllowed(setting: Setting): boolean {
    switch (setting) {
      case 'logout':
        return true;
      case 'edit-user':
        return true;
      case 'remove-game':
        return this.gameService.isFirst;
      case 'delete-user':
        return true;
      default:
        return false;
    }
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
      case 'remove-game':
        this.removeGame();
        break;
      case 'delete-user':
        this.deleteUser();
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

  private deleteUser() {
    this.confirmService
      .confirm('delete', '250px')
      .subscribe((result: boolean) => {
        if (result) {
          this.userService.deleteUser().add(() => {
            this.tokenService.clearCookies();
          });
        }
      });
  }

  private edit() {
    this.userService.startEdit();
  }

  private removeGame() {
    this.gameService.removeGame();
  }
}
