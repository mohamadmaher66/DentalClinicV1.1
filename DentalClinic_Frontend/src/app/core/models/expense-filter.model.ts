import { ExpenseTypeEnum } from "../../shared/enum/expense-type.enum";

export class ExpenseFilter {
    public dateFrom = new  Date();
    public dateTo = new Date();
    public clinicId: Number = 0;
    public clinicName: string;
    public type: ExpenseTypeEnum = ExpenseTypeEnum.None;
    public typeName: string;
    public userId: number = 0;
    public UserFullName: string;
}