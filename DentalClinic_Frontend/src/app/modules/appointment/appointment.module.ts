import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppointmentRoutingModule } from './appointment-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { AppointmentDetailsComponent } from './components/appointment-details/appointment-details.component';
import { AppointmentComponent } from './components/appointment/appointment.component';
import { FileUploaderComponent } from '../../shared/components/file-uploader/file-uploader.component';
import { PatientModule } from '../patient/patient.module';
import { AppointmentDashboardComponent } from './components/appointment-dashboard/appointment-dashboard.component';

@NgModule({
  declarations: [
    AppointmentComponent,
    AppointmentDetailsComponent,
    FileUploaderComponent,
    AppointmentDashboardComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AppointmentRoutingModule,
    PatientModule
  ]
  
})
export class AppointmentModule { }
