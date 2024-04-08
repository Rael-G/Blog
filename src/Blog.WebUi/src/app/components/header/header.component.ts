import { Component } from '@angular/core';
import { RouterLink, RouterModule } from '@angular/router';
import { Tag } from '../../interfaces/Tag';
import { TagService } from '../../services/tag/tag.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
protected tags: Tag[] = []

  constructor(private tagService: TagService) { }

ngOnInit(): void {
  this.tagService.getTags().subscribe((tags) => {
    this.tags = tags
  });
}

}