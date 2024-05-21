import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../services/user/user.service';
import { MessageService } from '../../../services/message.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Color } from '../../../enums/Color';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.scss'
})
export class SigninComponent implements OnInit {
  protected signinForm : FormGroup
  protected submitted : boolean = false

  constructor(private userService : UserService, private authService : AuthService, private messageService : MessageService, private router : Router){
    this.signinForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      repeatPassword: new FormControl('', Validators.required)
    });
  }
  public ngOnInit(): void {
    if(this.authService.getToken())
      this.redirect()
  }

  protected submit(){
    this.submitted = true
      if (this.signinForm.invalid)
        return

    this.userService.createUser(this.signinForm.value).subscribe({
      next: () => { 
        this.redirect()
      },
        error: (error : HttpErrorResponse) => {
          this.messageService.add(error.error, Color.red)
        }
      })
  }

  protected isPasswordEqual(): boolean {
    return !this.signinForm.get('repeatPassword')?.errors && 
      this.signinForm.get('password')?.value === this.signinForm.get('repeatPassword')?.value;
  }

  private redirect() {
    this.router.navigateByUrl('management')
  }

}
