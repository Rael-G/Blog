import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageService } from '../../services/message.service';
import { Color } from '../../enums/Color';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.scss'
})
export class MessagesComponent {
  constructor(protected messageService: MessageService) { }

  closeMessage() {
    this.messageService.clear()
  }

  getColor(): string {
    return this.messageService.color === Color.green ? 'green' :
      this.messageService.color === Color.red ? 'red' : 'blue'
  }
}
