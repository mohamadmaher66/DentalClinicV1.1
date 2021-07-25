import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentDashboardComponent } from './components/appointment-dashboard/appointment-dashboard.component';
import { AppointmentDetailsComponent } from './components/appointment-details/appointment-details.component';
import { AppointmentComponent } from './components/appointment/appointment.component';

const routes: Routes = [
  { path:'', component: AppointmentComponent },
  { path:'appointment', component: AppointmentComponent },
  { path:'appointmentdetails', component: AppointmentDetailsComponent },
  { path:'appointmentdetails/:id', component: AppointmentDetailsComponent },
  { path:'dashboard', component: AppointmentDashboardComponent },
  { path:'dashboard/:id', component: AppointmentDashboardComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppointmentRoutingModule { }
