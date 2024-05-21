import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth/auth.service';
import { Router, RouterLink } from '@angular/router';
import { MessageService } from '../../../services/message.service';
import { Color } from '../../../enums/Color';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
  protected loginForm: FormGroup
  protected submitted: boolean = false

  constructor(private authService : AuthService, private router : Router, private messageService: MessageService){
    this.loginForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  public ngOnInit(): void {
    if(this.authService.getToken())
      this.redirect();
  }

  protected submit(){
    this.submitted = true
    if (this.loginForm.invalid)
      return

    this.authService.login(this.loginForm.value).subscribe({
    next: () => { 
      this.redirect() 
    },
      error: (error : HttpErrorResponse) => {
        this.messageService.add(error.error, Color.red)
      }
    })
  }

  private redirect() {
    this.router.navigateByUrl('management')
  }
}
