import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { PlatformLocation } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'DisplayPdfBlob';

  constructor(private readonly httpClient: HttpClient, private readonly platformLocation: PlatformLocation){

  }

  downloadPdf(): void {
    const fileLocation =  "https://localhost:44353/Pdf/PreviewPdf";
    this.downloadFile(fileLocation);
  }

  downloadFile(fileLocation: any): void {
    const windowTimeout = 100;
    this.downloadPDF(fileLocation)
      .subscribe(res => {
        const fileURL = URL.createObjectURL(res.body);
        // Internet Explorer 6-11
        const isIE = !!(document as any).documentMode || false;
        if (isIE) {
          window.navigator.msSaveOrOpenBlob(res.body, res.headers.get('filedownloadname'));
        } else {
          const link = document.createElement('a');
          link.href = fileURL;
          link.target = '_blank';
          link.download = res.headers.get('filedownloadname');
          link.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));
          setTimeout(() => {
            // For Firefox it is necessary to delay revoking the ObjectURL
            window.URL.revokeObjectURL(fileURL);
            link.remove();
          }, windowTimeout);
        }
      });
  }

  downloadPDF(file: any): any {
    const url = file;

    return this.httpClient.get(url, { responseType: ResponseContentType.Blob, observe: 'response' })
      .pipe(
        tap(data => data,
          error => { }
        )
      );
  }
}

export enum ResponseContentType {
  Blob = 'blob'
}
