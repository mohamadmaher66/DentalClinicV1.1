import { ChangeDetectorRef, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-base-component',
  template: ""
})
export class BaseComponent  {
  searchSub = new Subject<string>();

   constructor(protected cdref: ChangeDetectorRef,
    protected route: ActivatedRoute,
    protected title: Title) {
  }

  ngAfterContentInit() {
    this.route.data.subscribe(data => this.title.setTitle(data.title));
  }
}
