import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LuzIngaHeaderComponent } from './luz-inga-header/luz-inga-header.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AppRoutingModule } from 'src/app/app-routing.module';

@NgModule({
  imports: [
    CommonModule,
    AppRoutingModule
  ],
  declarations: [LuzIngaHeaderComponent, PageNotFoundComponent],
  exports:[LuzIngaHeaderComponent, PageNotFoundComponent]
})
export class GlobalComponentsModule { }
