import { Component } from '@angular/core';
import { TokenService } from './services/token.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Sushi Go!';

  constructor(private tokenService: TokenService) { }

  public get loggedIn(): boolean {
    return true;
    return this.tokenService.loggedIn;
  }
}
