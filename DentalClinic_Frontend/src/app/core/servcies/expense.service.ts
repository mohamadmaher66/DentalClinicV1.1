import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/service/http-service';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {

  url: string = "api/Expense/";

  constructor(private httpService: HttpService) { }

  public getAllExpenses(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllExpenses');
  }

  public getExpense(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetExpense');
  }

  public addExpense(object: any) {
    return this.httpService.httpPost(object, this.url + 'AddExpense');
  }

  public editExpense(object: any) {
    return this.httpService.httpPost(object, this.url + 'EditExpense');
  }
  public deleteExpense(object: any) {
    return this.httpService.httpPost(object, this.url + 'DeleteExpense');
  }
  public getExpenseDetailsLists(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetExpenseDetailsLists');
  }
  

}
