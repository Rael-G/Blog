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
  protected pageSize: number = 10
  protected totalPosts: number = 100
  protected posts: Post[] = []
  protected title: string = 'Latest Posts'

  constructor(private postService: PostService) { }

  ngOnInit(): void {
    this.loadPost();
  }

  onPageChange(pageNumber: number) {
    this.currentPage = pageNumber;
    //Logic to call posts
  }

  loadPost(){
    this.postService.getPosts()
      .subscribe((posts) => {
        this.posts = this.loadPostsTag(posts)
    });
  }

  loadPostsTag(posts : Post[]) : Post[] {
    for(let post of posts){
        if(post.id && post.tags.length < 1){
          this.postService.getTags(post.id).subscribe((tags) =>
            post.tags = tags
          ) 
        }
    }
    return posts;
  }

}
