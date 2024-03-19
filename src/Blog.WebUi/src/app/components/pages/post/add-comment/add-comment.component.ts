import { Component, EventEmitter, Output } from '@angular/core';
import { CommentService } from '../../../../services/comment/comment.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Comment } from '../../../../models/Comment';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-comment',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './add-comment.component.html',
  styleUrl: './add-comment.component.scss'
})
export class AddCommentComponent {
  @Output() commentAdded = new EventEmitter<Comment>();
  
  protected commentForm: FormGroup
  protected submitted = false
  
  private route: ActivatedRoute
  private router: Router
  private commentService: CommentService

  constructor(commentService: CommentService, route: ActivatedRoute, router: Router) {
    this.commentForm = new FormGroup({
      author: new FormControl('', [Validators.required, Validators.maxLength(255)]),
      content: new FormControl('', [Validators.required, Validators.maxLength(511)])
    });

    this.commentService = commentService
    this.route = route
    this.router = router
  }

  createHandler(comment: Comment) {
    const postId = this.route.snapshot.paramMap.get('id');
    comment.postId = postId?? '';
    this.commentService.createComment(comment).subscribe()

    this.reset(comment);
  }

  submit() {
    this.submitted = true

    if (this.commentForm.invalid)
      return;

    this.createHandler(this.commentForm.value)
  }

  reset(comment: Comment){
    this.commentForm.reset()
    this.commentAdded.emit(comment);
    this.submitted = false;
  }

}
