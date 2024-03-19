import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PostService } from '../../../services/post/post.service';
import { Post } from '../../../models/Post';

@Component({
  selector: 'app-management',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './management.component.html',
  styleUrl: './management.component.scss'
})
export class ManagementComponent {
  protected posts: Post[] = []
  protected collapse: Map<string, boolean> = new Map<string, boolean>()
  
  private postService: PostService
  protected showComments = true;

  constructor(postService: PostService) {
    this.postService = postService 
   }

  ngOnInit() : void{
    this.postService.getPosts().subscribe((posts) => {
      this.posts = posts
    });
  }

  toggleComments(postId: string)
  {
    let post = this.collapse.get(postId)

    if (post == undefined)
      this.collapse.set(postId, true)
    else
      this.collapse.set(postId, !post)
  }

  isActive(postId: string): boolean{
    let post = this.collapse.get(postId)
    if (!post || post == undefined)
      return false;

    return post;
  }
}
