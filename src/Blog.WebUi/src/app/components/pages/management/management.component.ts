import { Component } from '@angular/core';
import { CreateTagComponent } from '../../create-tag/create-tag.component';
import { PostsManagementComponent } from '../../posts-management/posts-management.component';

@Component({
  selector: 'app-management',
  standalone: true,
  imports: [PostsManagementComponent, CreateTagComponent],
  templateUrl: './management.component.html',
  styleUrl: './management.component.scss'
})
export class ManagementComponent {
  
}
