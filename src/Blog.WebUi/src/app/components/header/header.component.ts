import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Tag } from '../../interfaces/Tag';
import { TagService } from '../../services/tag/tag.service';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  protected tags: Tag[] = []
  constructor(private tagService: TagService, private authService: AuthService) { }

  ngOnInit(): void {
    this.tagService.getTags().subscribe((tags) => {
      this.tags = tags
    });
  }

  showLogOutAndManagement() : boolean{
    if(this.authService.getToken())
      return true
    return false
  }

  logOut() {
    this.authService.logOut()
  }

  protected getUsername() : string{
    console.log(this.authService.getUser())
    return this.authService.getUser()?.userName!
  }
  
}