import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


import { NewsletterConfirmEmailComponent } from './newsletter-confirm-email/newsletter-confirm-email.component';
import { NewsletterConfigComponent } from './newsletter-config/newsletter-config.component';

const heroesRoutes: Routes = [
  { path: 'newsletter/:subscriptionId',  component: NewsletterConfigComponent },
  { path: 'newsletter/:subscriptionId/refreshConfirmationToken',  component: NewsletterConfigComponent },
  { path: 'newsletter/confirmEmail/:confirmationCode', component: NewsletterConfirmEmailComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(heroesRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class NewsletterRoutingModule { }