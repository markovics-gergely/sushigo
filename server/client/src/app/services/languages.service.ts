import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class LanguagesService {

  constructor(
    private tokenService: TokenService,
    private translateService: TranslateService
  ) { }

  public get language(): string | undefined {
    return this.translateService.currentLang;
  }

  public set language(lang: string | undefined) {
    this.tokenService.language = lang;
    this.translateService.use(lang ?? this.defaultLanguage);
  }

  public get languages(): string[] {
    return this.translateService.getLangs();
  }

  public get defaultLanguage(): string {
    const token = this.tokenService.language;
    if (token && this.languages.includes(token)) {
      return token;
    }
    const browser = this.translateService.getBrowserLang();
    if (browser && this.languages.includes(browser)) {
      return browser;
    }
    const defaultLanguage = this.translateService.getDefaultLang();
    if (defaultLanguage && this.languages.includes(defaultLanguage)) {
      return defaultLanguage;
    }
    return 'en';
  }

  public init(): void {
    this.translateService.setDefaultLang(this.defaultLanguage);
    this.translateService.addLangs(['en', 'hu']);
    this.translateService.use(this.defaultLanguage);
  }
}
