import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportsRoutingModule } from './reports-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ExpenseReportComponent } from './expense-report/expense-report.component';
import { ReportPopupComponent } from './report-popup/report-popup.component';
import { ReportService } from '../core/servcies/report.service';
import { PdfJsViewerModule } from 'ng2-pdfjs-viewer';
import { AppointmentReportComponent } from './appointment-report/appointment-report.component';
import { TotalExpenseReportComponent } from './total-expense-report/total-expense-report.component';

@NgModule({
  declarations: [
    ReportPopupComponent,
    ExpenseReportComponent,
    AppointmentReportComponent,
    TotalExpenseReportComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ReportsRoutingModule,
    PdfJsViewerModule
  ],
providers: [
  ReportService
]
})
export class ReportsModule { }
