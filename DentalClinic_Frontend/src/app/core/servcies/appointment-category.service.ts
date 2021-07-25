import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/service/http-service';

@Injectable({
  providedIn: 'root'
})
export class AppointmentCategoryService {

  url: string = "api/appointmentcategory/";

  constructor(private httpService: HttpService) { }

  public getAllAppointmentCategorys(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllAppointmentCategorys');
  }

  public getAllAppointmentCategoryLite(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllAppointmentCategoryLite');
  }

  public getAppointmentCategory(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAppointmentCategory');
  }

  public addAppointmentCategory(object: any) {
    return this.httpService.httpPost(object, this.url + 'AddAppointmentCategory');
  }

  public editAppointmentCategory(object: any) {
    return this.httpService.httpPost(object, this.url + 'EditAppointmentCategory');
  }
  public deleteAppointmentCategory(object: any) {
    return this.httpService.httpPost(object, this.url + 'DeleteAppointmentCategory');
  }

}
