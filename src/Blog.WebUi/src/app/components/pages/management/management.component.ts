import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-management',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './management.component.html',
  styleUrl: './management.component.scss'
})
export class ManagementComponent {
  lorem: string = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam convallis, enim vel sodales volutpat, lorem ipsum aliquet velit, ut hendrerit nulla odio ut dolor. Quisque vulputate risus id lacus commodo, et rutrum arcu lacinia. Phasellus hendrerit diam at leo congue sollicitudin.';
  route: string = '/posts/example';
  id: string[] = ['jfer8iqw9fjqefj8q4j8q4jfqj', 'fdas7f8dasf9sa87sda9f789sa', '7fdsaf9d7g8f9h7g9hf0h', 'yt8re9jbyb09rtby0rt8by0r']
  posts: any[] = [
    { id: this.id[Math.floor(Math.random() * 4)], title: 'Post Title 1' },
    { id: this.id[Math.floor(Math.random() * 4)], title: 'Post Title 2' },
    { id: this.id[Math.floor(Math.random() * 4)], title: 'Post Title 4' },
    { id: this.id[Math.floor(Math.random() * 4)], title: 'Post Title 5' },
    { id: this.id[Math.floor(Math.random() * 4)], title: 'Post Title 6' },
    { id: this.id[Math.floor(Math.random() * 4)], title: 'Post Title 7' },
    { id: this.id[Math.floor(Math.random() * 4)], title: 'Post Title 8' },
    { id: this.id[Math.floor(Math.random() * 4)], title: 'Post Title 9' },
    { id: this.id[Math.floor(Math.random() * 4)], title: 'Post Title 10' }
  ];
}
