import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { RoleEnum } from '../enum/role.enum';
import { SessionService } from './session.service';

@Injectable({
  providedIn: 'root'
})
export class DoctorAuthGaurd implements CanActivate {

    constructor(private sessionService: SessionService,
                private router: Router) {
    }
    canActivate(): boolean{
        if(this.sessionService.getUserRole() == RoleEnum.Doctor){
            return true;
        }
        else{
            this.router.navigate(['login']);
            return false;
        }
    }
}