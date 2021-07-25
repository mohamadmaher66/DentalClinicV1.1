import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorAuthGaurd } from '../../shared/service/doctorAuthGaurd.service';
import { ClinicComponent } from './components/clinic/clinic.component';

const routes: Routes = [
  { path:'', component: ClinicComponent, canActivate:[DoctorAuthGaurd] },
  { path:'clinic', component: ClinicComponent, canActivate:[DoctorAuthGaurd]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClinicRoutingModule { }
