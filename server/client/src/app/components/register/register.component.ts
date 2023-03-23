import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingService } from 'src/app/services/loading.service';
import { UserService } from 'src/app/services/user.service';
import { IRegisterUserDTO } from 'src/shared/user.models';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  private _registerForm: FormGroup | undefined;

  constructor(
    private userService: UserService,
    private loadingService: LoadingService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this._registerForm = new FormGroup({
      userName: new FormControl('', Validators.required),
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required),
      confirmedPassword: new FormControl('', Validators.required),
    });
  }

  /**
   * Submit registration to the server
   */
  register(): void {
    if (this.registerForm) {
      this.loadingService.loading = true;
      const registerUserDTO = this.registerForm.value as IRegisterUserDTO;
      this.userService
        .registration(registerUserDTO)
        .subscribe({
          next: () => {
            this.registerForm?.reset();
            this.router.navigate(['login']);
          },
          error: (err) => {
            console.log(err);
          },
        })
        .add(() => {
          this.loadingService.loading = false;
        });
    }
  }

  get registerForm(): FormGroup | undefined {
    return this._registerForm;
  }
}
