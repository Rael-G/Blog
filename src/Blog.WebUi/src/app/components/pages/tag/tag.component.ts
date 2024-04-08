import { Component } from '@angular/core';
import { Tag } from '../../../interfaces/Tag';
import { TagService } from '../../../services/tag/tag.service';
import { ActivatedRoute } from '@angular/router';
import { Post } from '../../../interfaces/Post';
import { ListPostComponent } from "../../list-post/list-post.component";
import { PaginationComponent } from "../../pagination/pagination.component";

@Component({
    selector: 'app-tag',
    standalone: true,
    templateUrl: './tag.component.html',
    styleUrl: './tag.component.scss',
    imports: [ListPostComponent, PaginationComponent]
})
export class TagComponent {
  
  protected tag!: Tag
  protected currentPage: number = 1
  protected pageSize: number = 10
  protected totalPosts: number = 100
  protected posts: Post[] = []
  protected title: string = ''

  constructor(private tagService: TagService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadTag()
  }

  init(tag: Tag){
    this.tag = tag
    this.posts = tag.posts
    this.title = tag.name
  }

  loadTag(): void {
    this.route.paramMap
      .subscribe((params) => {
        const id = params.get('id');
        this.tagService.getTag(id ?? '')
        .subscribe((tag) => {
          this.init(tag)
        });
      })
  }

  onPageChange(pageNumber: number) {
    this.currentPage = pageNumber;
    //Logic to call posts
  }
}
