import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/service/http-service';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  url: string = "api/user/";
  users: User[];

  constructor(private httpService: HttpService) { }

  public getAllUsers(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllUsers');
  }

  public getAllUserLite(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetAllUserLite');
  }

  public getUser(object: any) {
    return this.httpService.httpPost(object, this.url + 'GetUser');
  }

  public addUser(object: any) {
    return this.httpService.httpPost(object, this.url + 'AddUser');
  }

  public editUser(object: any) {
    return this.httpService.httpPost(object, this.url + 'EditUser');
  }
  public deleteUser(object: any) {
    return this.httpService.httpPost(object, this.url + 'DeleteUser');
  }
  public login(object: any) {
    return this.httpService.httpPost(object, this.url + 'login');
  }

}
