import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { LoadingService } from 'src/shared/services/loading.service';
import { UserService } from 'src/app/services/user.service';
import { IRegisterUserDTO } from 'src/shared/models/user.models';

@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  constructor(
    private userService: UserService,
    private loadingService: LoadingService,
    private router: Router
  ) {}

  register(registerForm: FormGroup): void {
    if (!registerForm.valid) {
      return;
    }
    this.loadingService.start();
    this.userService
      .registration(registerForm.value as IRegisterUserDTO)
      .subscribe({
        next: () => {
          registerForm?.reset();
          this.router.navigate(['login']).catch(console.error);
        },
        error: console.error,
      })
      .add(() => {
        this.loadingService.stop();
      });
  }
}
