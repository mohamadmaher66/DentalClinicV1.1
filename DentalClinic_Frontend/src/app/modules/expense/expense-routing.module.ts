import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorAuthGaurd } from '../../shared/service/doctorAuthGaurd.service';
import { ExpenseComponent } from './components/expense/expense.component';

const routes: Routes = [
  { path:'', component: ExpenseComponent, canActivate:[DoctorAuthGaurd] },
  { path:'expense', component: ExpenseComponent, canActivate:[DoctorAuthGaurd]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExpenseRoutingModule { }
