import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';
import { PageNotFoundComponent } from './features/global-components/page-not-found/page-not-found.component';

const routes: Routes = [
  {
    path: 'landing-page', component: LandingPageComponent
  },
  { path: '',   redirectTo: '/landing-page', pathMatch: 'full' },
  {path: '**', component: PageNotFoundComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
