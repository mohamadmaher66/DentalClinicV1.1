import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppointmentCategory } from '../../../../core/models/appointment-category.model';
import { AppointmentCategoryService } from '../../../../core/servcies/appointment-category.service';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';

@Component({
  selector: 'app-appointment-category-details',
  templateUrl: './appointment-category-details.component.html'
})
export class AppointmentCategoryDetailsComponent implements OnInit {
  //Variables
  public appointmentCategory: AppointmentCategory = new AppointmentCategory();
  requestAppointmentCategoryData: RequestedData<AppointmentCategory>;
  btnTitle: string = "حفظ";

  constructor(private appointmentCategoryService: AppointmentCategoryService,
    private alertService: AlertService,
    public dialogRef: MatDialogRef<AppointmentCategoryDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    }

  ngOnInit() {
    this.appointmentCategory.id = this.data.selectedDetails;
    if (this.appointmentCategory.id > 0) {
      this.btnTitle = 'حفظ';
      this.getAppointmentCategory();
    }
  }

  private getAppointmentCategory(){
    this.requestAppointmentCategoryData = new RequestedData<AppointmentCategory>();
    this.requestAppointmentCategoryData.entity = new AppointmentCategory;
    this.requestAppointmentCategoryData.entity.id = this.appointmentCategory.id;

    this.appointmentCategoryService.getAppointmentCategory(this.requestAppointmentCategoryData).subscribe(
      res => this.getAppointmentCategoryOnSuccess(res) ,        
      err => this.getAppointmentCategoryOnError(err)
    );
  }

  private getAppointmentCategoryOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.appointmentCategory = response.entity as AppointmentCategory;
  }

  private getAppointmentCategoryOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  public submitAppointmentCategory(form:NgForm) {
    if(form.invalid){
      return;
    }
    this.requestAppointmentCategoryData = new RequestedData<AppointmentCategory>();
    this.requestAppointmentCategoryData.entity = this.appointmentCategory;

    if (this.appointmentCategory.id > 0) {
      this.editAppointmentCategory();
    } 
    else {
      this.addAppointmentCategory();
    }
  }

  private addAppointmentCategory(){
    this.requestAppointmentCategoryData.entity.id = 0;
    this.appointmentCategoryService.addAppointmentCategory(this.requestAppointmentCategoryData).subscribe(
      res => this.appointmentCategoryActionOnSuccess(res),
      err => this.appointmentCategoryActionOnError(err)
    );
  }

  private editAppointmentCategory(){
    this.appointmentCategoryService.editAppointmentCategory(this.requestAppointmentCategoryData).subscribe(
      res => this.appointmentCategoryActionOnSuccess(res),
      err => this.appointmentCategoryActionOnError(err)
    );
  }

  private appointmentCategoryActionOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.dialogRef.close();
  }

  private appointmentCategoryActionOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

}
