import { ChangeDetectorRef, Component, Inject, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Patient } from '../../../../core/models/patient.model';
import { PatientService } from '../../../../core/servcies/patient.service';
import { BaseComponent } from '../../../../shared/components/base-component/base-component';
import { DeleteDialogComponent } from '../../../../shared/components/delete-dialog/delete-dialog.component';
import { GenderEnum } from '../../../../shared/enum/gender.enum';
import { GridSettings } from '../../../../shared/models/grid-settings.entity';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { ConfigService } from '../../../../shared/service/config.service';
import { hasValue } from '../../../../shared/service/helper.service';
import { PatientDetailsComponent } from '../patient-details/patient-details.component';

@Component({
  selector: 'app-patient',
  templateUrl: './patient.component.html'
})
export class PatientComponent extends BaseComponent {

  requestedPatientInfo: RequestedData<Patient>;
  patientList = new Array<Patient>();
  displayedColumns: string[] = ['fullName' , 'age', 'gender', 'address', 'phone' ,'actions'];
  dataSource = new MatTableDataSource(this.patientList);
  rowsCount: number = 0;
  searchText: string;
  genderEnum = GenderEnum;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatTable, { static: false }) table: MatTable<any>;

  constructor(private patientService: PatientService,
    public dialog: MatDialog,
    public router: Router,
    public dialogRef: MatDialogRef<PatientDetailsComponent>,
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
      this.getAllPatients();
    });
  }
  
  ngAfterViewInit() {
    this.getAllPatients();
  }

  private getAllPatients() {
    this.requestedPatientInfo = new RequestedData<Patient>();
    this.requestedPatientInfo.gridSettings = new GridSettings();

    if(hasValue(this.paginator)){
      this.requestedPatientInfo.gridSettings.pageIndex = this.paginator.pageIndex;
      this.requestedPatientInfo.gridSettings.pageSize = this.paginator.pageSize;
    }
    if(hasValue(this.searchText)){
      this.requestedPatientInfo.gridSettings.searchText = this.searchText;
    }

    this.patientService.getAllPatients(this.requestedPatientInfo).subscribe(
      res => this.getAllPatientsOnSuccess(res),
      err => this.getAllPatientsOnError(err)
    );
  }

  private getAllPatientsOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private getAllPatientsOnError(response: any): void {
    this.alertService.viewAlerts(response.error.alerts);
  }

  private fillGrid(response: any) {
    this.patientList = response.entityList as Patient[];
    this.paginator.length = this.rowsCount = response.gridSettings.rowsCount;
    this.dataSource = new MatTableDataSource(this.patientList);
    this.table.renderRows();
  }


  editRecord(id: number) {
    const dialogRef = this.dialog.open(PatientDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: id },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllPatients();
    });
  }

  addNewRecord() {
    const dialogRef = this.dialog.open(PatientDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: null },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllPatients();
    });
  }

  deleteRecord(id: number, patientDetails: string) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '650px',
      data: { selectedDetails: patientDetails }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == 'ok') {
        this.confirmDelete(id);
      }
    });
  }

  confirmDelete(id: number) {
    this.requestedPatientInfo.entity = new Patient;
    this.requestedPatientInfo.entity.id = id;

    this.patientService.deletePatient(this.requestedPatientInfo).subscribe(
      res => this.deletePatientOnSuccess(res),
      err => this.deletePatientOnError(err)
    );
  }

  private deletePatientOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private deletePatientOnError(response: any) {
    this.alertService.viewAlerts(response.error.alerts);
  }

  applyFilter(filterValue: string) {
    this.searchSub.next(filterValue);
  }

  getServerData(event: any) {
    this.getAllPatients();
  }

}
