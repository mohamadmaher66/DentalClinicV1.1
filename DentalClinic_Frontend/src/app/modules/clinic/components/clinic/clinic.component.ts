import { ChangeDetectorRef, Component, Inject, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Clinic } from '../../../../core/models/clinic.model';
import { ClinicService } from '../../../../core/servcies/clinic.service';
import { BaseComponent } from '../../../../shared/components/base-component/base-component';
import { DeleteDialogComponent } from '../../../../shared/components/delete-dialog/delete-dialog.component';
import { GridSettings } from '../../../../shared/models/grid-settings.entity';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { ConfigService } from '../../../../shared/service/config.service';
import { hasValue } from '../../../../shared/service/helper.service';
import { ClinicDetailsComponent } from '../clinic-details/clinic-details.component';

@Component({
  selector: 'app-clinic',
  templateUrl: './clinic.component.html'
})
export class ClinicComponent extends BaseComponent {

  requestedClinicInfo: RequestedData<Clinic>;
  clinicList = new Array<Clinic>();
  displayedColumns: string[] = ['name', 'address', 'actions'];
  dataSource = new MatTableDataSource(this.clinicList);
  rowsCount: number = 0;
  searchText: string;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatTable, { static: false }) table: MatTable<any>;

  constructor(private clinicService: ClinicService,
    public dialog: MatDialog,
    public router: Router,
    public dialogRef: MatDialogRef<ClinicDetailsComponent>,
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
      this.getAllClinics();
    });
  }

  ngAfterViewInit() {
    this.getAllClinics();
  }

  private getAllClinics() {
    this.requestedClinicInfo = new RequestedData<Clinic>();
    this.requestedClinicInfo.gridSettings = new GridSettings();

    if(hasValue(this.paginator)){
      this.requestedClinicInfo.gridSettings.pageIndex = this.paginator.pageIndex;
      this.requestedClinicInfo.gridSettings.pageSize = this.paginator.pageSize;
    }
    if(hasValue(this.searchText)){
      this.requestedClinicInfo.gridSettings.searchText = this.searchText;
    }

    this.clinicService.getAllClinics(this.requestedClinicInfo).subscribe(
      res => this.getAllClinicsOnSuccess(res),
      err => this.getAllClinicsOnError(err)
    );
  }

  private getAllClinicsOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private getAllClinicsOnError(response: any): void {
    this.alertService.viewAlerts(response.error.alerts);
  }

  private fillGrid(response: any) {
    this.clinicList = response.entityList as Clinic[];
    this.paginator.length = this.rowsCount = response.gridSettings.rowsCount;
    this.dataSource = new MatTableDataSource(this.clinicList);
    this.table.renderRows();
  }


  editRecord(id: number) {
    const dialogRef = this.dialog.open(ClinicDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: id },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllClinics();
    });
  }

  addNewRecord() {
    const dialogRef = this.dialog.open(ClinicDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: null },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllClinics();
    });
  }

  deleteRecord(id: number, clinicDetails: string) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '650px',
      data: { selectedDetails: clinicDetails }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == 'ok') {
        this.confirmDelete(id);
      }
    });
  }

  confirmDelete(id: number) {
    this.requestedClinicInfo.entity = new Clinic;
    this.requestedClinicInfo.entity.id = id;

    this.clinicService.deleteClinic(this.requestedClinicInfo).subscribe(
      res => this.deleteClinicOnSuccess(res),
      err => this.deleteClinicOnError(err)
    );
  }

  private deleteClinicOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private deleteClinicOnError(response: any) {
    this.alertService.viewAlerts(response.error.alerts);
  }

  applyFilter(filterValue: string) {
    this.searchSub.next(filterValue)
  }

  getServerData(event: any) {
    this.getAllClinics();
  }

}
