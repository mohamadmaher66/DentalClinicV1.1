import { ChangeDetectorRef, Component, Inject, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { AppointmentAddition } from '../../../../core/models/appointment-addition.model';
import { AppointmentAdditionService } from '../../../../core/servcies/appointment-addition.service';
import { BaseComponent } from '../../../../shared/components/base-component/base-component';
import { DeleteDialogComponent } from '../../../../shared/components/delete-dialog/delete-dialog.component';
import { GridSettings } from '../../../../shared/models/grid-settings.entity';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { ConfigService } from '../../../../shared/service/config.service';
import { hasValue } from '../../../../shared/service/helper.service';
import { AppointmentAdditionDetailsComponent } from '../appointment-addition-details/appointment-addition-details.component';

@Component({
  selector: 'app-appointment-addition',
  templateUrl: './appointment-addition.component.html'
})
export class AppointmentAdditionComponent extends BaseComponent {

  requestedAppointmentAdditionInfo: RequestedData<AppointmentAddition>;
  appointmentAdditionList = new Array<AppointmentAddition>();
  displayedColumns: string[] = ['name', 'price', 'description', 'actions'];
  dataSource = new MatTableDataSource(this.appointmentAdditionList);
  rowsCount: number = 0;
  searchText: string;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatTable, { static: false }) table: MatTable<any>;

  constructor(private appointmentAdditionService: AppointmentAdditionService,
    public dialog: MatDialog,
    public router: Router,
    public dialogRef: MatDialogRef<AppointmentAdditionDetailsComponent>,
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
      this.getAllAppointmentAdditions();
    });
  }
  
  ngAfterViewInit() {
    this.getAllAppointmentAdditions();
  }

  private getAllAppointmentAdditions() {
    this.requestedAppointmentAdditionInfo = new RequestedData<AppointmentAddition>();
    this.requestedAppointmentAdditionInfo.gridSettings = new GridSettings();

    if(hasValue(this.paginator)){
      this.requestedAppointmentAdditionInfo.gridSettings.pageIndex = this.paginator.pageIndex;
      this.requestedAppointmentAdditionInfo.gridSettings.pageSize = this.paginator.pageSize;
    }
    if(hasValue(this.searchText)){
      this.requestedAppointmentAdditionInfo.gridSettings.searchText = this.searchText;
    }

    this.appointmentAdditionService.getAllAppointmentAdditions(this.requestedAppointmentAdditionInfo).subscribe(
      res => this.getAllAppointmentAdditionsOnSuccess(res),
      err => this.getAllAppointmentAdditionsOnError(err)
    );
  }

  private getAllAppointmentAdditionsOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private getAllAppointmentAdditionsOnError(response: any): void {
    this.alertService.viewAlerts(response.error.alerts);
  }

  private fillGrid(response: any) {
    this.appointmentAdditionList = response.entityList as AppointmentAddition[];
    this.paginator.length = this.rowsCount = response.gridSettings.rowsCount;
    this.dataSource = new MatTableDataSource(this.appointmentAdditionList);
    this.table.renderRows();
  }


  editRecord(id: number) {
    const dialogRef = this.dialog.open(AppointmentAdditionDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: id },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllAppointmentAdditions();
    });
  }

  addNewRecord() {
    const dialogRef = this.dialog.open(AppointmentAdditionDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: null },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllAppointmentAdditions();
    });
  }

  deleteRecord(id: number, appointmentAdditionDetails: string) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '650px',
      data: { selectedDetails: appointmentAdditionDetails }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == 'ok') {
        this.confirmDelete(id);
      }
    });
  }

  confirmDelete(id: number) {
    this.requestedAppointmentAdditionInfo.entity = new AppointmentAddition;
    this.requestedAppointmentAdditionInfo.entity.id = id;

    this.appointmentAdditionService.deleteAppointmentAddition(this.requestedAppointmentAdditionInfo).subscribe(
      res => this.deleteAppointmentAdditionOnSuccess(res),
      err => this.deleteAppointmentAdditionOnError(err)
    );
  }

  private deleteAppointmentAdditionOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private deleteAppointmentAdditionOnError(response: any) {
    this.alertService.viewAlerts(response.error.alerts);
  }

  applyFilter(filterValue: string) {
    this.searchSub.next(filterValue)
  }

  getServerData(event: any) {
    this.getAllAppointmentAdditions();
  }

}
