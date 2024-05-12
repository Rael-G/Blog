import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Login } from '../../models/Login';
import { Token } from '../../interfaces/Token';
import { Router } from '@angular/router';
import { Observable, catchError, map } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseApiUrl = environment.baseApiUrl
  private loginUrl = `${this.baseApiUrl}/auth`
  private plataformId : object

  constructor(private http: HttpClient, private router: Router) 
  {
    this.plataformId = inject(PLATFORM_ID);
  }

  login(login: Login) : Observable<any> {
   return this.http.put<Token>(this.loginUrl + '/login', login)
    .pipe(
      map(response => {localStorage.setItem('token', JSON.stringify(response))}),
      catchError(error => { throw error })
    )
  }

  refreshToken(token: Token) : Observable<Token> {
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

  logOut(): void {
    localStorage.clear();
    this.router.navigateByUrl('login')
  }

  getToken() : Token | null{
    let jsonToken : string = ''
    if (isPlatformBrowser(this.plataformId)){
      jsonToken = localStorage.getItem('token')?? ''
    }
    
    let token : Token | null

    try{
      token = JSON.parse(jsonToken)
    }
    catch{
      token = null
    }

    return token
  }
}
