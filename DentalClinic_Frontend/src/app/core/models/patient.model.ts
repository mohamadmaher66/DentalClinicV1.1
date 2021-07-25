import { GenderEnum } from "../../shared/enum/gender.enum";
import { MedicalHistory } from "./medical-history.model";

export class Patient {
    public id: number;
    public fullName: string;
    public address: string;
    public phone: string;
    public gender: GenderEnum = GenderEnum.Male;
    public age: number; 
    public medicalHistoryList = new Array<MedicalHistory>();
}