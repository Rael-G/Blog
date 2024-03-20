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
  @Input() currentPage: number = 0
  @Input() totalItems: number = 0
  @Input() itemsPerPage: number = 0

  @Output() pageChange = new EventEmitter<number>()

  constructor() { }

  nextPage() {
    if (this.currentPage < this.totalPages()) {
      this.pageChange.emit(this.currentPage + 1)
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.pageChange.emit(this.currentPage--)
    }
  }

  totalPages(): number {
    return Math.ceil(this.totalItems / this.itemsPerPage)
  }

  goToPage(pageNumber: number) {
    if (pageNumber > 0 && pageNumber <= this.totalPages()) {
      this.pageChange.emit(pageNumber)
    }
  }
}
