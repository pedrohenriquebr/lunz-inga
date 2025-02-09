import { NewsLetter } from './features/newsletter/newsletter.module';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';
import { MatDialogModule } from '@angular/material/dialog';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms'
import { GlobalComponentsModule } from './features/global-components/global-components.module';
import { NewsletterRoutingModule } from './pages/newsletter/newsletter-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    LandingPageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NewsLetter,
    BrowserAnimationsModule,
    MatDialogModule,
    HttpClientModule,
    FormsModule,
    GlobalComponentsModule,
    NewsletterRoutingModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
