import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Login } from '../../models/Login';
import { Token } from '../../interfaces/Token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseApiUrl = environment.baseApiUrl
  private loginUrl = `${this.baseApiUrl}/auth`

  constructor(private http: HttpClient) { }

  login(login: Login) {
    this.http.put<Token>(this.loginUrl + '/login', login)
      .subscribe(response => {
        localStorage.setItem('accessToken', response.accessToken)
        localStorage.setItem('refreshToken', response.refreshToken)
        localStorage.setItem('creation', response.creation.toString())
        localStorage.setItem('expiration', response.expiration.toString())
      })
  }

  refreshToken(token: Token) {
    this.http.put<Token>(this.loginUrl + '/regen-token', token)
      .subscribe(response => {
        localStorage.setItem('accessToken', response.accessToken)
        localStorage.setItem('refreshToken', response.refreshToken)
        localStorage.setItem('creation', response.creation.toString())
        localStorage.setItem('expiration', response.expiration.toString())
      })
  }
}
