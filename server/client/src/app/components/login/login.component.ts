import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FriendService } from 'src/app/services/friend.service';
import { LoadingService } from 'src/app/services/loading.service';
import { SnackService } from 'src/app/services/snack.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { ILoginUserDTO, IUser } from 'src/shared/user.models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  private _loginForm: FormGroup | undefined;

  constructor(
    private userService: UserService,
    private loadingService: LoadingService,
    private router: Router,
    private snackService: SnackService,
    private tokenService: TokenService) { }

  ngOnInit(): void {
    this._loginForm = new FormGroup({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }

  public get loginForm(): FormGroup | undefined {
    return this._loginForm;
  }

  public login(): void {
    if (!this.loginForm?.valid) { return; }
    this.loadingService.loading = true;
    let loginUserDTO: ILoginUserDTO = {
      username: this.loginForm.get('userName')?.value,
      password: this.loginForm.get('password')?.value,
    };
    this.userService
        .login(loginUserDTO)
        .subscribe({
          next: (response) => {
            this.tokenService.userToken = response as IUser;
            this.router.navigate(['home']);
          },
          error: (err) => {
            console.log(err);
            this.snackService.openSnackBar(err.statusText, 'OK');
            this.tokenService.clearCookies();
          },
        })
        .add(() => {
          this.loadingService.loading = false;
        });
      this.loginForm.reset();
  }
}
