import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { CrawlingModel } from '../models/crawling.model';
import { CrawlingService } from '../services/crawling.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  crawlings$: Observable<CrawlingModel[]>;

  private triggerDataReloadSubject = new BehaviorSubject<any>(1);

  constructor(
    private crawlingService: CrawlingService,
    private router: Router) {
  }

  ngOnInit(): void {
    this.crawlings$ = this.triggerDataReloadSubject
      .pipe(switchMap(() => this.crawlingService.list()));
  }

  openDetails(crawlingId: string, expression: string): void {
    this.router.navigate(['/details', { id: crawlingId, expression: expression }]);
  }

  refresh(): void {
    this.triggerDataReloadSubject.next(1);
  }
}
