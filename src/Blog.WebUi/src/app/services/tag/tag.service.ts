import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tag } from '../../interfaces/Tag';

@Injectable({
  providedIn: 'root'
})
export class TagService {

  private baseApiUrl = environment.baseApiUrl
  private tagsUrl = `${this.baseApiUrl}/tags`

  constructor(private http: HttpClient) { }

  getTags(): Observable<Tag[]> {
    return this.http.get<Tag[]>(this.tagsUrl)
  }

  getTag(id: string): Observable<Tag> {
    return this.http.get<Tag>(this.tagsUrl + '/' + id)
  }

  createTag(post: Tag): Observable<Tag> {
    return this.http.post<Tag>(this.tagsUrl, post)
  }

  editTag(post: Tag): Observable<Tag> {
    return this.http.put<Tag>(this.tagsUrl + '/' + post.id, post)
  }

  deleteTag(id: string): Observable<Tag> {
    return this.http.delete<Tag>(this.tagsUrl + '/' + id)
  }
}
