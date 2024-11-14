import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from '../../interfaces/Post';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  private baseApiUrl = environment.baseApiUrl
  private postUrl = `${this.baseApiUrl}/posts`

  constructor(private http: HttpClient) { }

  getPosts(page : number): Observable<Post[]> {
    return this.http.get<Post[]>(this.postUrl, { params: new HttpParams().append("page", page) })
  }

  getPost(id: string): Observable<Post> {
    return this.http.get<Post>(this.postUrl + '/' + id)
  }

  getPageCount() : Observable<number> {
    return this.http.get<number>(this.postUrl + "/page-count")
  }

  createPost(post: Post): Observable<Post> {
    return this.http.post<Post>(this.postUrl, post)
  }

  editPost(post: Post): Observable<Post> {
    return this.http.put<Post>(this.postUrl + '/' + post.id, post)
  }

  deletePost(id: string): Observable<Post> {
    return this.http.delete<Post>(this.postUrl + '/' + id)
  }
}
