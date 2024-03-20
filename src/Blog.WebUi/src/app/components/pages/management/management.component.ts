import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PostService } from '../../../services/post/post.service';
import { Post } from '../../../models/Post';
import { CommentService } from '../../../services/comment/comment.service';
import { Comment } from '../../../models/Comment';

@Component({
  selector: 'app-management',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './management.component.html',
  styleUrl: './management.component.scss'
})
export class ManagementComponent implements OnInit{
  protected posts: Post[] = []
  protected coolapseComments: Map<string, boolean> = new Map<string, boolean>()
  protected coolapseConfirmPostDelete: Map<string, boolean> = new Map<string, boolean>()
  protected coolapseConfirmCommentDelete: Map<string, boolean> = new Map<string, boolean>()

  constructor(private postService: PostService, private commentService: CommentService) { }

  ngOnInit() : void{
    this.getPosts()
  }

  getPosts(){
    this.postService.getPosts().subscribe((posts) => {
      this.posts = posts
    });
  }

  toggle(id: string, map: Map<string, boolean>)
  {
    let obj = map.get(id)

    if (!obj){
      map.set(id, true)
      return
    }

    map.set(id, !obj)
  }

  isActive(id: string, map: Map<string, boolean>): boolean{
    let obj = map.get(id)
    if (!obj)
      return false;

    return obj;
  }

  deletePost(post: Post)
  {
    this.postService.deletePost(post.id ?? '').subscribe()
    
    this.refresh()
  }

  deleteComment(comment: Comment)
  {
    this.commentService.deleteComment(comment).subscribe();

    this.refresh()
  }

  refresh(){
    this.toggleAllOff(this.coolapseConfirmCommentDelete)
    this.toggleAllOff(this.coolapseConfirmPostDelete)

    this.getPosts()
  }

  toggleAllOff(map: Map<string, boolean>){
    map.forEach((value, key) => {
      map.set(key, false);
    });
  }
}
