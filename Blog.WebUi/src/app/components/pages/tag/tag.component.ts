import { Component, OnInit } from '@angular/core';
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
export class TagComponent implements OnInit {
  
  protected tag!: Tag
  protected currentPage: number = 1
  protected totalPages: number = 0
  protected pageNumbers : number[] = []
  protected posts: Post[] = []
  protected title: string = ''

  constructor(private tagService: TagService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.load();
  }

  load(){
    let id : string = ''
    this.route.paramMap.subscribe((params) => {
      id = params.get('id')!
      this.loadTag(id, 1)
    })
  }

  loadTag(id : string, pageNumber : number): void {
    this.tagService.getPage(id, pageNumber)
    .subscribe((tag) => {
      this.tag = tag
      this.title = tag.name
      this.posts = tag.posts
      this.loadPageCount(tag.id!, pageNumber)
    });
  }

  loadPageCount(id : string, page : number){
    this.tagService.getPageCount(id)
      .subscribe((count) =>{
        this.totalPages = count
        this.pageNumbers = PaginationComponent.SetPageNumbers(page, this.totalPages)
      })
    
  }

  onPageChange(pageNumber: number) {
    this.currentPage = pageNumber
    this.loadTag(this.tag.id!, pageNumber)
  }
}
