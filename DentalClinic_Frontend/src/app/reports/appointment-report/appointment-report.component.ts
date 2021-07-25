import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Clinic } from '../../core/models/clinic.model';
import { AppointmentFilter } from '../../core/models/appointment-filter.model';
import { User } from '../../core/models/user.model';
import { ReportService } from '../../core/servcies/report.service';
import { DetailsListEnum } from '../../shared/enum/details-list.enum';
import { RequestedData } from '../../shared/models/request-data.entity';
import { AlertService } from '../../shared/service/alert.service';
import { listHasValue } from '../../shared/service/helper.service';
import { ReportPopupComponent } from '../report-popup/report-popup.component';
import { AppointmentStateEnum } from '../../shared/enum/appointment-state.enum';
import { Patient } from '../../core/models/patient.model';
import { AppointmentCategory } from '../../core/models/appointment-category.model';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { BaseComponent } from '../../shared/components/base-component/base-component';

@Component({
  selector: 'app-appointment-report',
  templateUrl: './appointment-report.component.html'
})
export class AppointmentReportComponent extends BaseComponent implements OnInit {

  appointmentFilter = new AppointmentFilter();
  appointmentStateEnum = AppointmentStateEnum;

  // Lists
  clinicList = new Array<Clinic>();
  filteredClinicList = new Array<Clinic>();
  userList = new Array<User>();
  filteredUserList = new Array<User>();
  patientList = new Array<Patient>();
  filteredPatientList = new Array<Patient>();
  categoryList = new Array<AppointmentCategory>();
  filteredCategoryList = new Array<AppointmentCategory>();

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

  public filterPatient(value: string) {
    let filter = value.toLowerCase();
    this.filteredPatientList = this.patientList
      .filter(option => option.fullName.toLowerCase().startsWith(filter) || option.fullName.startsWith(filter));
  }

  public filterCategory(value: string) {
    let filter = value.toLowerCase();
    this.filteredCategoryList = this.categoryList
      .filter(option => option.name.toLowerCase().startsWith(filter));
  }

  private getDetailsLists() {
    let requestedDate = new RequestedData<AppointmentFilter>();

    this.reportService.getAppointmentDetailsLists(requestedDate).subscribe(
      res => this.getAppointmentDetailsListsOnSuccess(res),
      err => this.getAppointmentDetailsListsListsOnError(err)
    );
  }

  private getAppointmentDetailsListsOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.setDetailsLists(response);
  }
  private getAppointmentDetailsListsListsOnError(response: any) {
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

        case DetailsListEnum.Patient:
          this.patientList = list.list;
          this.filteredPatientList = this.patientList;
          break;

        case DetailsListEnum.AppointmentCategory:
          this.categoryList = list.list;
          this.filteredCategoryList = this.categoryList;
          break;
      }
    });
  }

  openReport() {
    if (this.appointmentFilter.dateFrom == null || this.appointmentFilter.dateTo == null) {
      return;
    }
    let requestedData = new RequestedData<AppointmentFilter>();
    requestedData.entity = this.appointmentFilter;

    this.reportService.getAppointmentReport(requestedData).subscribe(
      res => this.getReportOnSuccess(res),
      err => this.alertService.viewAlerts(err.error.alerts)
    );
  }

  getReportOnSuccess(reportPDF: any) {
    const dialogRef = this.dialog.open(ReportPopupComponent, {
      width: '1100px',
      height: '640px',
      maxHeight: '650px',
      data: { reportTitle: "تقرير الكشوفات", pdf: reportPDF }
    });
  }

}
