import { ChangeDetectorRef, Component, Inject, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Expense } from '../../../../core/models/expense.model';
import { ExpenseService } from '../../../../core/servcies/expense.service';
import { BaseComponent } from '../../../../shared/components/base-component/base-component';
import { DeleteDialogComponent } from '../../../../shared/components/delete-dialog/delete-dialog.component';
import { ExpenseTypeEnum } from '../../../../shared/enum/expense-type.enum';
import { GridSettings } from '../../../../shared/models/grid-settings.entity';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { ConfigService } from '../../../../shared/service/config.service';
import { hasValue } from '../../../../shared/service/helper.service';
import { ExpenseDetailsComponent } from '../expense-details/expense-details.component';

@Component({
  selector: 'app-expense',
  templateUrl: './expense.component.html'
})
export class ExpenseComponent extends BaseComponent {

  requestedExpenseInfo: RequestedData<Expense>;
  expenseList = new Array<Expense>();
  displayedColumns: string[] = ['name', 'cost', 'actionDate', 'type', 'clinicName' ,'actions'];
  dataSource = new MatTableDataSource(this.expenseList);
  rowsCount: number = 0;
  searchText: string;
  expenseTypeEnum = ExpenseTypeEnum;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatTable, { static: false }) table: MatTable<any>;

  constructor(private expenseService: ExpenseService,
    public dialog: MatDialog,
    public router: Router,
    public dialogRef: MatDialogRef<ExpenseDetailsComponent>,
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
      this.getAllExpenses();
    });
  }
  
  ngAfterViewInit() {
    this.getAllExpenses();
  }

  private getAllExpenses() {
    this.requestedExpenseInfo = new RequestedData<Expense>();
    this.requestedExpenseInfo.gridSettings = new GridSettings();

    if(hasValue(this.paginator)){
      this.requestedExpenseInfo.gridSettings.pageIndex = this.paginator.pageIndex;
      this.requestedExpenseInfo.gridSettings.pageSize = this.paginator.pageSize;
    }
    if(hasValue(this.searchText)){
      this.requestedExpenseInfo.gridSettings.searchText = this.searchText;
    }

    this.expenseService.getAllExpenses(this.requestedExpenseInfo).subscribe(
      res => this.getAllExpensesOnSuccess(res),
      err => this.getAllExpensesOnError(err)
    );
  }

  private getAllExpensesOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private getAllExpensesOnError(response: any): void {
    this.alertService.viewAlerts(response.error.alerts);
  }

  private fillGrid(response: any) {
    this.expenseList = response.entityList as Expense[];
    this.paginator.length = this.rowsCount = response.gridSettings.rowsCount;
    this.dataSource = new MatTableDataSource(this.expenseList);
    this.table.renderRows();
  }


  editRecord(id: number) {
    const dialogRef = this.dialog.open(ExpenseDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: id },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllExpenses();
    });
  }

  addNewRecord() {
    const dialogRef = this.dialog.open(ExpenseDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: null },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllExpenses();
    });
  }

  deleteRecord(id: number, expenseDetails: string) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '650px',
      data: { selectedDetails: expenseDetails }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == 'ok') {
        this.confirmDelete(id);
      }
    });
  }

  confirmDelete(id: number) {
    this.requestedExpenseInfo.entity = new Expense;
    this.requestedExpenseInfo.entity.id = id;

    this.expenseService.deleteExpense(this.requestedExpenseInfo).subscribe(
      res => this.deleteExpenseOnSuccess(res),
      err => this.deleteExpenseOnError(err)
    );
  }

  private deleteExpenseOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private deleteExpenseOnError(response: any) {
    this.alertService.viewAlerts(response.error.alerts);
  }

  applyFilter(filterValue: string) {
    this.searchSub.next(filterValue);
  }

  getServerData(event: any) {
    this.getAllExpenses();
  }

}
