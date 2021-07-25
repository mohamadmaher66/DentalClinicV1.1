import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DefaultLayoutComponent } from './containers';
import { LoginComponent } from './modules/user/components/login/login.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, data: { title: 'Login Page' } },
  { path: '', component: DefaultLayoutComponent, data: { title: 'Home' },
    children: [
      // App Components
      {
        path: 'medicalhistory',
        loadChildren: () => import('./modules/medical-history/medical-history.module').then(m => m.MedicalHistoryModule) ,
        data:{ title: 'التاريخ الطبي'}
      },
      {
        path: 'user',
        loadChildren: () => import('./modules/user/user.module').then(m => m.UserModule) ,
        data:{ title: 'المستخدمين'}
      },
      {
        path: 'clinic',
        loadChildren: () => import('./modules/clinic/clinic.module').then(m => m.ClinicModule) ,
        data:{ title: 'العيادات'}
      },
      {
        path: 'appointmentaddition',
        loadChildren: () => import('./modules/appointment-addition/appointment-addition.module').then(m => m.AppointmentAdditionModule) ,
        data:{ title: 'اضافات الكشف'}
      },
      {
        path: 'appointmentcategory',
        loadChildren: () => import('./modules/appointment-category/appointment-category.module').then(m => m.AppointmentCategoryModule) ,
        data:{ title: 'انواع الكشف'}
      },
      {
        path: 'expense',
        loadChildren: () => import('./modules/expense/expense.module').then(m => m.ExpenseModule) ,
        data:{ title: 'المصاريف'}
      },
      {
        path: 'patient',
        loadChildren: () => import('./modules/patient/patient.module').then(m => m.PatientModule) ,
        data:{ title: 'المرضي'}
      },
      {
        path: 'appointment',
        loadChildren: () => import('./modules/appointment/appointment.module').then(m => m.AppointmentModule) ,
        data:{ title: 'الكشوفات'}
      },
      {
        path: 'report',
        loadChildren: () => import('./reports/reports.module').then(m => m.ReportsModule),
        data:{ title: 'التقارير'}
      }
    ]
  }
  //{ path: '**', component: P404Component },
  
];

@NgModule({
  imports: [ RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' }) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
