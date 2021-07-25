import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/service/http-service';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  url: string = "api/appointment/";

  constructor(private httpService: HttpService) { }

  public getAllAppointments(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllAppointments');
  }

  public getAllDashboard(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllAppointmentsDashboard');
  }

  public getAllAppointmentLite(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllAppointmentLite');
  }

  public getAppointment(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAppointment');
  }

  public addAppointment(object: any) {
    return this.httpService.httpPost(object, this.url + 'AddAppointment');
  }

  public editAppointment(object: any) {
    return this.httpService.httpPost(object, this.url + 'EditAppointment');
  }
  public deleteAppointment(object: any) {
    return this.httpService.httpPost(object, this.url + 'DeleteAppointment');
  }
  public getAppointmentDetailsLists(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAppointmentDetailsLists');
  }
  public saveState(object: any) {
    return this.httpService.httpPost(object, this.url + 'SaveState');
  }

  
}
