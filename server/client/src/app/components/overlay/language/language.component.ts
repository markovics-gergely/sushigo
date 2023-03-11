import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.scss']
})
export class LanguageComponent {
  constructor(private translate: TranslateService) { }

  public onSelect(lang: string) {
    this.translate.use(lang)
  }

  public get langs(): string[] {
    return this.translate.getLangs();
  }

  public get currentLang(): string {
    return this.translate.currentLang;
  }
}
