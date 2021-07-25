import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MedicalHistory } from '../../../../core/models/medical-history.model';
import { Patient } from '../../../../core/models/patient.model';
import { PatientService } from '../../../../core/servcies/patient.service';
import { DetailsListEnum } from '../../../../shared/enum/details-list.enum';
import { GenderEnum } from '../../../../shared/enum/gender.enum';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { hasValue, listHasValue } from '../../../../shared/service/helper.service';

@Component({
  selector: 'app-Patient-details',
  templateUrl: './Patient-details.component.html'
})
export class PatientDetailsComponent implements OnInit {
  //Variables
  public patient: Patient = new Patient();
  requestPatientData: RequestedData<Patient>;
  btnTitle: string = "حفظ";
  genderEnum = GenderEnum;
  medicalHistoryList = new Array<MedicalHistory>();
  selectedMedicalHistoryList = new Array<MedicalHistory>();

  constructor(private PatientService: PatientService,
    private alertService: AlertService,
    public dialogRef: MatDialogRef<PatientDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    }

  ngOnInit() {
    this.patient.id = this.data.selectedDetails;
    if (this.patient.id > 0) {
      this.btnTitle = 'حفظ';
      this.getPatient();
    }
    else{
      this.getDetailsLists();
    }
  }

  private getPatient(){
    this.requestPatientData = new RequestedData<Patient>();
    this.requestPatientData.entity = new Patient;
    this.requestPatientData.entity.id = this.patient.id;

    this.PatientService.getPatient(this.requestPatientData).subscribe(
      res => this.getPatientOnSuccess(res) ,        
      err => this.getPatientOnError(err)
    );
  }

  private getPatientOnSuccess(response: RequestedData<Patient>){
    this.alertService.viewAlerts(response.alerts);
    this.patient = response.entity as Patient;
    this.selectedMedicalHistoryList = response.entity.medicalHistoryList;
    this.setDetailsLists(response);
  }

  private getPatientOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  public submitPatient(form:NgForm) {
    if(form.invalid){
      return;
    }
    this.requestPatientData = new RequestedData<Patient>();
    this.requestPatientData.entity = this.patient;
    this.requestPatientData.entity.medicalHistoryList = this.selectedMedicalHistoryList;
     
    if (this.patient.id > 0) {
      this.editPatient();
    } 
    else {
      this.addPatient();
    }
  }

  private addPatient(){
    this.requestPatientData.entity.id = 0;
    this.PatientService.addPatient(this.requestPatientData).subscribe(
      res => this.PatientActionOnSuccess(res),
      err => this.PatientActionOnError(err)
    );
  }

  private editPatient(){
    this.PatientService.editPatient(this.requestPatientData).subscribe(
      res => this.PatientActionOnSuccess(res),
      err => this.PatientActionOnError(err)
    );
  }

  private PatientActionOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.dialogRef.close();
  }

  private PatientActionOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  private getDetailsLists(){
    this.requestPatientData = new RequestedData<Patient>();

    this.PatientService.getPatientDetailsLists(this.requestPatientData).subscribe(
      res => this.getPatientDetailsListsOnSuccess(res) ,        
      err => this.getPatientDetailsListsOnError(err)
    );
  }

  private getPatientDetailsListsOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.setDetailsLists(response);
  }

  private getPatientDetailsListsOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  private setDetailsLists(response: RequestedData<any>){
    if (listHasValue(response.detailsList)) { }
      response.detailsList.forEach(list => {
        switch (list.detailsListId) {
          case DetailsListEnum.MedicalHistory:
            this.medicalHistoryList = list.list;
        }
      });
  }

  public selectMedicalHistory(selectedId: number, checked: boolean){
    if(checked){
      this.selectedMedicalHistoryList.push({id: selectedId} as MedicalHistory);
    }
    else{
      this.selectedMedicalHistoryList = this.selectedMedicalHistoryList.filter(m => m.id != selectedId);
    }
  }

  public isChecked(checkBoxId: number){
    if(listHasValue(this.selectedMedicalHistoryList) && this.selectedMedicalHistoryList.some(m => m.id == checkBoxId)){
      return true;
    }
    else{
      return false;
    }
  }

}
