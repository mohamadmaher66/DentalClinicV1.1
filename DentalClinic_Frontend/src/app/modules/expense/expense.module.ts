import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExpenseRoutingModule } from './expense-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { ExpenseDetailsComponent } from './components/expense-details/expense-details.component';
import { ExpenseComponent } from './components/expense/expense.component';

@NgModule({
  declarations: [ExpenseComponent, ExpenseDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    ExpenseRoutingModule
  ]
})
export class ExpenseModule { }
