import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../../../../core/models/user.model';
import { UserService } from '../../../../core/servcies/user.service';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { hasValue } from '../../../../shared/service/helper.service';
import { SessionService } from '../../../../shared/service/session.service';

@Component({
  selector: 'app-login',
  templateUrl: 'login.component.html'
})
export class LoginComponent { 
  user = new User();
  invalidLogin: boolean = false;
  
  constructor(private userService: UserService,
              private sessionService: SessionService,
              private router: Router,
              private alertService: AlertService) {
  }

  public login(form: NgForm){
    if(form.invalid){
      return;
    }
    let requestLoginData = new RequestedData<User>();
    requestLoginData.entity = this.user;

    this.userService.login(requestLoginData).subscribe(
      res => this.loginOnSuccess(res),
      err => this.loginOnError(err)
    );
  }
  loginOnSuccess(res: any): void {
    if(hasValue(res.entity.token)){
      this.sessionService.currentUser = res.entity;
      this.sessionService.setToken(res.entity.token);
      this.sessionService.setUserRole(res.entity.role);
      this.sessionService.setUserName(res.entity.fullName);
      this.router.navigateByUrl('/appointment/dashboard');
    }
    else{
      this.invalidLogin = true;
    }
  }
  loginOnError(res: any): void {
    this.alertService.viewAlerts(res.error.alerts);
  }
}
