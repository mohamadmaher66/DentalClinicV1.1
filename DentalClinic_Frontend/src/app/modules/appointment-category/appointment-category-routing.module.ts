import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorAuthGaurd } from '../../shared/service/doctorAuthGaurd.service';
import { AppointmentCategoryComponent } from './components/appointment-category/appointment-category.component';

const routes: Routes = [
  { path:'', component: AppointmentCategoryComponent, canActivate:[DoctorAuthGaurd] },
  { path:'appointment-category', component: AppointmentCategoryComponent, canActivate:[DoctorAuthGaurd]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppointmentCategoryRoutingModule { }
