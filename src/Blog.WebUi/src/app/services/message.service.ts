import { Injectable } from '@angular/core';
import { Color } from '../enums/Color';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  public message: string = ''
  public color!: Color

  constructor() { }

  add(message: string, color: Color, seconds: number = 4){
    this.message = message
    this.color = color
    setTimeout(() => {
      this.clear()
    }, seconds * 1000);
  }

  clear(){
    this.message = ''
  }

}
