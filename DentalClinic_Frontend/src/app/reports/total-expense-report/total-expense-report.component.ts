import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Clinic } from '../../core/models/clinic.model';
import { TotalExpenseFilter } from '../../core/models/total-expense-filter.model';
import { User } from '../../core/models/user.model';
import { ReportService } from '../../core/servcies/report.service';
import { BaseComponent } from '../../shared/components/base-component/base-component';
import { DetailsListEnum } from '../../shared/enum/details-list.enum';
import { RequestedData } from '../../shared/models/request-data.entity';
import { AlertService } from '../../shared/service/alert.service';
import { listHasValue } from '../../shared/service/helper.service';
import { ReportPopupComponent } from '../report-popup/report-popup.component';

@Component({
  selector: 'app-total-expense-report',
  templateUrl: './total-expense-report.component.html'
})
export class TotalExpenseReportComponent extends BaseComponent implements OnInit {

  totalExpenseFilter = new TotalExpenseFilter();
  // Lists
  clinicList = new Array<Clinic>();
  filteredClinicList = new Array<Clinic>();
  userList = new Array<User>();
  filteredUserList = new Array<User>();

  constructor(private reportService: ReportService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<ReportPopupComponent>,
    private alertService: AlertService,
    protected cdref: ChangeDetectorRef,
    protected route: ActivatedRoute,
    protected title: Title) { 
      super(cdref, route, title);
    }

  ngOnInit(): void {
    this.getDetailsLists();
  }

  public filterClinic(value: string) {
    let filter = value.toLowerCase();
    this.filteredClinicList = this.clinicList
      .filter(option => option.name.toLowerCase().startsWith(filter));
  }

  public filterUser(value: string) {
    let filter = value.toLowerCase();
    this.filteredUserList = this.userList
      .filter(option => option.fullName.toLowerCase().startsWith(filter));
  }

  private getDetailsLists() {
    let requestedDate = new RequestedData<TotalExpenseFilter>();

    this.reportService.getExpenseDetailsLists(requestedDate).subscribe(
      res => this.getTotalExpenseDetailsListsOnSuccess(res),
      err => this.getTotalExpenseDetailsListsListsOnError(err)
    );
  }

  private getTotalExpenseDetailsListsOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.setDetailsLists(response);
  }
  private getTotalExpenseDetailsListsListsOnError(response: any) {
    this.alertService.viewAlerts(response.error.alerts);
  }

  private setDetailsLists(response: RequestedData<any>) {
    if (listHasValue(response.detailsList)) { }
    response.detailsList.forEach(list => {

      switch (list.detailsListId) {
        case DetailsListEnum.Clinic:
          this.clinicList = list.list;
          this.filteredClinicList = this.clinicList;
          break;

        case DetailsListEnum.User:
          this.userList = list.list;
          this.filteredUserList = this.userList;
          break;
      }
    });
  }

  openReport() {
    if (this.totalExpenseFilter.dateFrom == null || this.totalExpenseFilter.dateTo == null) {
      return;
    }
    let requestedData = new RequestedData<TotalExpenseFilter>();
    requestedData.entity = this.totalExpenseFilter;

    this.reportService.getTotalExpenseReport(requestedData).subscribe(
      res => this.getReportOnSuccess(res),
      err => this.alertService.viewAlerts(err.error.alerts)
    );
  }

  getReportOnSuccess(reportPDF: any) {
    const dialogRef = this.dialog.open(ReportPopupComponent, {
      width: '1100px',
      height: '640px',
      maxHeight: '650px',
      data: { reportTitle: " تقرير اجمالى المصاريف", pdf: reportPDF }
    });
  }

}
