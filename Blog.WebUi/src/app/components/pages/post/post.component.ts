import { Component, OnInit, SecurityContext } from '@angular/core';
import { Post } from '../../../interfaces/Post';
import { PostService } from '../../../services/post/post.service';
import { ActivatedRoute } from '@angular/router';
import { AddCommentComponent } from './add-comment/add-comment.component';
import { Comment } from '../../../interfaces/Comment'
import { MarkdownComponent, MarkdownModule } from 'ngx-markdown';

@Component({
  selector: 'app-post',
  standalone: true,
  imports: [
    AddCommentComponent,
    MarkdownComponent
  ],
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent implements OnInit {
  protected post!: Post

  constructor(private postService: PostService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadPost()
  }

  commentAdded(comment: Comment) {
    this.post.comments.push(comment)
  }

  loadPost() {
    const id = this.route.snapshot.paramMap.get('id')
    this.postService.getPost(id ?? '').subscribe((post) => {
      this.post = post
    })
  }
}
