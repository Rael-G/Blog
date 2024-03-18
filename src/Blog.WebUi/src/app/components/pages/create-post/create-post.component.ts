import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Post } from '../../../models/Post';
import { PostService } from '../../../services/post/post.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-create-post',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent {

  protected postForm: FormGroup
  protected submitted = false

  private postService: PostService
  private router: Router

  constructor(postService: PostService, router: Router) {
    this.postForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.maxLength(255)]),
      content: new FormControl('', [Validators.required]),
      category: new FormControl('', [Validators.required])
    });

    this.postService = postService
    this.router = router

  }

  createHandler(post: Post) {
    this.postService.createPost(post).subscribe()

    this.redirect()
  }

  submit() {
    this.submitted = true

    if (this.postForm.invalid)
      return;

    this.createHandler(this.postForm.value)
  }

  redirect(){
    this.router.navigate(['/'])
  }



  //example
  categories: string[] = [
    'Sports',
    'Tecnology',
    'Home',
    'Life'
  ]
}
