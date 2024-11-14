import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
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

  getPage(id : string, page : number): Observable<Tag> {
    return this.http.get<Tag>(this.tagsUrl + '/' + id + '/page', { params: new HttpParams().append("page", page) })
  }

  getPageCount(id : string) : Observable<number> {
    return this.http.get<number>(this.tagsUrl + '/' + id + "/page-count")
  }

  createTag(tag: Tag): Observable<Tag> {
    return this.http.post<Tag>(this.tagsUrl, tag)
  }

  editTag(tag: Tag): Observable<Tag> {
    return this.http.put<Tag>(this.tagsUrl + '/' + tag.id, tag)
  }

  deleteTag(id: string): Observable<Tag> {
    return this.http.delete<Tag>(this.tagsUrl + '/' + id)
  }
}
