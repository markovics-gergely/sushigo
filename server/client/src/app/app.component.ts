import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { TokenService } from './services/token.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Sushi Go!';

  constructor(private tokenService: TokenService, translate: TranslateService) {
    translate.setDefaultLang('en');
    translate.use('en');
  }

  public get loggedIn(): boolean {
    return this.tokenService.loggedIn;
  }
}
