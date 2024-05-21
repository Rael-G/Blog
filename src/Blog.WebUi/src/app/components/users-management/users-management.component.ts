import { Component, OnInit } from '@angular/core';
import { PaginationComponent } from '../pagination/pagination.component';
import { User } from '../../interfaces/User';
import { UserService } from '../../services/user/user.service';
import { MessageService } from '../../services/message.service';
import { Color } from '../../enums/Color';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-users-management',
  standalone: true,
  imports: [PaginationComponent, ReactiveFormsModule],
  templateUrl: './users-management.component.html',
  styleUrl: './users-management.component.scss'
})
export class UsersManagementComponent implements OnInit {
  protected users : User[] = null!
  protected coolapseConfirmUserDelete: Map<string, boolean> = new Map<string, boolean>()
  protected coolapseEditRoles: Map<string, boolean> = new Map<string, boolean>()
  protected roles = [environment.roles.admin, environment.roles.moderator]
  protected rolesForm : FormGroup

  constructor(private userService : UserService, private messageService : MessageService) {
    this.rolesForm = new FormGroup({
      roles: new FormControl(''),
    })
   }

  public ngOnInit(): void {
    this.getTags()
  }

  protected submit(id : string){
    this.userService.editUserRoles(id, this.rolesForm.get('roles')?.value).subscribe()
  }

  protected isActive(id: string, map: Map<string, boolean>): boolean {
    let obj = map.get(id)
    if (!obj)
      return false

    return obj
  }

  protected toggle(id: string, map: Map<string, boolean>) {
    let obj = map.get(id)

    if (!obj) {
      map.set(id, true)
      return
    }

    map.set(id, !obj)
  }

  private getTags() {
    this.userService.getUsers()
      .subscribe((users) => {
        this.users = users
      })
  }

  protected deleteUser(user: User) {
    this.userService.deleteUser(user.id ?? '').subscribe()
    this.messageService.add("User was deleted successfully. Refresh to reflect the changes...", Color.red, 6)
    this.refresh()
  }

  private refresh() {
    this.coolapseConfirmUserDelete.forEach((value, key) => {
      this.coolapseConfirmUserDelete.set(key, false);
    })
  }
}
