import { Component, OnInit } from '@angular/core';
import { CreateTagComponent } from '../create-tag/create-tag.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { Tag } from '../../interfaces/Tag';
import { TagService } from '../../services/tag/tag.service';
import { Color } from '../../enums/Color';
import { MessageService } from '../../services/message.service';

@Component({
  selector: 'app-tags-management',
  standalone: true,
  imports: [CreateTagComponent, PaginationComponent],
  templateUrl: './tags-management.component.html',
  styleUrl: './tags-management.component.scss'
})
export class TagsManagementComponent implements OnInit{
  protected tags : Tag[] = null!
  protected coolapseConfirmTagDelete: Map<string, boolean> = new Map<string, boolean>()
  protected coolapseEditTag: Map<string, boolean> = new Map<string, boolean>()

  constructor(private tagService : TagService, private messageService : MessageService){ }

  public ngOnInit(): void {
    this.getTags()
  }

  protected onSubmitCreate(tag : Tag){
    this.tagService.createTag(tag).subscribe()
    this.messageService.add('Tag was successfully created.  Refresh to reflect the changes...', Color.green)
  }

  protected onSubmitEdit(tag : Tag){
    this.tagService.editTag(tag).subscribe()
    this.messageService.add('Tag was successfully edited.  Refresh to reflect the changes...', Color.green)
    this.refresh()
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
    this.tagService.getTags()
      .subscribe((tags) => {
        this.tags = tags
      })
  }

  protected deleteTag(tag: Tag) {
    this.tagService.deleteTag(tag.id ?? '').subscribe()
    this.messageService.add("Tag was deleted successfully. Refresh to reflect the changes...", Color.red, 6)
    this.refresh()
  }

  private refresh() {
    this.toggleAllOff(this.coolapseConfirmTagDelete)
    this.toggleAllOff(this.coolapseEditTag)
  }

  toggleAllOff(map: Map<string, boolean>) {
    map.forEach((value, key) => {
      map.set(key, false);
    });
  }
}
