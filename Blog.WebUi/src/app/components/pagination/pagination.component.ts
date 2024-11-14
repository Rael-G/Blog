import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss'
})
export class PaginationComponent {
  @Input() public currentPage: number = 0
  @Input() public totalPages: number = 0
  @Input() public pageNumbers : number[] = []

  @Output() pageChange = new EventEmitter<number>()

  constructor() { }

  protected nextPage() {
    if (this.currentPage < this.totalPages) {
      this.pageChange.emit(this.currentPage + 1)
    }
  }

  protected previousPage() {
    if (this.currentPage > 1) {
      this.pageChange.emit(this.currentPage - 1)
    }
  }

  protected goToPage(pageNumber: number) {
    if (pageNumber > 0 && pageNumber <= this.totalPages) {
      this.pageChange.emit(pageNumber)
    }
  }

  public static SetPageNumbers(currentPage : number, totalPages : number) : number[]{
    let page = currentPage - 2
    let pageNumbers : number[] = []
    for (let i = 0; i < 5; i++){
      if (page > totalPages)
        break
      if(page > 0)
        pageNumbers.push(page)
      page++
    }
    return pageNumbers
  }
}
