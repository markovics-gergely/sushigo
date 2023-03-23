import { DOCUMENT } from '@angular/common';
import { Inject, Injectable, Renderer2, RendererFactory2 } from '@angular/core';
import { Themes, ThemeType } from 'src/shared/theme.models';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private renderer: Renderer2;

  constructor(private tokenService: TokenService, rendererFactory: RendererFactory2, @Inject(DOCUMENT) private document: Document) {
    this.renderer = rendererFactory.createRenderer(null, null);
   }

  public get theme(): string | undefined {
    return this.tokenService.theme;
  }

  public set theme(value: string | undefined) {
    if (value) {
      this.changeTheme(value, this.theme);
    }
    this.tokenService.theme = value;
  }

  public get themes(): ThemeType[] {
    return Themes;
  }

  private changeTheme(theme: string, oldTheme: string | undefined) {
    if (oldTheme) {
      this.renderer.removeClass(document.body, oldTheme);
    }
    this.renderer.addClass(document.body, theme);
  }

  public init() {
    this.theme = this.tokenService.theme || this.themes[0].theme;
  }
}
