import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/service/http-service';

@Injectable({
  providedIn: 'root'
})
export class ClinicService {

  url: string = "api/clinic/";

  constructor(private httpService: HttpService) { }

  public getAllClinics(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllClinics');
  }

  public getAllClinicLite(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllClinicLite');
  }

  public getClinic(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetClinic');
  }

  public addClinic(object: any) {
    return this.httpService.httpPost(object, this.url + 'AddClinic');
  }

  public editClinic(object: any) {
    return this.httpService.httpPost(object, this.url + 'EditClinic');
  }
  public deleteClinic(object: any) {
    return this.httpService.httpPost(object, this.url + 'DeleteClinic');
  }

}
