import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Clinic } from '../../../../core/models/clinic.model';
import { Expense } from '../../../../core/models/expense.model';
import { ExpenseService } from '../../../../core/servcies/expense.service';
import { DetailsListEnum } from '../../../../shared/enum/details-list.enum';
import { ExpenseTypeEnum } from '../../../../shared/enum/expense-type.enum';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { listHasValue } from '../../../../shared/service/helper.service';

@Component({
  selector: 'app-Expense-details',
  templateUrl: './Expense-details.component.html'
})
export class ExpenseDetailsComponent implements OnInit {
  //Variables
  public expense: Expense = new Expense();
  requestExpenseData: RequestedData<Expense>;
  btnTitle: string = "حفظ";
  expenseTypeEnum = ExpenseTypeEnum;
  clinicList = new Array<Clinic>();
  filteredClinicList = new Array<Clinic>();

  constructor(private ExpenseService: ExpenseService,
    private alertService: AlertService,
    public dialogRef: MatDialogRef<ExpenseDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    }

  ngOnInit() {
    this.expense.id = this.data.selectedDetails;
    if (this.expense.id > 0) {
      this.btnTitle = 'حفظ';
      this.getExpense();
    }
    else{
      this.getDetailsLists();
    }
  }

  private getExpense(){
    this.requestExpenseData = new RequestedData<Expense>();
    this.requestExpenseData.entity = new Expense;
    this.requestExpenseData.entity.id = this.expense.id;

    this.ExpenseService.getExpense(this.requestExpenseData).subscribe(
      res => this.getExpenseOnSuccess(res) ,        
      err => this.getExpenseOnError(err)
    );
  }

  private getExpenseOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.expense = response.entity as Expense;
    this.setDetailsLists(response);
  }

  private getExpenseOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  public submitExpense(form:NgForm) {
    if(form.invalid){
      return;
    }
    this.requestExpenseData = new RequestedData<Expense>();
    this.requestExpenseData.entity = this.expense;
     
    if (this.expense.id > 0) {
      this.editExpense();
    } 
    else {
      this.addExpense();
    }
  }

  private addExpense(){
    this.requestExpenseData.entity.id = 0;
    this.ExpenseService.addExpense(this.requestExpenseData).subscribe(
      res => this.ExpenseActionOnSuccess(res),
      err => this.ExpenseActionOnError(err)
    );
  }

  private editExpense(){
    this.ExpenseService.editExpense(this.requestExpenseData).subscribe(
      res => this.ExpenseActionOnSuccess(res),
      err => this.ExpenseActionOnError(err)
    );
  }

  private ExpenseActionOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.dialogRef.close();
  }

  private ExpenseActionOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  private getDetailsLists(){
    this.requestExpenseData = new RequestedData<Expense>();

    this.ExpenseService.getExpenseDetailsLists(this.requestExpenseData).subscribe(
      res => this.getExpenseDetailsListsOnSuccess(res) ,        
      err => this.getExpenseDetailsListsOnError(err)
    );
  }

  private getExpenseDetailsListsOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.setDetailsLists(response);
  }

  private getExpenseDetailsListsOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  public filterClinic(value: string){
    let filter = value.toLowerCase();
    this.filteredClinicList = this.clinicList
    .filter(option => option.name.toLowerCase().startsWith(filter));
  }

  private setDetailsLists(response: RequestedData<any>){
    if (listHasValue(response.detailsList)) { }
      response.detailsList.forEach(list => {
        switch (list.detailsListId) {
          case DetailsListEnum.Clinic:
            this.clinicList = list.list;
            this.filteredClinicList = this.clinicList;
        }
      });
  }

}
