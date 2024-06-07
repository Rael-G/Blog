import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { User } from '../../../interfaces/User';
import { UserService } from '../../../services/user/user.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {  
  protected user! : User
  protected editUser : boolean = false
  protected editPassword : boolean = false
  protected userForm : FormGroup
  protected passwordForm : FormGroup
  protected editUserSubmitted : boolean = false
  protected editPasswordSubmitted : boolean = false

  constructor(private authService : AuthService, private userService : UserService) {
    this.userForm = new FormGroup({
      username: new FormControl('', Validators.required)
    })

    this.passwordForm = new FormGroup({
      password: new FormControl('', [Validators.required, Validators.pattern(environment.passwordPattern)]),
      repeatPassword: new FormControl('', [Validators.required])
    })

   }

  public ngOnInit(): void {
    this.authService.getUser().subscribe((user) =>
      this.user = user!
    )

    this.userForm.patchValue({'username' : this.user.userName})
  }

  protected toogleEditUser(){
    this.editUser = !this.editUser;
  }

  protected toogleEditPassword(){
    this.editPassword = !this.editPassword;
  }

  protected onSubmitUser(){
    this.editUserSubmitted = true

    if(this.userForm.invalid)
      return

    this.userService.editUser(this.user.id, this.userForm.value).subscribe({
      next: () => {
        this.authService.setUser( this.userForm.get('username')?.value)
      }
    })
    this.passwordForm.reset()
    this.editUserSubmitted = false
  }

  protected onSubmitPassword(){
    this.editPasswordSubmitted = true

    if(this.passwordForm.invalid || !this.isPasswordEqual())
      return

    this.userService.resetPassword(this.user.id, this.passwordForm.value).subscribe()
    
    this.passwordForm.reset()
    this.editPasswordSubmitted = false
  }

  protected isPasswordEqual(): boolean {
    return !this.passwordForm.get('repeatPassword')?.invalid && 
      this.passwordForm.get('password')?.value === this.passwordForm.get('repeatPassword')?.value;
  }

  protected getUsername(){
    let username
    this.authService.getUser().subscribe((user) => username = user?.userName)
    return username
  }
}
