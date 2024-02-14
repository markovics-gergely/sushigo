import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { RegisterService } from '../services/register.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss']
})
export class RegisterPageComponent {
  protected registerForm: FormGroup | undefined;

  constructor(private registerService: RegisterService) { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      userName: new FormControl('', Validators.required),
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required),
      confirmedPassword: new FormControl('', Validators.required),
    });
  }

  register(): void {
    this.registerService.register(this.registerForm!);
  }
}
