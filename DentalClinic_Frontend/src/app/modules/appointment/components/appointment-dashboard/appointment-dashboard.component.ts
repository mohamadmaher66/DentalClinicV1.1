import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Appointment } from '../../../../core/models/appointment.model';
import { Clinic } from '../../../../core/models/clinic.model';
import { User } from '../../../../core/models/user.model';
import { AppointmentService } from '../../../../core/servcies/appointment.service';
import { AppointmentStateEnum } from '../../../../shared/enum/appointment-state.enum';
import { ToothPositionEnum } from '../../../../shared/enum/appointment-tooth.enum';
import { DetailsListEnum } from '../../../../shared/enum/details-list.enum';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { hasValue, listHasValue } from '../../../../shared/service/helper.service';

@Component({
  selector: 'app-appointment-dashboard',
  templateUrl: './appointment-dashboard.component.html'
})
export class AppointmentDashboardComponent {

  appointmentList = new Array<Appointment>();
  appointmentStateEnum = AppointmentStateEnum; 
  toothPositionEnum = ToothPositionEnum;
  filterEntity = new Appointment();

  filteredClinicList = new Array<Clinic>();
  clinicList = new Array<Clinic>();
  filteredUserList = new Array<User>();
  userList = new Array<User>();

  constructor(private appointmentService: AppointmentService,
    public router: Router,
    public alertService: AlertService) { }

  ngAfterViewInit() {
    this.getAllAppointments();
  }

  public getAllAppointments() {
    let requestedAppointmentInfo = new RequestedData<Appointment>();
    requestedAppointmentInfo.entity = this.filterEntity;

    this.appointmentService.getAllDashboard(requestedAppointmentInfo)
    .subscribe(
      res => this.getAllDashboardOnSuccess(res),
      err => this.getAllDashboardOnError(err)
    );
  }

  private getAllDashboardOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.appointmentList = response.entityList;
    this.setDetailsLists(response);
  }

  private getAllDashboardOnError(response: any): void {
    this.alertService.viewAlerts(response.error.alerts);
  }

  editRecord(id: number) {
    this.router.navigate(['appointment/appointmentdetails', id]);
  }

  addNewRecord() {
    this.router.navigate(['appointment/appointmentdetails']);
  }

  public saveState(appointment: Appointment, state: AppointmentStateEnum){
    let requestedAppointmentInfo = new RequestedData<Appointment>();
    appointment.state = state;
    requestedAppointmentInfo.entity = appointment;

    this.appointmentService.saveState(requestedAppointmentInfo)
    .subscribe(
      res => this.saveStateOnSuccess(res),
      err => this.alertService.viewAlerts(err.error.alerts)
    );
  }

  private saveStateOnSuccess(response:any) {
    this.alertService.viewAlerts(response.alerts);
    this.getAllAppointments();
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

  public selectClinic(clinicId: number){
    this.filterEntity.clinic = new Clinic();
    this.filterEntity.clinic.id = clinicId;
    this.getAllAppointments();
  }

  public selectUser(userId: number){
    this.filterEntity.user = new User();
    this.filterEntity.user.id = userId;
    this.getAllAppointments();
  }
}
