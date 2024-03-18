import { Component } from '@angular/core';
import { Post } from '../../../models/Post';
import { PostService } from '../../../services/post/post.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-post',
  standalone: true,
  imports: [],
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent {
  private postService: PostService
  private route: ActivatedRoute
  protected post!: Post;

  constructor(postService: PostService, route: ActivatedRoute) {
    this.postService = postService
    this.route = route
   }

  ngOnInit() : void{
    const id = this.route.snapshot.paramMap.get('id');
    this.postService.getPost(id ?? '').subscribe((post) => {
      this.post = post
  });
}
}
