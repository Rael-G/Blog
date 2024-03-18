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
  
  private postService: PostService

  constructor(postService: PostService) {
    this.postService = postService 
   }

  ngOnInit() : void{
    this.postService.getPosts().subscribe((posts) => {
      this.posts = posts
    });
  }
}
