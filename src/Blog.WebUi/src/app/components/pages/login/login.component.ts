import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  protected loginForm: FormGroup
  protected submitted: boolean = false

  constructor(private authService : AuthService, private router : Router){
    this.loginForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  protected submit(){
    this.submitted = true
    if (this.loginForm.invalid)
      return

    this.authService.login(this.loginForm.value)
    //this.redirect()
  }

  redirect() {
    this.router.navigate(['/management'])
  }
}