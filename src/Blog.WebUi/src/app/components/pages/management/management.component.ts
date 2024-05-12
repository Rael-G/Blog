import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { PostService } from '../../../services/post/post.service';
import { Post } from '../../../interfaces/Post';
import { CommentService } from '../../../services/comment/comment.service';
import { CreateTagComponent } from '../../create-tag/create-tag.component';
import { TablePostComponent } from '../../table-post/table-post.component';
import { PaginationComponent } from '../../pagination/pagination.component';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-management',
  standalone: true,
  imports: [RouterLink, CreateTagComponent, TablePostComponent, PaginationComponent],
  templateUrl: './management.component.html',
  styleUrl: './management.component.scss'
})
export class ManagementComponent implements OnInit {
  protected posts: Post[] = []
  protected currentPage: number = 1
  protected totalPages: number = 0
  protected pageNumbers : number[] = []
  
  constructor(private postService: PostService, private commentService : CommentService, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.getPosts()
    this.redirectToLogin()
  }

  protected onPageChange(pageNumber: number) {
    this.posts = []
    this.postService.getPosts(pageNumber).subscribe((posts) => 
      this.posts = posts)
    this.currentPage = pageNumber
    this.loadPageCount()
  }

  protected loadPageCount(){
    this.postService.getPageCount().subscribe((count) => {
      this.totalPages = count
      this.pageNumbers = PaginationComponent.SetPageNumbers(this.currentPage, this.totalPages)
    })
  }

  protected getPosts() {
    this.postService.getPosts(this.currentPage)
      .subscribe((posts) => {
        this.posts = posts
        for(let post of posts){
          if (post.id){
            this.commentService.getComments(post.id).subscribe((comments) => {
              post.comments = comments
            })
          }
        }
        this.loadPageCount()
      })
  }

  redirectToLogin()
  {
    if(!this.authService.getToken())
      this.router.navigateByUrl('login')
  }
}
