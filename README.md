# Display-Pdf-Blob-With-Name
Asp.net core project with API project that uses swagger, and web project that uses angular 7. Setup to auto build the angular code. The project shows how u can pass the file name down in the header with the blob to angular httplclient and access it.


## To Access Header with angular use 'observe' as  a parameter as follows:


downloadPDF(file: any): any {
    const url = file;

    return this.httpClient.get(url, { responseType: ResponseContentType.Blob, observe: 'response' })
      .pipe(
        tap(data => data,
          error => { }
        )
      );
  }
      
Then access the header like this:

    this.downloadPDF(fileLocation)
      .subscribe(res => {
        res.headers.get('filedownloadname'))
      });

## Add the file name to header with API by doing the following in the controller function:

Response.Headers.Add("FileDownloadName", response.FileDownloadName);
Response.Headers.Add("Access-Control-Expose-Headers", "filedownloadname");

as seen in the source code.
