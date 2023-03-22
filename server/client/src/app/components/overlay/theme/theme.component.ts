import { Component } from '@angular/core';
import { ThemeService } from 'src/app/services/theme.service';
import { ThemeType } from 'src/shared/theme.models';

@Component({
  selector: 'app-theme',
  templateUrl: './theme.component.html',
  styleUrls: ['./theme.component.scss']
})
export class ThemeComponent {
  private _open: boolean = false;

  constructor(private themeService: ThemeService) { }

  public onSelect(val: string) {
    this.themeService.theme = val;
  }

  public get themes(): ThemeType[] {
    return this.themeService.themes;
  }

  public get currentTheme(): string | undefined {
    return this.themeService.theme;
  }

  public get open(): boolean {
    return this._open;
  }

  public switchOpen(): void {
    this._open = !this._open;
  }

  public get closeStyle(): { [klass: string]: any; } {
    return this._open ? { right: "0px" } : { right: `-${this.themes.length * 50 + 2}px` };
  }

}
