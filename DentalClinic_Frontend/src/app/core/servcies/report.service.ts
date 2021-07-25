import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/service/http-service';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
 
  url: string = "api/report/";

  constructor(private httpService: HttpService) { }

  public getExpenseReport(object: any) {
    return this.httpService.httpDownloadFile(object, this.url + 'GetExpenseReport');
  }
  public getExpenseDetailsLists(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetExpenseDetailsLists');
  }
  public getAppointmentDetailsLists(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAppointmentDetailsLists');
  }
  public getAppointmentReport(object: any) {
    return this.httpService.httpDownloadFile(object, this.url + 'GetAppointmentReport');
  }
  public getTotalExpenseReport(object: any) {
    return this.httpService.httpDownloadFile(object, this.url + 'GetTotalExpenseReport');
  }
}
