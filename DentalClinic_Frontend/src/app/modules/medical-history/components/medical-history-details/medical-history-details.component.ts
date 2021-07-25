import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { MedicalHistory } from '../../../../core/models/medical-history.model';
import { MedicalHistoryService } from '../../../../core/servcies/medical-history.service';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';

@Component({
  selector: 'app-medical-history-details',
  templateUrl: './medical-history-details.component.html'
})
export class MedicalHistoryDetailsComponent implements OnInit {
  //Variables
  public medicalHistory: MedicalHistory = new MedicalHistory();
  requestMedicalHistoryData: RequestedData<MedicalHistory>;
  btnTitle: string = "حفظ";

  constructor(private medicalHistoryService: MedicalHistoryService,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    public dialogRef: MatDialogRef<MedicalHistoryDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
      
    }

  ngOnInit() {
    this.medicalHistory.id = this.data.selectedDetails;

    if (this.medicalHistory.id > 0) {
      this.btnTitle = 'حفظ';
      this.getMedicalHistory();
    }

  }

  private getMedicalHistory(){
    this.requestMedicalHistoryData = new RequestedData<MedicalHistory>();
    this.requestMedicalHistoryData.entity = new MedicalHistory;
    this.requestMedicalHistoryData.entity.id = this.medicalHistory.id;

    this.medicalHistoryService.getMedicalHistory(this.requestMedicalHistoryData).subscribe(
      res => this.getMedicalHistoryOnSuccess(res) ,        
      err => this.getMedicalHistoryOnError(err)
    );
  }

  private getMedicalHistoryOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.medicalHistory = response.entity as MedicalHistory;
  }

  private getMedicalHistoryOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  public submitMedicalHistory(form:NgForm) {
    if(form.invalid){
      return;
    }
    this.requestMedicalHistoryData = new RequestedData<MedicalHistory>();
    this.requestMedicalHistoryData.entity = this.medicalHistory;

    if (this.medicalHistory.id > 0) {
      this.editMedicalHistory();
    } 
    else {
      this.addMedicalHistory();
    }
  }

  private addMedicalHistory(){
    this.requestMedicalHistoryData.entity.id = 0;
    this.medicalHistoryService.addMedicalHistory(this.requestMedicalHistoryData).subscribe(
      res => this.medicalHistoryActionOnSuccess(res),
      err => this.medicalHistoryActionOnError(err)
    );

  }

  private editMedicalHistory(){
    this.medicalHistoryService.editMedicalHistory(this.requestMedicalHistoryData).subscribe(
      res => this.medicalHistoryActionOnSuccess(res),
      err => this.medicalHistoryActionOnError(err)
    );
  }

  private medicalHistoryActionOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.dialogRef.close();
  }

  private medicalHistoryActionOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

}
