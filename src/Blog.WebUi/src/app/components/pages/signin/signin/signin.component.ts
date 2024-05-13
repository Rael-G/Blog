import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.scss'
})
export class SigninComponent {
  protected signinForm : FormGroup
  protected submitted : boolean = false

  constructor(){
    this.signinForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      repeatPassword: new FormControl('', Validators.required)
    });
  }

  protected submit(){
    this.submitted = true
      if (this.signinForm.invalid)
        return


  }

  protected isPasswordEqual(): boolean {
    return !this.signinForm.get('repeatPassword')?.errors && 
      this.signinForm.get('password')?.value === this.signinForm.get('repeatPassword')?.value;
  }

}
