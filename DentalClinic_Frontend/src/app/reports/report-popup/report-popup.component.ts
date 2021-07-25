import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PdfJsViewerComponent } from 'ng2-pdfjs-viewer';

@Component({
    selector: 'report-popup',
    templateUrl: './report-popup.component.html'
})
export class ReportPopupComponent implements OnInit {

    @ViewChild('pdfViewer', { static: true }) pdfViewer: PdfJsViewerComponent;

    //Variables
    reportTitle: string = "Report";

    constructor(
        public dialogRef: MatDialogRef<ReportPopupComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any) {
    }

    ngOnInit() {
        this.bindPDF(this.data.reportTitle, this.data.pdf);
    }

    public bindPDF(reportTitle: string, reportPDF: any) {
        this.reportTitle = reportTitle;
        this.pdfViewer.pdfSrc = reportPDF;
        this.pdfViewer.refresh();
    }

}
