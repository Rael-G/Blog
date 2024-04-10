import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from '../../interfaces/Post';
import { Tag } from '../../interfaces/Tag';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  private baseApiUrl = environment.baseApiUrl
  private postUrl = `${this.baseApiUrl}/posts`

  constructor(private http: HttpClient) { }

  getPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(this.postUrl)
  }

  getTags(id: string): Observable<Tag[]> {
    return this.http.get<Tag[]>(this.postUrl + '/tags/' + id)
  }

  getPost(id: string): Observable<Post> {
    return this.http.get<Post>(this.postUrl + '/' + id)
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
