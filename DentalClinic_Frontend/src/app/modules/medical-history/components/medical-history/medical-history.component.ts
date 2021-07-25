import { ChangeDetectorRef, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { MedicalHistory } from '../../../../core/models/medical-history.model';
import { MedicalHistoryService } from '../../../../core/servcies/medical-history.service';
import { BaseComponent } from '../../../../shared/components/base-component/base-component';
import { DeleteDialogComponent } from '../../../../shared/components/delete-dialog/delete-dialog.component';
import { GridSettings } from '../../../../shared/models/grid-settings.entity';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { ConfigService } from '../../../../shared/service/config.service';
import { hasValue } from '../../../../shared/service/helper.service';
import { MedicalHistoryDetailsComponent } from '../medical-history-details/medical-history-details.component';

@Component({
  selector: 'app-medical-history',
  templateUrl: './medical-history.component.html'
})
export class MedicalHistoryComponent extends BaseComponent {

  requestedMedicalHistoryInfo: RequestedData<MedicalHistory>;
  medicalHistoryList = new Array<MedicalHistory>();
  displayedColumns: string[] = ['Name', 'Description', 'actions'];
  dataSource = new MatTableDataSource(this.medicalHistoryList);
  rowsCount: number = 0;
  searchText: string;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatTable, { static: false }) table: MatTable<any>;

  constructor(private medicalHistoryService: MedicalHistoryService,
    public dialog: MatDialog,
    public router: Router,
    public dialogRef: MatDialogRef<MedicalHistoryDetailsComponent>,
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
      this.getAllMedicalHistorys();
    });
  }
  
  ngAfterViewInit() {
    this.getAllMedicalHistorys();
  }

  private getAllMedicalHistorys() {
    this.requestedMedicalHistoryInfo = new RequestedData<MedicalHistory>();
    this.requestedMedicalHistoryInfo.gridSettings = new GridSettings();

    if(hasValue(this.paginator)){
      this.requestedMedicalHistoryInfo.gridSettings.pageIndex = this.paginator.pageIndex;
      this.requestedMedicalHistoryInfo.gridSettings.pageSize = this.paginator.pageSize;
    }
    if(hasValue(this.searchText)){
      this.requestedMedicalHistoryInfo.gridSettings.searchText = this.searchText;
    }

    this.medicalHistoryService.getAllMedicalHistorys(this.requestedMedicalHistoryInfo).subscribe(
      res => this.getAllMedicalHistorysOnSuccess(res),
      err => this.getAllMedicalHistorysOnError(err)
    );
  }

  private getAllMedicalHistorysOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private getAllMedicalHistorysOnError(response: any): void {
    this.alertService.viewAlerts(response.error.alerts);
  }

  private fillGrid(response: any) {
    this.medicalHistoryList = response.entityList as MedicalHistory[];
    this.paginator.length = this.rowsCount = response.gridSettings.rowsCount;
    this.dataSource = new MatTableDataSource(this.medicalHistoryList);
    this.table.renderRows();
  }


  editRecord(id: number) {
    const dialogRef = this.dialog.open(MedicalHistoryDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: id },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllMedicalHistorys();
    });
  }

  addNewRecord() {
    const dialogRef = this.dialog.open(MedicalHistoryDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: null },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllMedicalHistorys();
    });
  }

  deleteRecord(id: number, medicalHistoryDetails: string) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '650px',
      data: { selectedDetails: medicalHistoryDetails }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == 'ok') {
        this.confirmDelete(id);
      }
    });
  }

  confirmDelete(id: number) {
    this.requestedMedicalHistoryInfo.entity = new MedicalHistory;
    this.requestedMedicalHistoryInfo.entity.id = id;

    this.medicalHistoryService.deleteMedicalHistory(this.requestedMedicalHistoryInfo).subscribe(
      res => this.deleteMedicalHistoryOnSuccess(res),
      err => this.deleteMedicalHistoryOnError(err)
    );
  }

  private deleteMedicalHistoryOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private deleteMedicalHistoryOnError(response: any) {
    this.alertService.viewAlerts(response.error.alerts);
  }

  applyFilter(filterValue: string) {
    this.searchSub.next(filterValue);
  }

  getServerData(event: any) {
    this.getAllMedicalHistorys();
  }

}
