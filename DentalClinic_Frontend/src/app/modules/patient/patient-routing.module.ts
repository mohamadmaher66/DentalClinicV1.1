import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorAuthGaurd } from '../../shared/service/doctorAuthGaurd.service';
import { PatientComponent } from './components/patient/patient.component';

const routes: Routes = [
  { path:'', component: PatientComponent, canActivate:[DoctorAuthGaurd] },
  { path:'patient', component: PatientComponent, canActivate:[DoctorAuthGaurd]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientRoutingModule { }
