import { Component, OnInit } from '@angular/core';
import { PostFormComponent } from '../../post-form/post-form.component'; // Import the form component
import { PostService } from '../../../services/post/post.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from '../../../models/Post';

@Component({
  selector: 'app-edit-post',
  standalone: true,
  imports: [PostFormComponent],
  templateUrl: './edit-post.component.html',
  styleUrls: ['./edit-post.component.scss']
})
export class EditPostComponent implements OnInit {

  public post!: Post
  private postId: string

  constructor(private postService: PostService, private router: Router, private route: ActivatedRoute) {
    this.postId = this.route.snapshot.paramMap.get('id') ?? ''
  }

  ngOnInit() {
    this.loadPost()
  }

  async loadPost() {
    this.postService.getPost(this.postId).subscribe(
      (post) => this.post = post
    )
  }

  editHandler(post: Post) {
    post.id = this.postId
    this.postService.editPost(post).subscribe()
    this.redirect()
  }

  redirect() {
    this.router.navigate(['/management'])
  }
}