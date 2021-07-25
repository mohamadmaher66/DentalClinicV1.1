import { ChangeDetectorRef, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { User } from '../../../../core/models/user.model';
import { UserService } from '../../../../core/servcies/user.service';
import { BaseComponent } from '../../../../shared/components/base-component/base-component';
import { DeleteDialogComponent } from '../../../../shared/components/delete-dialog/delete-dialog.component';
import { RoleEnum } from '../../../../shared/enum/role.enum';
import { GridSettings } from '../../../../shared/models/grid-settings.entity';
import { RequestedData } from '../../../../shared/models/request-data.entity';
import { AlertService } from '../../../../shared/service/alert.service';
import { ConfigService } from '../../../../shared/service/config.service';
import { hasValue } from '../../../../shared/service/helper.service';
import { UserDetailsComponent } from '../user-details/user-details.component';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html'
})
export class UserComponent extends BaseComponent {

  requestedUserInfo: RequestedData<User>;
  userList = new Array<User>();
  displayedColumns: string[] = ['fullName', 'username', 'address', 'phone', 'role', 'isActive', 'actions'];
  dataSource = new MatTableDataSource(this.userList);
  rowsCount: number = 0;
  searchText: string;
  roleEnum = RoleEnum;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatTable, { static: false }) table: MatTable<any>;

  constructor(private userService: UserService,
    public dialog: MatDialog,
    public router: Router,
    public dialogRef: MatDialogRef<UserDetailsComponent>,
    public alertService: AlertService,
    public configService: ConfigService,
    protected cdref: ChangeDetectorRef,
    protected route: ActivatedRoute,
    protected title: Title,
    @Inject(MAT_DIALOG_DATA) public data: any) {
      super(cdref, route, title);
  }

  ngOnInit(){
    this.searchSub.pipe(debounceTime(300), distinctUntilChanged())
    .subscribe((filterValue: string) => {
      this.searchText = filterValue.trim().toLowerCase();
      this.getAllUsers();
    });
  }
  
  ngAfterViewInit() {
    this.getAllUsers();
  }

  private getAllUsers() {
    this.requestedUserInfo = new RequestedData<User>();
    this.requestedUserInfo.gridSettings = new GridSettings();

    if(hasValue(this.paginator)){
      this.requestedUserInfo.gridSettings.pageIndex = this.paginator.pageIndex;
      this.requestedUserInfo.gridSettings.pageSize = this.paginator.pageSize;
    }
    if(hasValue(this.searchText)){
      this.requestedUserInfo.gridSettings.searchText = this.searchText;
    }

    this.userService.getAllUsers(this.requestedUserInfo).subscribe(
      res => this.getAllUsersOnSuccess(res),
      err => this.getAllUsersOnError(err)
    );
  }

  private getAllUsersOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private getAllUsersOnError(response: any): void {
    this.alertService.viewAlerts(response.error.alerts);
  }

  private fillGrid(response: any) {
    this.userList = response.entityList as User[];
    this.paginator.length = this.rowsCount = response.gridSettings.rowsCount;
    this.dataSource = new MatTableDataSource(this.userList);
    this.table.renderRows();
  }


  editRecord(id: number) {
    const dialogRef = this.dialog.open(UserDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: id },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllUsers();
    });
  }

  addNewRecord() {
    const dialogRef = this.dialog.open(UserDetailsComponent, {
      width: '1000px',
      data: { selectedDetails: null },
      panelClass: 'details-panel'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllUsers();
    });
  }

  deleteRecord(id: number, userDetails: string) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '650px',
      data: { selectedDetails: userDetails }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == 'ok') {
        this.confirmDelete(id);
      }
    });
  }

  confirmDelete(id: number) {
    this.requestedUserInfo.entity = new User;
    this.requestedUserInfo.entity.id = id;

    this.userService.deleteUser(this.requestedUserInfo).subscribe(
      res => this.deleteUserOnSuccess(res),
      err => this.deleteUserOnError(err)
    );
  }

  private deleteUserOnSuccess(response: any) {
    this.alertService.viewAlerts(response.alerts);
    this.fillGrid(response);
  }

  private deleteUserOnError(response: any) {
    this.alertService.viewAlerts(response.error.alerts);
  }

  applyFilter(filterValue: string) {
    this.searchSub.next(filterValue);
  }

  getServerData(event: any) {
    this.getAllUsers();
  }

}
