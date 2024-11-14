import { Component, OnInit } from '@angular/core';
import { PaginationComponent } from "../../pagination/pagination.component";
import { RouterLink } from '@angular/router';
import { PostService } from '../../../services/post/post.service';
import { Post } from '../../../interfaces/Post';
import { ListPostComponent } from '../../list-post/list-post.component';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  imports: [PaginationComponent, RouterLink, ListPostComponent]
})
export class HomeComponent implements OnInit {
  protected currentPage: number = 1
  protected totalPages: number = 0
  protected pageNumbers : number[] = []
  protected posts: Post[] = []
  protected title: string = 'Latest Posts'

  constructor(private postService: PostService) { }

  ngOnInit(): void {
    this.loadPost()
  }

  onPageChange(pageNumber: number) {
    this.posts = []
    this.postService.getPosts(pageNumber).subscribe((posts) => 
      this.posts = posts)
    this.currentPage = pageNumber
    this.loadPageCount()
  }

  loadPost(){
    this.postService.getPosts(this.currentPage)
      .subscribe((posts) => {
        this.posts = posts
        this.loadPageCount()
    });
  }

  loadPageCount(){
    this.postService.getPageCount().subscribe((count) => {
      this.totalPages = count
      this.pageNumbers = PaginationComponent.SetPageNumbers(this.currentPage, this.totalPages)
    })
  }
}
