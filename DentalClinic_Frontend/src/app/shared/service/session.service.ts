import { Injectable, Inject, EventEmitter } from '@angular/core';
import { isNullOrEmptyString, hasValue } from './helper.service';
import { Router } from '@angular/router';
import { User } from '../../core/models/user.model';
import { RoleEnum } from '../enum/role.enum';

@Injectable()
export class SessionService {

    private token: string;
    private tokenKey: string = "c__tk";
    private roleKey: string = "c__rl";
    private nameKey: string = "c__nm";
    public currentUser = new User();

    constructor(private router: Router) {
    }

    public setToken(token: string) {
        this.setValue(this.tokenKey, token);
    }
    public setUserRole(role: number) {
        this.setValue(this.roleKey, role);
    }
    public setUserName(name: string) {
        name =  name.split(' ')[0];
        if(this.getUserRole() == RoleEnum.Doctor){
            name = "دكتور " + name;
        }
        this.setValue(this.nameKey, name);
    }

    public getUserRole(): number {
        return Number(this.getValue(this.roleKey));
    }
    public getToken(): string {
        return this.getValue(this.tokenKey);
    }
    public getUserName(): string {
        return this.getValue(this.nameKey);
    }

    private setValue(key: string, value: any) {
        localStorage.setItem(key, value);
    }

    private getValue(key: string) {
        if (isNullOrEmptyString(localStorage.getItem(key))) {
            this.router.navigate(['login']);
            return null;
        }
        switch (key) {
            case this.tokenKey:
                if (isNullOrEmptyString(this.token)) {
                    this.token = localStorage.getItem(key);
                }
                return 'Bearer ' + this.token;
            default :
                return localStorage.getItem(key);
        }
    }

    public clear() {
        localStorage.removeItem(this.tokenKey);
        localStorage.removeItem(this.roleKey);
        localStorage.removeItem(this.nameKey);
        this.token = "";
    }

    public signOutWithErrorMessage(errMessage) {
        this.clear();
        if (hasValue(errMessage)) {
            this.router.navigate(['/login', { errormessage: errMessage }]);
        } else {
            this.router.navigateByUrl("/login");
        }
    }


}