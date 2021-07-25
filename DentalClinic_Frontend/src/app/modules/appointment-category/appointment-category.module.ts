import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppointmentCategoryRoutingModule } from './appointment-category-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { AppointmentCategoryDetailsComponent } from './components/appointment-category-details/appointment-category-details.component';
import { AppointmentCategoryComponent } from './components/appointment-category/appointment-category.component';

@NgModule({
  declarations: [AppointmentCategoryComponent, AppointmentCategoryDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    AppointmentCategoryRoutingModule
  ]
})
export class AppointmentCategoryModule { }
