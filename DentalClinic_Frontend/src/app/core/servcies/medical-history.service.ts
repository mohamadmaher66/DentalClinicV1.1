import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/service/http-service';
import { MedicalHistory } from '../models/medical-history.model';

@Injectable({
  providedIn: 'root'
})
export class MedicalHistoryService {

  url: string = "api/medicalhistory/";
  medicalHistorys: MedicalHistory[];

  constructor(private httpService: HttpService) { }

  public getAllMedicalHistorys(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllMedicalHistorys');
  }

  public getAllMedicalHistoryLite(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllMedicalHistoryLite');
  }

  public getMedicalHistory(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetMedicalHistory');
  }

  public addMedicalHistory(object: any) {
    return this.httpService.httpPost(object, this.url + 'AddMedicalHistory');
  }

  public editMedicalHistory(object: any) {
    return this.httpService.httpPost(object, this.url + 'EditMedicalHistory');
  }
  public deleteMedicalHistory(object: any) {
    return this.httpService.httpPost(object, this.url + 'DeleteMedicalHistory');
  }

}
