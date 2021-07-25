import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './components/user/user.component';
import { SharedModule } from '../../shared/shared.module';
import { UserDetailsComponent } from './components/user-details/user-details.component';

@NgModule({
  declarations: [UserComponent, UserDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    UserRoutingModule
  ]
})
export class UserModule { }
