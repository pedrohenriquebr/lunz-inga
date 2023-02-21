import { Component, EventEmitter, OnInit, Output, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-close-button',
  templateUrl: './close-button.component.html',
  styleUrls: ['./close-button.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CloseButtonComponent implements OnInit {

  @Output()
  close  = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  public handleClick(event: Event){
    this.close.emit();
  }

}
