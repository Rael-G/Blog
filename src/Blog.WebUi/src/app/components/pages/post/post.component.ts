import { Component } from '@angular/core';
import { Post } from '../../../models/Post';
import { PostService } from '../../../services/post/post.service';
import { ActivatedRoute } from '@angular/router';
import { AddCommentComponent } from './add-comment/add-comment.component';
import { Comment } from '../../../models/Comment'

@Component({
  selector: 'app-post',
  standalone: true,
  imports: [AddCommentComponent],
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
    this.LoadPost();
  }

  protected CommentAdded(comment: Comment){
    this.post.comments.push(comment)
  }

  LoadPost(){
    const id = this.route.snapshot.paramMap.get('id');
    this.postService.getPost(id ?? '').subscribe((post) => {
      this.post = post
    });
  }
}
