import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-newsletter',
  templateUrl: './newsletter.component.html',
  styleUrls: ['./newsletter.component.scss'],
})
export class NewsletterComponent  {

  constructor(private dialogRef: MatDialogRef<NewsletterComponent>) { }
  public handleClose(){
    this.dialogRef.close();
  }

}
