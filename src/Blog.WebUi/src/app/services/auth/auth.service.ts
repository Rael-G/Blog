import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Login } from '../../interfaces/Login';
import { Token } from '../../interfaces/Token';
import { Router } from '@angular/router';
import { Observable, catchError, map } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { User } from '../../interfaces/User';
import { UserService } from '../user/user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseApiUrl = environment.baseApiUrl
  private loginUrl = `${this.baseApiUrl}/auth`
  private plataformId : object
  private user : User | null = null
  private token : Token | null = null

  constructor(private http: HttpClient, private router: Router, private userService : UserService) 
  {
    this.plataformId = inject(PLATFORM_ID);
  }

  public login(login: Login) : Observable<any> {
   return this.http.put<Token>(this.loginUrl + '/login', login)
    .pipe(
      map(response => {
        localStorage.setItem('token', JSON.stringify(response))
        this.setUser(login.username)
      }),
      catchError(error => { throw error })
    )
  }

  public refreshToken(token: Token) : Observable<Token> {
    return this.http.put<Token>(this.loginUrl + '/regen-token', token)
      .pipe(
        map(response => {
          localStorage.setItem('token', JSON.stringify(response))
          return response
        }),
        catchError(error => { 
          if(error.status === 403)
            this.logOut()
          throw error
         })
      )
  }

  public logOut(): void {
    localStorage.clear();
    this.router.navigateByUrl('login')
  }

  public getToken() : Token | null {
    if(!this.token){
      let jsonToken = ''
      if (isPlatformBrowser(this.plataformId)){
        jsonToken = localStorage.getItem('token')?? ''
      }
      try{
        this.token = JSON.parse(jsonToken)
      }
      catch{ }
    }
  
    return this.token
  }

  public getUser() : User | null {
    if (!this.user){
      let userJson  = ''
      if (isPlatformBrowser(this.plataformId)){
        userJson = localStorage.getItem('user')?? ''
      }
      try{
        this.user = JSON.parse(userJson)
      }
      catch{}
    }
    
    return this.user
  }

  private setUser(username : string){
    this.userService.getUserByUsername(username).subscribe({
      next: (user) => {localStorage.setItem('user',  JSON.stringify(user)); console.log(user)}
    }
    )
  }
}
