import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Comment } from '../../interfaces/Comment'

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private baseApiUrl = environment.baseApiUrl
  private commentsUrl = `${this.baseApiUrl}/comments`

  constructor(private http: HttpClient) { }

  getComments(postId: string): Observable<Comment[]> {
    return this.http.get<Comment[]>(this.commentsUrl, { params: new HttpParams().append("postId", postId) })
  }

  createComment(comment: Comment): Observable<Comment> {
    return this.http.post<Comment>(`${this.commentsUrl}`, comment)
  }

  deleteComment(comment: Comment): Observable<Comment> {
    return this.http.delete<Comment>(`${this.commentsUrl}/${comment.id}`)
  }
}
