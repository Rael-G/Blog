import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Post } from '../../models/Post';

@Component({
  selector: 'app-post-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './post-form.component.html',
  styleUrl: './post-form.component.scss'
})
export class PostFormComponent implements OnInit {
  protected postForm: FormGroup
  protected submitted = false
  @Input() post!: Post

  @Output() postSubmitted = new EventEmitter<Post>()

  constructor() {
    this.postForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.maxLength(255)]),
      content: new FormControl('', [Validators.required]),
      category: new FormControl('', [Validators.required])
    });
  }

  ngOnInit() {
    this.getPostDetails()
  }

  getPostDetails() {
    this.postForm.patchValue({
      title: this.post?.title,
      content: this.post?.content
    })
  }

  submit() {
    this.submitted = true

    if (this.postForm.invalid)
      return

    this.postSubmitted.emit(this.postForm.value)
  }

  //example
  categories: string[] = [
    'Sports',
    'Tecnology',
    'Home',
    'Life'
  ]
}
