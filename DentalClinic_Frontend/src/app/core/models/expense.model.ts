import { ExpenseTypeEnum } from "../../shared/enum/expense-type.enum";

export class Expense {
    public id: number;
    public name: string;
    public cost: number;
    public actionDate: Date = new Date();
    public description: string;
    public type: ExpenseTypeEnum; 
    public clinicId: number;
    public clinicName: string;
}