import { ChangeDetectorRef, Component, Inject, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { AppointmentCategory } from '../../../../core/models/appointment-category.model';
import { AppointmentCategoryService } from '../../../../core/servcies/appointment-category.service';
import { BaseComponent } from '../../../../shared/components/base-component/base-component';
import { DeleteDialogComponent } from '../../../../shared/components/delete-dialog/delete-dialog.component';
import { GridSettings } from '../../../../shared/models/grid-settings.entity';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { ConfigService } from '../../../../shared/service/config.service';
import { hasValue } from '../../../../shared/service/helper.service';
import { AppointmentCategoryDetailsComponent } from '../appointment-category-details/appointment-category-details.component';

@Component({
  selector: 'app-appointment-category',
  templateUrl: './appointment-category.component.html'
})
export class AppointmentCategoryComponent extends BaseComponent {

  requestedAppointmentCategoryInfo: RequestedData<AppointmentCategory>;
  appointmentCategoryList = new Array<AppointmentCategory>();
  displayedColumns: string[] = ['name', 'price', 'description', 'actions'];
  dataSource = new MatTableDataSource(this.appointmentCategoryList);
  rowsCount: number = 0;
  searchText: string;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatTable, { static: false }) table: MatTable<any>;

  constructor(private appointmentCategoryService: AppointmentCategoryService,
    public dialog: MatDialog,
    public router: Router,
    public dialogRef: MatDialogRef<AppointmentCategoryDetailsComponent>,
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
      this.getAllAppointmentCategorys();
    });
  }
  
  ngAfterViewInit() {
    this.getAllAppointmentCategorys();
  }

  private getAllAppointmentCategorys() {
    this.requestedAppointmentCategoryInfo = new RequestedData<AppointmentCategory>();
    this.requestedAppointmentCategoryInfo.gridSettings = new GridSettings();

    if(hasValue(this.paginator)){
      this.requestedAppointmentCategoryInfo.gridSettings.pageIndex = this.paginator.pageIndex;
      this.requestedAppointmentCategoryInfo.gridSettings.pageSize = this.paginator.pageSize;
    }
    if(hasValue(this.searchText)){
      this.requestedAppointmentCategoryInfo.gridSettings.searchText = this.searchText;
    }

    this.appointmentCategoryService.getAllAppointmentCategorys(this.requestedAppointmentCategoryInfo).subscribe(
      res => this.getAllAppointmentCategorysOnSuccess(res),
      err => this.getAllAppointmentCategorysOnError(err)
    );
  }

  private getAllAppointmentCategorysOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private getAllAppointmentCategorysOnError(response: any): void {
    this.alertService.viewAlerts(response.error.alerts);
  }

  private fillGrid(response: any) {
    this.appointmentCategoryList = response.entityList as AppointmentCategory[];
    this.paginator.length = this.rowsCount = response.gridSettings.rowsCount;
    this.dataSource = new MatTableDataSource(this.appointmentCategoryList);
    this.table.renderRows();
  }


  editRecord(id: number) {
    const dialogRef = this.dialog.open(AppointmentCategoryDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: id },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllAppointmentCategorys();
    });
  }

  addNewRecord() {
    const dialogRef = this.dialog.open(AppointmentCategoryDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: null },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllAppointmentCategorys();
    });
  }

  deleteRecord(id: number, appointmentCategoryDetails: string) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '650px',
      data: { selectedDetails: appointmentCategoryDetails }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == 'ok') {
        this.confirmDelete(id);
      }
    });
  }

  confirmDelete(id: number) {
    this.requestedAppointmentCategoryInfo.entity = new AppointmentCategory;
    this.requestedAppointmentCategoryInfo.entity.id = id;

    this.appointmentCategoryService.deleteAppointmentCategory(this.requestedAppointmentCategoryInfo).subscribe(
      res => this.deleteAppointmentCategoryOnSuccess(res),
      err => this.deleteAppointmentCategoryOnError(err)
    );
  }

  private deleteAppointmentCategoryOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private deleteAppointmentCategoryOnError(response: any) {
    this.alertService.viewAlerts(response.error.alerts);
  }

  applyFilter(filterValue: string) {
    this.searchSub.next(filterValue)
  }

  getServerData(event: any) {
    this.getAllAppointmentCategorys();
  }

}
