import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorAuthGaurd } from '../../shared/service/doctorAuthGaurd.service';
import { MedicalHistoryComponent } from './components/medical-history/medical-history.component';

const routes: Routes = [
  { path:'', component: MedicalHistoryComponent, canActivate:[DoctorAuthGaurd] },
  { path:'medicalhistory', component: MedicalHistoryComponent, canActivate:[DoctorAuthGaurd] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MedicalHistoryRoutingModule { }
