import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/service/http-service';

@Injectable({
  providedIn: 'root'
})
export class AppointmentAdditionService {

  url: string = "api/appointmentaddition/";

  constructor(private httpService: HttpService) { }

  public getAllAppointmentAdditions(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllAppointmentAdditions');
  }

  public getAllAppointmentAdditionLite(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllAppointmentAdditionLite');
  }

  public getAppointmentAddition(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAppointmentAddition');
  }

  public addAppointmentAddition(object: any) {
    return this.httpService.httpPost(object, this.url + 'AddAppointmentAddition');
  }

  public editAppointmentAddition(object: any) {
    return this.httpService.httpPost(object, this.url + 'EditAppointmentAddition');
  }
  public deleteAppointmentAddition(object: any) {
    return this.httpService.httpPost(object, this.url + 'DeleteAppointmentAddition');
  }

}
