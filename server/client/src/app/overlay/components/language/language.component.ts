import { Component } from '@angular/core';
import { LanguagesService } from 'src/app/overlay/services/languages.service';
import { AbstractOpenableComponent } from '../abstract-openable.component';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.scss']
})
export class LanguageComponent extends AbstractOpenableComponent {

  constructor(private languagesService: LanguagesService) {
    super();
  }

  public onSelect(lang: string) {
    this.languagesService.language = lang;
  }

  public get langs(): string[] {
    return this.languagesService.languages;
  }

  public get currentLang(): string {
    return this.languagesService.language ?? this.languagesService.defaultLanguage;
  }

  public get closeStyle(): { [klass: string]: any; } {
    return this.open ? { right: "0px" } : { right: `-${this.langs.length * 50 + 2}px` };
  }
}
