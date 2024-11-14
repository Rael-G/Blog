import { Component, Input, Output } from '@angular/core';
import { TablePostComponent } from '../table-post/table-post.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { RouterLink } from '@angular/router';
import { Post } from '../../interfaces/Post';
import { EventEmitter } from '@angular/core';

@Component({
  selector: 'app-posts-management',
  standalone: true,
  imports: [TablePostComponent, PaginationComponent, RouterLink],
  templateUrl: './posts-management.component.html',
  styleUrl: './posts-management.component.scss'
})
export class PostsManagementComponent {

  @Input() title: string = 'Posts Management'
  @Input() posts: Post[] = []
  @Input() currentPage: number = 1;
  @Input() totalPages: number = 0;
  @Input() pageNumbers: number[] = [];
  @Input() public showEdit: boolean = false;
  @Output() pageChange = new EventEmitter<number>()

  protected onPageChange(pageNumber: number) {
    this.pageChange.emit(pageNumber)
  }
}
