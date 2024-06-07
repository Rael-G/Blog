import { Component, OnInit } from '@angular/core';
import { Post } from '../interfaces/Post';
import { User } from '../interfaces/User';
import { PostService } from '../services/post/post.service';
import { CommentService } from '../services/comment/comment.service';
import { UserService } from '../services/user/user.service';
import { AuthService } from '../services/auth/auth.service';
import { environment } from '../../environments/environment';
import { PaginationComponent } from '../components/pagination/pagination.component';
import { PostsManagementComponent } from '../components/posts-management/posts-management.component';

@Component({
  selector: 'app-posts-management-section',
  standalone: true,
  imports: [PostsManagementComponent],
  templateUrl: './posts-management-section.component.html',
  styleUrl: './posts-management-section.component.scss'
})
export class PostsManagementSectionComponent implements OnInit {
  protected globalTitle: string = 'Global Posts Management'
  protected userTitle: string = 'Posts Management'

  protected isGlobal = false

  protected globalPosts: Post[] = []
  protected globalCurrentPage: number = 1
  protected globalTotalPages: number = 0
  protected globalPageNumbers: number[] = []

  protected userPosts: Post[] = []
  protected userCurrentPage: number = 1
  protected userTotalPages: number = 0
  protected userPageNumbers: number[] = []

  private user: User | null = null

  constructor(private postService: PostService, private commentService: CommentService, private userService: UserService, private authService: AuthService) {
  }

  public ngOnInit(): void {
    this.authService.getUser().subscribe(
      (user) => {
        this.user = user!
        this.userGetPosts()
        if (this.isAdmin())
          this.globalGetPosts()
      }
    )
  }

  protected isAdmin(): boolean {
    return this.authService.userIsInRole(environment.roles.admin)
  }

  protected toogleGlobal() {
    this.isGlobal = !this.isGlobal
  }

  protected toogleMessage(): string {
    if (this.isGlobal)
      return 'Global'
    return 'User'
  }

  protected userOnPageChange(pageNumber: number) {
    this.userPosts = []
    this.userService.getPage(this.user?.id ?? '', pageNumber).subscribe((user) =>
      this.userPosts = user.posts)
    this.userCurrentPage = pageNumber
    this.userLoadPageCount()
  }

  protected globalOnPageChange(pageNumber: number) {
    this.globalPosts = []
    this.postService.getPosts(pageNumber).subscribe((posts) =>
      this.globalPosts = posts)
    this.globalCurrentPage = pageNumber
    this.globalLoadPageCount()
  }

  protected userLoadPageCount() {
    this.userService.getPageCount(this.user?.id ?? '').subscribe((count) => {
      this.userTotalPages = count
      this.userPageNumbers = PaginationComponent.SetPageNumbers(this.userCurrentPage, this.userTotalPages)
    })
  }

  protected globalLoadPageCount() {
    this.postService.getPageCount().subscribe((count) => {
      this.globalTotalPages = count
      this.globalPageNumbers = PaginationComponent.SetPageNumbers(this.globalCurrentPage, this.globalTotalPages)
    })
  }

  private userGetPosts() {
    this.userService.getPage(this.user?.id ?? '', this.userCurrentPage)
      .subscribe((user) => {
        this.userPosts = user.posts
        for (let post of user.posts) {
          if (post.id) {
            this.commentService.getComments(post.id).subscribe((comments) => {
              post.comments = comments
            })
          }
        }
        this.userLoadPageCount()
      })
  }

  private globalGetPosts() {
    this.postService.getPosts(this.globalCurrentPage)
      .subscribe((posts) => {
        this.globalPosts = posts
        for (let post of posts) {
          if (post.id) {
            this.commentService.getComments(post.id).subscribe((comments) => {
              post.comments = comments
            })
          }
        }
        this.globalLoadPageCount()
      })
  }
}
