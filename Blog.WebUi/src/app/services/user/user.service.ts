import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Signin } from '../../interfaces/Signin';
import { Observable, map } from 'rxjs';
import { User } from '../../interfaces/User';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseApiUrl = environment.baseApiUrl
  private usersUrl = `${this.baseApiUrl}/users`

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.usersUrl)
  }

  getUserByUsername(username : string): Observable<User>{
    return this.http.get<User>(this.usersUrl + `/username/${username}`)
  }

  createUser(user: Signin): Observable<any> {
    return this.http.post(this.usersUrl, user)
  }

  editUser(id: string, user : User){
    return this.http.put(this.usersUrl + '/' + id, user)
  }

  editUserRoles(id: string, roles: string[]) : Observable<any>{
    return this.http.put(this.usersUrl + `/roles/${id}`, roles)
  }

  resetPassword(id: string, user : Signin){
    return this.http.put(this.usersUrl + '/reset-password/' + id, user)
  }

  deleteUser(id: string) {
    return this.http.delete(this.usersUrl + '/' + id)
  }

  getPage(id : string, page : number): Observable<User> {
    return this.http.get<User>(this.usersUrl + '/' + id + '/page', { params: new HttpParams().append("page", page) })
  }

  getPageCount(id : string) : Observable<number> {
    return this.http.get<number>(this.usersUrl + '/' + id + "/page-count")
  }
}
