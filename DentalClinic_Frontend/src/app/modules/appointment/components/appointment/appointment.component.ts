import { formatDate } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, ViewChild } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { Appointment } from '../../../../core/models/appointment.model';
import { AppointmentService } from '../../../../core/servcies/appointment.service';
import { BaseComponent } from '../../../../shared/components/base-component/base-component';
import { DeleteDialogComponent } from '../../../../shared/components/delete-dialog/delete-dialog.component';
import { AppointmentStateEnum } from '../../../../shared/enum/appointment-state.enum';
import { GridSettings } from '../../../../shared/models/grid-settings.entity';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { ConfigService } from '../../../../shared/service/config.service';
import { hasValue } from '../../../../shared/service/helper.service';

@Component({
  selector: 'app-appointment',
  templateUrl: './appointment.component.html'
})
export class AppointmentComponent extends BaseComponent {

  requestedAppointmentInfo: RequestedData<Appointment>;
  appointmentList = new Array<Appointment>();
  displayedColumns: string[] = ['categoryName', 'userName', 'patientName', 'clinicName' ,'date', 'state' ,'actions'];
  dataSource = new MatTableDataSource(this.appointmentList);
  rowsCount: number = 0;
  searchText: string;
  appointmentStateEnum = AppointmentStateEnum; 

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatTable, { static: false }) table: MatTable<any>;

  constructor(private appointmentService: AppointmentService,
    public dialog: MatDialog,
    public router: Router,
    public alertService: AlertService,
    public configService: ConfigService,
    protected cdref: ChangeDetectorRef,
    protected route: ActivatedRoute,
    protected title: Title,
    @Inject(MAT_DIALOG_DATA) public data: any) {
      super(cdref, route, title);
  }

  ngOnInit(){
    this.searchSub.pipe(debounceTime(300), distinctUntilChanged())
    .subscribe((filterValue: string) => {
      this.searchText = filterValue.trim().toLowerCase();
      this.getAllAppointments();
    });
  }

  ngAfterViewInit() {
    this.getAllAppointments();
  }

  private getAllAppointments() {
    this.requestedAppointmentInfo = new RequestedData<Appointment>();
    this.requestedAppointmentInfo.gridSettings = new GridSettings();

    if(hasValue(this.paginator)){
      this.requestedAppointmentInfo.gridSettings.pageIndex = this.paginator.pageIndex;
      this.requestedAppointmentInfo.gridSettings.pageSize = this.paginator.pageSize;
    }
    if(hasValue(this.searchText)){
      this.requestedAppointmentInfo.gridSettings.searchText = this.searchText;
    }

    this.appointmentService.getAllAppointments(this.requestedAppointmentInfo)
    .subscribe(
      res => this.getAllAppointmentsOnSuccess(res),
      err => this.getAllAppointmentsOnError(err)
    );
  }

  private getAllAppointmentsOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private getAllAppointmentsOnError(response: any): void {
    this.alertService.viewAlerts(response.error.alerts);
  }

  private fillGrid(response: any) {
    this.appointmentList = response.entityList as Appointment[];
    this.paginator.length = this.rowsCount = response.gridSettings.rowsCount;
    this.dataSource = new MatTableDataSource(this.appointmentList);
    this.table.renderRows();
  }


  editRecord(id: number) {
    this.router.navigate(['appointment/appointmentdetails', id]);
  }

  addNewRecord() {
    this.router.navigate(['appointment/appointmentdetails']);
  }

  deleteRecord(id: number, appointmentDetails: string) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '650px',
      data: { selectedDetails: appointmentDetails }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == 'ok') {
        this.confirmDelete(id);
      }
    });
  }

  confirmDelete(id: number) {
    this.requestedAppointmentInfo.entity = new Appointment;
    this.requestedAppointmentInfo.entity.id = id;

    this.appointmentService.deleteAppointment(this.requestedAppointmentInfo).subscribe(
      res => this.deleteAppointmentOnSuccess(res),
      err => this.deleteAppointmentOnError(err)
    );
  }

  private deleteAppointmentOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private deleteAppointmentOnError(response: any) {
    this.alertService.viewAlerts(response.error.alerts);
  }

  applyFilter(filterValue: string) {
    this.searchSub.next(filterValue)
  }
  applyDateFilter(event: any){
    this.searchText = formatDate(event.value,"dd/MM/yyy","en-US");
    this.getAllAppointments();
  }

  getServerData(event: any) {
    this.getAllAppointments();
  }

}
