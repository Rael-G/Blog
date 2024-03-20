import { Component, OnInit } from '@angular/core';
import { PostFormComponent } from '../../post-form/post-form.component'; // Import the form component
import { PostService } from '../../../services/post/post.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from '../../../models/Post';
import { MessageService } from '../../../services/message.service';
import { Color } from '../../../enums/Color';

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

  constructor(private postService: PostService, private router: Router, private route: ActivatedRoute, private messageService: MessageService) {
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
    this.messageService.add('Post was edited successfully. Refresh to reflect the changes...', Color.blue)
  }

  redirect() {
    this.router.navigate(['/management'])
  }
}