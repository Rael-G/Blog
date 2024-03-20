import { Injectable } from '@angular/core';
import { enviroment } from '../../../enviroments/enviroment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from '../../models/Post';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  private baseApiUrl = enviroment.baseApiUrl
  private postUrl = `${this.baseApiUrl}/posts`

  constructor(private http: HttpClient) { }

  getPosts(): Observable<Post[]>{
    return this.http.get<Post[]>(this.postUrl)
  }

  getPost(id: string): Observable<Post>{
    return this.http.get<Post>(this.postUrl + '/' + id)
  }

  createPost(post: Post): Observable<Post>{
    return this.http.post<Post>(this.postUrl, post)
  }

  editPost(post: Post): Observable<Post>{
    return this.http.put<Post>(this.postUrl + '/' + post.id, post)
  }

  deletePost(id: string): Observable<Post>{
    return this.http.delete<Post>(this.postUrl + '/' + id)
  }
}
