import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Login } from '../../interfaces/Login';
import { Token } from '../../interfaces/Token';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, catchError, map, of } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { User } from '../../interfaces/User';
import { UserService } from '../user/user.service';
import { resolveSoa } from 'node:dns';
import { get } from 'node:http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseApiUrl = environment.baseApiUrl
  private loginUrl = `${this.baseApiUrl}/auth`
  private plataformId : object
  private token : Token | null = null
  private userSubject = new BehaviorSubject<User | null>(null);

  constructor(private http: HttpClient, private router: Router, private userService : UserService) 
  {
    this.plataformId = inject(PLATFORM_ID);
  }

  public login(login: Login) : Observable<any> {
   return this.http.put<Token>(this.loginUrl + '/login', login)
    .pipe(
      map(token => {
        localStorage.setItem('token', JSON.stringify(token))
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
    window.location.reload()
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

  public userIsInRole(role : string) : boolean{
    let isInRole = false
    this.getUser().subscribe((user) =>
      isInRole = user?.roles.some((value) => value === role)?? false
    )
    return isInRole
  }

  public getUser(): Observable<User | null> {
    let userJson = localStorage.getItem('user');

    if (userJson && !this.userSubject.value) {
      let user : User | null = null
      try {
        user = JSON.parse(userJson);
      } catch {
      }
      this.userSubject.next(user);
    }

    return this.userSubject.asObservable();
  }

  public setUser(username : string){
    this.userService.getUserByUsername(username).subscribe({
      next: (user) => {
        localStorage.setItem('user',  JSON.stringify(user))
        this.userSubject = new BehaviorSubject<User | null>(null)
      }
    })
  }
}
