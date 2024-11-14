import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user/user.service';
import { ActivatedRoute } from '@angular/router';
import { Post } from '../../../interfaces/Post';
import { User } from '../../../interfaces/User';
import { PaginationComponent } from '../../pagination/pagination.component';
import { ListPostComponent } from '../../list-post/list-post.component';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [ListPostComponent, PaginationComponent],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent implements OnInit {
  protected user!: User
  protected currentPage: number = 1
  protected totalPages: number = 0
  protected pageNumbers : number[] = []
  protected posts: Post[] = []
  protected title: string = ''

  constructor(private userService: UserService, private route: ActivatedRoute) { }

  public ngOnInit(): void {
    this.load();
  }

  load(){
    let id : string = ''
    this.route.paramMap.subscribe((params) => {
      id = params.get('id')!
      this.loadUser(id, 1)
    })
  }

  loadUser(id : string, pageNumber : number): void {
    this.userService.getPage(id, pageNumber)
    .subscribe((user) => {
      this.user = user
      this.title = user.userName
      this.posts = user.posts
      this.loadPageCount(user.id!, pageNumber)
    });
  }

  loadPageCount(id : string, page : number){
    this.userService.getPageCount(id)
      .subscribe((count) =>{
        this.totalPages = count
        this.pageNumbers = PaginationComponent.SetPageNumbers(page, this.totalPages)
      })
    
  }

  onPageChange(pageNumber: number) {
    this.currentPage = pageNumber
    this.loadUser(this.user.id!, pageNumber)
  }
}
