import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClinicRoutingModule } from './clinic-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { ClinicDetailsComponent } from './components/clinic-details/clinic-details.component';
import { ClinicComponent } from './components/clinic/clinic.component';

@NgModule({
  declarations: [ClinicComponent, ClinicDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    ClinicRoutingModule
  ]
})
export class ClinicModule { }
