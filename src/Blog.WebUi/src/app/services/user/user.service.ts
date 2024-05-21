import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
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

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.usersUrl)
  }

  getUserByUsername(username : string): Observable<User>{
    return this.http.get<User>(this.usersUrl + `/username/${username}`)
  }

  createUser(user: Signin): Observable<any> {
    return this.http.post(this.usersUrl, user)
  }

  editUserRoles(id: string, roles: string[]) : Observable<any>{
    return this.http.put(this.usersUrl + `/roles/${id}`, roles)
  }

  deleteUser(id: string): Observable<User> {
    return this.http.delete<User>(this.usersUrl + '/' + id)
  }

  getPage(id : string, page : number): Observable<User> {
    return this.http.get<User>(this.usersUrl + '/' + id + '/page', { params: new HttpParams().append("page", page) })
  }

  getPageCount(id : string) : Observable<number> {
    return this.http.get<number>(this.usersUrl + '/' + id + "/page-count")
  }
}
