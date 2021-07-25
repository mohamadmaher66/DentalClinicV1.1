import { AppointmentStateEnum } from "../../shared/enum/appointment-state.enum";

export class AppointmentFilter{
    public dateFrom = new  Date();
    public dateTo = new Date();
    public patientId: number = 0;
    public clinicId: Number = 0;
    public categoryId: number = 0;
    public userId: number = 0;
    public state: AppointmentStateEnum = AppointmentStateEnum.None;
}