import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Signin } from '../../interfaces/Signin';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseApiUrl = environment.baseApiUrl
  private usersUrl = `${this.baseApiUrl}/user`

  constructor(private http: HttpClient) { }

  createUser(user: Signin): Observable<any> {
    return this.http.post(this.usersUrl, user)
  }
}
