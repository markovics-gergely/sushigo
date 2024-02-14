import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { LoadingService } from 'src/shared/services/loading.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { ILoginUserDTO } from 'src/shared/models/user.models';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  constructor(
    private userService: UserService,
    private loadingService: LoadingService,
    private router: Router,
    private route: ActivatedRoute,
    private tokenService: TokenService) { }

  public login(loginForm: FormGroup): void {
    if (!loginForm.valid) { return; }

    this.loadingService.start();
    return this.userService
      .login(loginForm.value as ILoginUserDTO)
      .subscribe({
        next: (response) => {
          loginForm?.reset();
          this.tokenService.userToken = response;
          this.router
            .navigateByUrl(
              this.route.snapshot.queryParams['returnUrl'] || 'home'
            )
            .catch(console.error);
        },
        error: (err) => {
          console.log(err);
          this.tokenService.clearCookies();
        },
      })
      .add(() => {
        this.loadingService.stop();
      });
  }
}
