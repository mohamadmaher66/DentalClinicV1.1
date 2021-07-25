import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Clinic } from '../../../../core/models/clinic.model';
import { ClinicService } from '../../../../core/servcies/clinic.service';
import { RoleEnum } from '../../../../shared/enum/role.enum';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';

@Component({
  selector: 'app-clinic-details',
  templateUrl: './clinic-details.component.html'
})
export class ClinicDetailsComponent implements OnInit {
  //Variables
  public clinic: Clinic = new Clinic();
  requestClinicData: RequestedData<Clinic>;
  btnTitle: string = "حفظ";
  roleEnum = RoleEnum;

  constructor(private clinicService: ClinicService,
    private alertService: AlertService,
    public dialogRef: MatDialogRef<ClinicDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    }

  ngOnInit() {
    this.clinic.id = this.data.selectedDetails;
    if (this.clinic.id > 0) {
      this.btnTitle = 'حفظ';
      this.getClinic();
    }
  }

  private getClinic(){
    this.requestClinicData = new RequestedData<Clinic>();
    this.requestClinicData.entity = new Clinic;
    this.requestClinicData.entity.id = this.clinic.id;

    this.clinicService.getClinic(this.requestClinicData).subscribe(
      res => this.getClinicOnSuccess(res) ,        
      err => this.getClinicOnError(err)
    );
  }

  private getClinicOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.clinic = response.entity as Clinic;
  }

  private getClinicOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  public submitClinic(form:NgForm) {
    if(form.invalid){
      return;
    }
    this.requestClinicData = new RequestedData<Clinic>();
    this.requestClinicData.entity = this.clinic;

    if (this.clinic.id > 0) {
      this.editClinic();
    } 
    else {
      this.addClinic();
    }
  }

  private addClinic(){
    this.requestClinicData.entity.id = 0;
    this.clinicService.addClinic(this.requestClinicData).subscribe(
      res => this.clinicActionOnSuccess(res),
      err => this.clinicActionOnError(err)
    );
  }

  private editClinic(){
    this.clinicService.editClinic(this.requestClinicData).subscribe(
      res => this.clinicActionOnSuccess(res),
      err => this.clinicActionOnError(err)
    );
  }

  private clinicActionOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.dialogRef.close();
  }

  private clinicActionOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

}
