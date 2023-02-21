import { Component, EventEmitter, Output, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-close-button',
  templateUrl: './close-button.component.html',
  styleUrls: ['./close-button.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CloseButtonComponent {

  @Output()
  close  = new EventEmitter();
  
  public handleClick(event: Event){
    this.close.emit();
  }

}
