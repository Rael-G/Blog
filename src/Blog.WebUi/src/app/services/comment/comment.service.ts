import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { enviroment } from '../../../enviroments/enviroment';
import { HttpClient } from '@angular/common/http';
import { Comment } from '../../models/Comment'

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private baseApiUrl = enviroment.baseApiUrl
  private postsUrl = `${this.baseApiUrl}/posts`

  constructor(private http: HttpClient) { }

  createComment(comment: Comment): Observable<Comment>{
    return this.http.post<Comment>(`${this.postsUrl}/${comment.postId}/comments`, comment)
  }

  deleteComment(comment: Comment): Observable<Comment>{
    return this.http.delete<Comment>(`${this.postsUrl}/${comment.postId}/comments/${comment.id}`)
  }
}
