import { ToothPositionEnum } from "../../shared/enum/appointment-tooth.enum";

export class AppointmentTooth {
    public id: number;
    public toothNumber: number;
    public toothPosition: ToothPositionEnum = ToothPositionEnum.Upper;
    public appointmentId: number;
}