import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MedicalHistoryRoutingModule } from './medical-history-routing.module';
import { MedicalHistoryComponent } from './components/medical-history/medical-history.component';
import { SharedModule } from '../../shared/shared.module';
import { MedicalHistoryDetailsComponent } from './components/medical-history-details/medical-history-details.component';

@NgModule({
  declarations: [MedicalHistoryComponent, MedicalHistoryDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    MedicalHistoryRoutingModule
  ]
})
export class MedicalHistoryModule { }
