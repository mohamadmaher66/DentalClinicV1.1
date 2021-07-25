import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorAuthGaurd } from '../../shared/service/doctorAuthGaurd.service';
import { UserComponent } from './components/user/user.component';

const routes: Routes = [
  { path:'', component: UserComponent, canActivate:[DoctorAuthGaurd] },
  { path:'user', component: UserComponent, canActivate:[DoctorAuthGaurd]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
