import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Post } from '../../interfaces/Post';
import { TagService } from '../../services/tag/tag.service';
import { Tag } from '../../interfaces/Tag';

@Component({
  selector: 'app-post-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './post-form.component.html',
  styleUrl: './post-form.component.scss'
})
export class PostFormComponent implements OnInit {
  protected postForm: FormGroup
  protected submitted = false
  protected tags: Tag[] = []
  @Input() post!: Post

  @Output() postSubmitted = new EventEmitter<Post>()

  constructor(private tagService : TagService) {
    this.postForm = this.GenerateFormGroup()
  }

  ngOnInit() {
    this.getPostDetails()
    this.loadTag();
  }

  private getPostDetails() {
    let tagIds : string[] = []

    if (!this.post)
      return

    for(let tag of this.post?.tags){
      if (tag.id)
        tagIds.push(tag.id)
    }

    this.postForm.patchValue({
      title: this.post?.title,
      content: this.post?.content,
      tags: tagIds
    })
  }

  private loadTag(){
    this.tagService.getTags().subscribe(tags => 
      this.tags = tags
    )
  }

  private GenerateFormGroup() : FormGroup {
    return new FormGroup({
      title: new FormControl('', [Validators.maxLength(255)]),
      content: new FormControl(''),
      tags: new FormControl('')
    });
  }

  private mapTags()
  {
    const ids = this.postForm.get('tags')?.value as string[]
    let mappedTags : Tag[] = []
    for(let index = 0; index < ids.length; index++){
      let tag = this.tags.find(t => t.id == ids[index])
      if (tag)
        mappedTags.push(tag);
    }
    this.postForm.patchValue({ tags: mappedTags });
  }

  protected submit() {
    this.submitted = true

    if (this.postForm.invalid)
      return

    this.mapTags();
    
    this.postSubmitted.emit(this.postForm.value)
  }
}
