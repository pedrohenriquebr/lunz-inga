import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { NewsletterComponent } from 'src/app/components/newsletter/newsletter.component';
import { CookieService } from 'ngx-cookie-service';
import { CookiesNames } from 'src/app/config/cookiesNames.enum';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.scss']
})
export class LandingPageComponent implements OnInit, OnDestroy {

  constructor(
    public dialog: MatDialog,
    private cookieService: CookieService
  ) { }
  private readonly newsletterDialogConfig: MatDialogConfig<NewsletterComponent> = {
    disableClose: true,
    autoFocus: true,
    hasBackdrop: false,
    position: {
      top: '10%'
    }
  };

  private timerId: any = null;

  ngOnInit(): void {
    const hasClosedNewsLetter = this.cookieService.get(CookiesNames.HAS_CLOSED_NEWSLETTER) == '1';
    if (!hasClosedNewsLetter) {
      this.timerId = setTimeout(() => this.openNewsLetterModal(), environment.NEWSLETTER_DELAY);
    }
  }

  ngOnDestroy(): void {
    if(this.timerId != null)
        clearTimeout(this.timerId)
  }


  private openNewsLetterModal() {
    const dialogRef = this.dialog
      .open(NewsletterComponent, this.newsletterDialogConfig);

    dialogRef
      .afterClosed()
      .subscribe(result => {
        this.cookieService.set(CookiesNames.HAS_CLOSED_NEWSLETTER, '1');
      });
  }
}
