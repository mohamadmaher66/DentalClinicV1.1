import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/service/http-service';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  url: string = "api/patient/";

  constructor(private httpService: HttpService) { }

  public getAllPatients(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllPatients');
  }

  public getAllPatientLite(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllPatientLite');
  }

  public getPatient(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetPatient');
  }

  public addPatient(object: any) {
    return this.httpService.httpPost(object, this.url + 'AddPatient');
  }

  public editPatient(object: any) {
    return this.httpService.httpPost(object, this.url + 'EditPatient');
  }
  public deletePatient(object: any) {
    return this.httpService.httpPost(object, this.url + 'DeletePatient');
  }

  public getPatientDetailsLists(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetPatientDetailsLists');
  }
}
