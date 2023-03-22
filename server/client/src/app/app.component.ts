import { Component, OnInit } from '@angular/core';
import { LanguagesService } from './services/languages.service';
import { ThemeService } from './services/theme.service';
import { TokenService } from './services/token.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Sushi Go!';

  constructor(private tokenService: TokenService, private languageService: LanguagesService, private themeService: ThemeService) {
  }
  
  ngOnInit(): void {
    this.themeService.init();
    this.languageService.init();
  }

  public get loggedIn(): boolean {
    return this.tokenService.loggedIn;
  }
}
