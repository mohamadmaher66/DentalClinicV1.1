import { Injectable } from '@angular/core';
import { Alert } from '../models/alert.entity';
import { ToastrService } from 'ngx-toastr';
import { AlertType } from '../enum/alert-type.enum';
import { listHasValue } from './helper.service';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  constructor(private toastr: ToastrService) { }

  public viewAlerts(alerts:Alert[] ){
    if(listHasValue(alerts)){
      alerts.forEach(alert => {
        switch(alert.type){
          case AlertType.Error: this.toastr.error(alert.message, alert.title); console.log(alert.message); break;
          case AlertType.Warning: this.toastr.warning(alert.message, alert.title); break;
          case AlertType.Success: this.toastr.success(alert.message, alert.title); break;
          case AlertType.Info: this.toastr.info(alert.message, alert.title); break;
        }
      })
    }
  }
}
