import { Component } from '@angular/core';
import { ThemeService } from 'src/app/overlay/services/theme.service';
import { ThemeType } from 'src/shared/models/theme.models';
import { AbstractOpenableComponent } from '../abstract-openable.component';

@Component({
  selector: 'app-theme',
  templateUrl: './theme.component.html',
  styleUrls: ['./theme.component.scss']
})
export class ThemeComponent extends AbstractOpenableComponent {

  constructor(private themeService: ThemeService) {
    super();
  }

  public onSelect(val: string) {
    this.themeService.theme = val;
  }

  public get themes(): ThemeType[] {
    return this.themeService.themes;
  }

  public get currentTheme(): string | undefined {
    return this.themeService.theme;
  }

  public get closeStyle(): { [klass: string]: any; } {
    return this.open ? { right: "0px" } : { right: `-${this.themes.length * 50 + 2}px` };
  }

}
