import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { User } from '../../../../core/models/user.model';
import { UserService } from '../../../../core/servcies/user.service';
import { RoleEnum } from '../../../../shared/enum/role.enum';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html'
})
export class UserDetailsComponent implements OnInit {
  //Variables
  public user: User = new User();
  requestUserData: RequestedData<User>;
  btnTitle: string = "حفظ";
  roleEnum = RoleEnum;

  constructor(private userService: UserService,
    private alertService: AlertService,
    public dialogRef: MatDialogRef<UserDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    }

  ngOnInit() {
    this.user.id = this.data.selectedDetails;
    if (this.user.id > 0) {
      this.btnTitle = 'حفظ';
      this.getUser();
    }
  }

  private getUser(){
    this.requestUserData = new RequestedData<User>();
    this.requestUserData.entity = new User;
    this.requestUserData.entity.id = this.user.id;

    this.userService.getUser(this.requestUserData).subscribe(
      res => this.getUserOnSuccess(res) ,        
      err => this.getUserOnError(err)
    );
  }

  private getUserOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.user = response.entity as User;
  }

  private getUserOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

  public submitUser(form:NgForm) {
    if(form.invalid){
      return;
    }
    this.requestUserData = new RequestedData<User>();
    this.requestUserData.entity = this.user;

    if (this.user.id > 0) {
      this.editUser();
    } 
    else {
      this.addUser();
    }
  }

  private addUser(){
    this.requestUserData.entity.id = 0;
    this.userService.addUser(this.requestUserData).subscribe(
      res => this.userActionOnSuccess(res),
      err => this.userActionOnError(err)
    );

  }

  private editUser(){
    this.userService.editUser(this.requestUserData).subscribe(
      res => this.userActionOnSuccess(res),
      err => this.userActionOnError(err)
    );
  }

  private userActionOnSuccess(response: any){
    this.alertService.viewAlerts(response.alerts);
    this.dialogRef.close();
  }

  private userActionOnError(response:any){
    this.alertService.viewAlerts(response.error.alerts);
  }

}
