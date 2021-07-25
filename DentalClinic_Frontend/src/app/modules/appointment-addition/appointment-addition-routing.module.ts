import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorAuthGaurd } from '../../shared/service/doctorAuthGaurd.service';
import { AppointmentAdditionComponent } from './components/appointment-addition/appointment-addition.component';

const routes: Routes = [
  { path:'', component: AppointmentAdditionComponent, canActivate:[DoctorAuthGaurd] },
  { path:'appointment-addition', component: AppointmentAdditionComponent, canActivate:[DoctorAuthGaurd]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppointmentAdditionRoutingModule { }
