import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppointmentAdditionRoutingModule } from './appointment-addition-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { AppointmentAdditionDetailsComponent } from './components/appointment-addition-details/appointment-addition-details.component';
import { AppointmentAdditionComponent } from './components/appointment-addition/appointment-addition.component';

@NgModule({
  declarations: [AppointmentAdditionComponent, AppointmentAdditionDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    AppointmentAdditionRoutingModule
  ]
})
export class AppointmentAdditionModule { }
