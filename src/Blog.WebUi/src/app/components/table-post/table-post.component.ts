import { Component, Input } from '@angular/core';
import { Post } from '../../interfaces/Post';
import { Comment } from '../../interfaces/Comment'
import { RouterLink } from '@angular/router';
import { PostService } from '../../services/post/post.service';
import { CommentService } from '../../services/comment/comment.service';
import { MessageService } from '../../services/message/message.service';
import { Color } from '../../enums/Color';

@Component({
  selector: 'app-table-post',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './table-post.component.html',
  styleUrl: './table-post.component.scss'
})
export class TablePostComponent {
  @Input() public posts : Post[] = []
  @Input() public showEdit : boolean = false;
  protected coolapseComments: Map<string, boolean> = new Map<string, boolean>()
  protected coolapseConfirmPostDelete: Map<string, boolean> = new Map<string, boolean>()
  protected coolapseConfirmCommentDelete: Map<string, boolean> = new Map<string, boolean>()

  constructor(private postService : PostService, private commentService : CommentService, 
    private messageService : MessageService) { }

  toggle(id: string, map: Map<string, boolean>) {
    let obj = map.get(id)

    if (!obj) {
      map.set(id, true)
      return
    }

    map.set(id, !obj)
  }

  isActive(id: string, map: Map<string, boolean>): boolean {
    let obj = map.get(id)
    if (!obj)
      return false;

    return obj;
  }

  deletePost(post: Post) {
    this.postService.deletePost(post.id ?? '').subscribe()
    this.messageService.add("Post was deleted successfully. Refresh to reflect the changes...", Color.red, 6)
    this.refresh()
  }

  deleteComment(comment: Comment) {
    this.commentService.deleteComment(comment).subscribe();
    this.messageService.add("Comment was deleted successfully. Refresh to reflect the changes...", Color.red, 6)
    this.refresh()
  }

  refresh() {
    this.toggleAllOff(this.coolapseConfirmCommentDelete)
    this.toggleAllOff(this.coolapseConfirmPostDelete)
  }

  toggleAllOff(map: Map<string, boolean>) {
    map.forEach((value, key) => {
      map.set(key, false);
    });
  }
}
