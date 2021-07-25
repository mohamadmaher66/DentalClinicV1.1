import { RoleEnum } from "../../shared/enum/role.enum";

export class User{
    public id: number;
    public username: string;
    public password: string;
    public fullName: string;
    public address: string;
    public phone: string;
    public role: RoleEnum;
    public isActive: boolean;
    public token: string;
}