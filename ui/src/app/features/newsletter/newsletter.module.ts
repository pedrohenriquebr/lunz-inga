import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsletterComponent } from './components/newsletter/newsletter.component';
import { CloseButtonComponent } from './components/newsletter/close-button/close-button.component';
import { BaseNewsLetterService } from './services/newsletter.service';
import { NewsletterService } from './services/newsletter-impl.service';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    NewsletterComponent,
    CloseButtonComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    NewsletterComponent
  ],
  providers: [
    {
      provide: BaseNewsLetterService,
      useClass: NewsletterService
    }
  ],
})
export class NewsLetter { }
