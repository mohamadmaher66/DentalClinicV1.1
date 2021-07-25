import { AppointmentStateEnum } from "../../shared/enum/appointment-state.enum";
import { AppointmentAddition } from "./appointment-addition.model";
import { AppointmentCategory } from "./appointment-category.model";
import { AppointmentTooth } from "./appointment-tooth.model";
import { Attachment } from "./attachment.model";
import { Clinic } from "./clinic.model";
import { Patient } from "./patient.model";
import { User } from "./user.model";

export class Appointment {
    public id: number;
    public category = new AppointmentCategory();
    public user = new User();
    public patient = new Patient();
    public clinic = new Clinic();
    public date: Date = new Date();
    public totalPrice: number = 0;
    public discountPercentage: number = 0;
    public paidAmount: number = 0;
    public state: AppointmentStateEnum = AppointmentStateEnum.Pending;
    public notes: string;
    public attachmentList = new Array<Attachment>();
    public toothList = new Array<AppointmentTooth>();
    public appointmentAdditionList = new Array<AppointmentAddition>();
}