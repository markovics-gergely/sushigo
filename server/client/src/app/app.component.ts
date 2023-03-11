import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { TokenService } from './services/token.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Sushi Go!';

  constructor(private tokenService: TokenService, private translate: TranslateService) {
  }
  
  ngOnInit(): void {
    this.translate.setDefaultLang('en');
    this.translate.addLangs(['en', 'hu']);
    this.translate.use('en');
  }

  public get loggedIn(): boolean {
    return this.tokenService.loggedIn;
  }
}
