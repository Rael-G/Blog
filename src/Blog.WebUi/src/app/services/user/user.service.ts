import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Signin } from '../../interfaces/Signin';
import { Observable } from 'rxjs';
import { User } from '../../interfaces/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseApiUrl = environment.baseApiUrl
  private usersUrl = `${this.baseApiUrl}/users`

  constructor(private http: HttpClient) { }

  getUserByUsername(username : string): Observable<User>{
    return this.http.get<User>(this.usersUrl + `/username/${username}`)
  }
  createUser(user: Signin): Observable<any> {
    return this.http.post(this.usersUrl, user)
  }
}
