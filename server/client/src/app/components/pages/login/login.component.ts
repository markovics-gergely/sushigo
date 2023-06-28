import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from 'src/app/services/loading.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { ILoginUserDTO } from 'src/shared/user.models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class LoginComponent implements OnInit {
  private _loginForm: FormGroup | undefined;

  constructor(
    private userService: UserService,
    private loadingService: LoadingService,
    private router: Router,
    private route: ActivatedRoute,
    private tokenService: TokenService) { }

  ngOnInit(): void {
    this._loginForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }

  public get loginForm(): FormGroup | undefined {
    return this._loginForm;
  }

  public login(): void {
    if (!this.loginForm?.valid) { return; }
    this.loadingService.start();
    const loginUserDTO = this.loginForm.value as ILoginUserDTO;
    this.userService
      .login(loginUserDTO)
      .subscribe({
        next: (response) => {
          this.loginForm?.reset();
          this.tokenService.userToken = response;
          this.router.navigateByUrl(this.route.snapshot.queryParams['returnUrl'] || 'home').catch(console.error);
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
