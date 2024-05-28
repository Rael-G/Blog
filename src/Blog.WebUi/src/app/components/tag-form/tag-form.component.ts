import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Tag } from '../../interfaces/Tag';

@Component({
  selector: 'app-tag-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './tag-form.component.html',
  styleUrl: './tag-form.component.scss'
})
export class TagFormComponent implements OnInit{
  protected expandForm : boolean = false
  protected tagForm: FormGroup
  protected submitted: boolean = false
  @Input() tag : Tag | undefined

  @Output() onSubmit = new EventEmitter<Tag>();

  constructor()
  {
    this.tagForm = new FormGroup({
      id: new FormControl(),
      name: new FormControl('', [Validators.required, Validators.maxLength(15), Validators.minLength(3)]),
    });
  }
  ngOnInit(): void {
    this.tagForm.patchValue({
      id: this.tag?.id,
      name: this.tag?.name
    })
  }

  protected submit(){
    this.submitted = true

    if (this.tagForm.invalid)
      return

    this.tagForm.reset()
    this.submitted = false
    this.onSubmit.emit(this.tagForm.value)
  }

  protected toggleExpandForm()
  {
    this.expandForm = !this.expandForm
  }
}