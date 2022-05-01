import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsletterComponent } from './newsletter/newsletter.component';



@NgModule({
  declarations: [
    NewsletterComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    NewsletterComponent
  ]
})
export class ComponentsModule { }
