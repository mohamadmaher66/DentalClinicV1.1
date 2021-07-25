import { INavData } from '@coreui/angular';

export const doctorNavItems: INavData[] = [
  {
    title: true,
    name: 'Components'
  },
  {
    name: 'كشوفات اليوم',
    url: '/appointment/dashboard',
    icon: 'cil-calendar-check'
  },
  {
    name: 'الكشوفات',
    url: '/appointment',
    icon: 'cil-briefcase'
  },
  {
    name: 'المستخدمين',
    url: '/user',
    icon: 'cil-user'
  },
  {
    name: 'المرضي',
    url: '/patient',
    icon: 'cil-medical-cross'
  },
  {
    name: 'المصاريف',
    url: '/expense',
    icon: 'cil-dollar'
  },
  {
    name: 'البيانات الرئيسية',
    url: '/base',
    icon: 'cil-bookmark',
    children: [
      {
        name: 'العيادات',
        url: '/clinic',
        icon: 'cil-location-pin'
      },
      {
        name: 'التاريخ الطبي',
        url: '/medicalhistory',
        icon: 'cil-hospital'
      },
      {

        name: 'اضافات للكشف',
        url: '/appointmentaddition',
        icon: 'cil-plus'
      },
      {
        name: 'انواع الكشف',
        url: '/appointmentcategory',
        icon: 'cil-layers'
      }
    ]
  },
  {
    name: 'التقارير',
    url: '/report',
    icon: 'cil-print',
    children: [
      {
        name: 'تقرير المصاريف',
        url: '/report/expensereport',
        icon: 'cil-dollar'
      },
      {
        name: 'تقرير الكشوفات',
        url: '/report/appointmentreport',
        icon: 'cil-briefcase'
      },
      {
        name: 'تقرير اجمالى المصاريف',
        url: '/report/totalexpensereport',
        icon: 'cil-chart-line'
      }
      
    ]
  }
];
