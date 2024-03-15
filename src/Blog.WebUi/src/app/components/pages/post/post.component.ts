import { Component } from '@angular/core';

@Component({
  selector: 'app-post',
  standalone: true,
  imports: [],
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent {
  lorem: string = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam convallis, enim vel sodales volutpat, lorem ipsum aliquet velit, ut hendrerit nulla odio ut dolor. Quisque vulputate risus id lacus commodo, et rutrum arcu lacinia. Phasellus hendrerit diam at leo congue sollicitudin.';

  index: string[] = [this.lorem, this.lorem, this.lorem, this.lorem, this.lorem, this.lorem, this.lorem, this.lorem, this.lorem, this.lorem];
}
