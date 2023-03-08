import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsletterComponent } from './newsletter/newsletter.component';
import { CloseButtonComponent } from './newsletter/close-button/close-button.component';



@NgModule({
  declarations: [
    NewsletterComponent,
    CloseButtonComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    NewsletterComponent
  ]
})
export class ComponentsModule { }
