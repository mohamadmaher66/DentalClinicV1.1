import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppointmentAddition } from '../../../../core/models/appointment-addition.model';
import { AppointmentAdditionService } from '../../../../core/servcies/appointment-addition.service';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';

@Component({
  selector: 'app-appointment-addition-details',
  templateUrl: './appointment-addition-details.component.html'
})
export class AppointmentAdditionDetailsComponent implements OnInit {
  //Variables
  public appointmentAddition: AppointmentAddition = new AppointmentAddition();
  requestAppointmentAdditionData: RequestedData<AppointmentAddition>;
  btnTitle: string = "حفظ";

  constructor(private appointmentAdditionService: AppointmentAdditionService,
    private alertService: AlertService,
    public dialogRef: MatDialogRef<AppointmentAdditionDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    }

  ngOnInit() {
    this.appointmentAddition.id = this.data.selectedDetails;
    if (this.appointmentAddition.id > 0) {
      this.btnTitle = 'حفظ';
      this.getAppointmentAddition();
    }
  }

  private getAppointmentAddition(){
    this.requestAppointmentAdditionData = new RequestedData<AppointmentAddition>();
    this.requestAppointmentAdditionData.entity = new AppointmentAddition;
    this.requestAppointmentAdditionData.entity.id = this.appointmentAddition.id;

    this.appointmentAdditionService.getAppointmentAddition(this.requestAppointmentAdditionData).subscribe(
      res => this.getAppointmentAdditionOnSuccess(res) ,        
      err => this.getAppointmentAdditionOnError(err)
    );
  }

  private getAppointmentAdditionOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.appointmentAddition = response.entity as AppointmentAddition;
  }

  private getAppointmentAdditionOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  public submitAppointmentAddition(form:NgForm) {
    if(form.invalid){
      return;
    }
    this.requestAppointmentAdditionData = new RequestedData<AppointmentAddition>();
    this.requestAppointmentAdditionData.entity = this.appointmentAddition;

    if (this.appointmentAddition.id > 0) {
      this.editAppointmentAddition();
    } 
    else {
      this.addAppointmentAddition();
    }
  }

  private addAppointmentAddition(){
    this.requestAppointmentAdditionData.entity.id = 0;
    this.appointmentAdditionService.addAppointmentAddition(this.requestAppointmentAdditionData).subscribe(
      res => this.appointmentAdditionActionOnSuccess(res),
      err => this.appointmentAdditionActionOnError(err)
    );
  }

  private editAppointmentAddition(){
    this.appointmentAdditionService.editAppointmentAddition(this.requestAppointmentAdditionData).subscribe(
      res => this.appointmentAdditionActionOnSuccess(res),
      err => this.appointmentAdditionActionOnError(err)
    );
  }

  private appointmentAdditionActionOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.dialogRef.close();
  }

  private appointmentAdditionActionOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

}
