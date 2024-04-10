import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { debug } from 'console';
import { TagService } from '../../services/tag/tag.service';
import { MessageService } from '../../services/message.service';
import { Color } from '../../enums/Color';

@Component({
  selector: 'app-create-tag',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './create-tag.component.html',
  styleUrl: './create-tag.component.scss'
})
export class CreateTagComponent {
  protected expandForm : boolean = false
  protected tagForm: FormGroup
  protected submitted: boolean = false

  constructor(private tagService : TagService, private messageService : MessageService)
  {
    this.tagForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.maxLength(15), Validators.minLength(3)]),
    });
  }

  protected submit(){
    this.submitted = true

    if (this.tagForm.invalid)
      return

    this.tagService.createTag(this.tagForm.value).subscribe()
    this.messageService.add('Tag was successfully created.  Refresh to reflect the changes...', Color.green)
  }

  protected toggleExpandForm()
  {
    this.expandForm = !this.expandForm
  }
}
