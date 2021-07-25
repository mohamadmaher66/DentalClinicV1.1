import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { HttpService } from './shared/service/http-service';

@Component({
  selector: 'body',
  template: '<router-outlet><mat-spinner diameter="150" *ngIf="inProgress"></mat-spinner></router-outlet>',
  providers: [IconSetService],
})
export class AppComponent implements OnInit {

  inProgress: boolean = false;
  constructor(private httpService: HttpService,
    private router: Router,
    public iconSet: IconSetService
  ) {
    iconSet.icons = { ...freeSet };
  }

  ngOnInit() {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });

    this.httpService.inProgressEventEmitter.subscribe(
      res => this.inProgress = res
    );


  }
}
