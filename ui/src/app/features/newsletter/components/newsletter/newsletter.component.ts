import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { BaseNewsLetterService } from '../../services/newsletter.service';

@Component({
  selector: 'app-newsletter',
  templateUrl: './newsletter.component.html',
  styleUrls: ['./newsletter.component.scss'],
})
export class NewsletterComponent {

  public userEmail: string = '';
  public alreadyExists = false;
  constructor(private dialogRef: MatDialogRef<NewsletterComponent>,
    private service: BaseNewsLetterService) { }
  public handleClose() {
    this.dialogRef.close();
  }

  public handleSubmit() {
    this.service.checkEmail(this.userEmail)
      .subscribe(result => {
        this.alreadyExists = result;
      });
  }

}
