import { Component } from '@angular/core';
import { PostsManagementComponent } from '../../posts-management/posts-management.component';
import { TagsManagementComponent } from '../../tags-management/tags-management.component';
import { UsersManagementComponent } from '../../users-management/users-management.component';
import { AuthService } from '../../../services/auth/auth.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-management',
  standalone: true,
  imports: [PostsManagementComponent, TagsManagementComponent, UsersManagementComponent],
  templateUrl: './management.component.html',
  styleUrl: './management.component.scss'
})
export class ManagementComponent {

  constructor (private authService : AuthService) { }
  protected isAdmin() : boolean{
    return this.authService.userIsInRole(environment.roles.admin)
  }
}
