import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-close-button',
  templateUrl: './close-button.component.html',
  styleUrls: ['./close-button.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CloseButtonComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
