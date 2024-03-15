import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Post } from '../../../models/Post';

@Component({
  selector: 'app-create-post',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent {

  postForm: FormGroup;
  submitted = false;

  constructor() {
    this.postForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.maxLength(255)]),
      content: new FormControl('', [Validators.required]),
      category: new FormControl('', [Validators.required])
    });
  }

  CreateHandler(post: Post) {

  }

  submit() {
    this.submitted = true;

    if (this.postForm.invalid)
      return;

    this.CreateHandler(this.postForm.value)
  }



  //example
  categories: string[] = [
    'Sports',
    'Tecnology',
    'Home',
    'Life'
  ]
}
