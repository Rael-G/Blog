import { Component } from '@angular/core';
import { PaginationComponent } from "../../pagination/pagination.component";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  imports: [PaginationComponent, RouterLink]
})
export class HomeComponent {

  currentPage: number = 1;
  pageSize: number = 10;
  totalPosts: number = 100;

  constructor() { }

  onPageChange(pageNumber: number) {
    this.currentPage = pageNumber;
    //Logic to call posts
  }

  //examples
  lorem: string = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam convallis, enim vel sodales volutpat, lorem ipsum aliquet velit, ut hendrerit nulla odio ut dolor. Quisque vulputate risus id lacus commodo, et rutrum arcu lacinia. Phasellus hendrerit diam at leo congue sollicitudin.';
  route: string = '/posts/example';
  categories: string[] = ['physic', 'alchemy', 'biology', 'matematical'];
  posts: any[] = [
    { title: 'Post Title 1', summary: this.lorem, route: this.route, categorie: this.categories[Math.floor(Math.random() * 4)] },
    { title: 'Post Title 2', summary: this.lorem, route: this.route, categorie: this.categories[Math.floor(Math.random() * 4)] },
    { title: 'Post Title 4', summary: this.lorem, route: this.route, categorie: this.categories[Math.floor(Math.random() * 4)] },
    { title: 'Post Title 5', summary: this.lorem, route: this.route, categorie: this.categories[Math.floor(Math.random() * 4)] },
    { title: 'Post Title 6', summary: this.lorem, route: this.route, categorie: this.categories[Math.floor(Math.random() * 4)] },
    { title: 'Post Title 7', summary: this.lorem, route: this.route, categorie: this.categories[Math.floor(Math.random() * 4)] },
    { title: 'Post Title 8', summary: this.lorem, route: this.route, categorie: this.categories[Math.floor(Math.random() * 4)] },
    { title: 'Post Title 9', summary: this.lorem, route: this.route, categorie: this.categories[Math.floor(Math.random() * 4)] },
    { title: 'Post Title 10', summary: this.lorem, route: this.route, categorie: this.categories[Math.floor(Math.random() * 4)] },
  ];
}
