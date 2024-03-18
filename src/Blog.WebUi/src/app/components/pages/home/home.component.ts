import { Component } from '@angular/core';
import { PaginationComponent } from "../../pagination/pagination.component";
import { RouterLink } from '@angular/router';
import { PostService } from '../../../services/post/post.service';
import { Post } from '../../../models/Post';
import { enviroment } from '../../../../enviroments/enviroment';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  imports: [PaginationComponent, RouterLink]
})
export class HomeComponent {

  protected currentPage: number = 1;
  protected pageSize: number = 10;
  protected totalPosts: number = 100;
  protected posts: Post[] = []
  protected categoriesUrl: string = enviroment.baseAppUrl + '/categories'
  private postService: PostService

  constructor(postService: PostService) {
    this.postService = postService 
   }

  ngOnInit() : void{
    this.postService.getPosts().subscribe((posts) => {
      this.posts = posts
    });
  }

  onPageChange(pageNumber: number) {
    this.currentPage = pageNumber;
    //Logic to call posts
  }

}
