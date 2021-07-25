import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientRoutingModule } from './patient-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { PatientDetailsComponent } from './components/patient-details/patient-details.component';
import { PatientComponent } from './components/patient/patient.component';

@NgModule({
  declarations: [PatientComponent, PatientDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    PatientRoutingModule
  ]
})
export class PatientModule { }
