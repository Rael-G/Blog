import { Component, Input } from '@angular/core';
import { Post } from '../../interfaces/Post';
import { RouterLink } from '@angular/router';
import { MarkdownComponent } from 'ngx-markdown';

@Component({
  selector: 'app-list-post',
  standalone: true,
  imports: [RouterLink, MarkdownComponent],
  templateUrl: './list-post.component.html',
  styleUrl: './list-post.component.scss'
})
export class ListPostComponent {
  @Input() public title : string = ''
  @Input() public posts : Post[] = []
}
