import { Component } from '@angular/core';
import { LanguagesService } from 'src/app/services/languages.service';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.scss']
})
export class LanguageComponent {
  private _open: boolean = false;

  constructor(private languagesService: LanguagesService) { }

  public onSelect(lang: string) {
    this.languagesService.language = lang;
  }

  public get langs(): string[] {
    return this.languagesService.languages;
  }

  public get currentLang(): string {
    return this.languagesService.language ?? this.languagesService.defaultLanguage;
  }

  public get open(): boolean {
    return this._open;
  }

  public switchOpen(): void {
    this._open = !this._open;
  }

  public get closeStyle(): { [klass: string]: any; } {
    return this._open ? { right: "0px" } : { right: `-${this.langs.length * 50 + 2}px` };
  }
}
