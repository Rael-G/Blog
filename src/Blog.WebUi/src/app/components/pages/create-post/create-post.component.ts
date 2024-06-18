import { Component } from '@angular/core';
import { Post } from '../../../interfaces/Post';
import { PostService } from '../../../services/post/post.service';
import { Router } from '@angular/router';
import { PostFormComponent } from '../../post-form/post-form.component';
import { Color } from '../../../enums/Color';
import { MessageService } from '../../../services/message/message.service';

@Component({
  selector: 'app-create-post',
  standalone: true,
  imports: [PostFormComponent],
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent {
  constructor(private postService: PostService, private router: Router, private messageService: MessageService) { }

  createHandler(post: Post) {
    this.postService.createPost(post).subscribe()
    this.redirect()
    this.messageService.add('Post was created successfully. Refresh to reflect the changes...', Color.green)
  }

  redirect() {
    this.router.navigate(['/management'])
  }
}