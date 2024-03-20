import { Component } from '@angular/core';
import { Post } from '../../../models/Post';
import { PostService } from '../../../services/post/post.service';
import { Router } from '@angular/router';
import { PostFormComponent } from '../../post-form/post-form.component';

@Component({
  selector: 'app-create-post',
  standalone: true,
  imports: [PostFormComponent],
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent {


  constructor(private postService: PostService, private router: Router) { }

  createHandler(post: Post) {
    this.postService.createPost(post).subscribe()
    this.redirect()
  }

  redirect(){
    this.router.navigate(['/management'])
  }

}
