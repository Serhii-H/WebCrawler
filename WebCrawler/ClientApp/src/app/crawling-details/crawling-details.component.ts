import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { CrawlingDetailsModel } from '../models/crawling-details.model';
import { CrawlingService } from '../services/crawling.service';

@Component({
  selector: 'app-crawling-details',
  templateUrl: './crawling-details.component.html',
  styleUrls: ['./crawling-details.component.css']
})
export class CrawlingDetailsComponent implements OnInit {
  crawlingDetails$: Observable<CrawlingDetailsModel[]>;
  expression: string;

  constructor(
    private crawlingService: CrawlingService,
    private router: Router,
    private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.crawlingDetails$ = this.route.paramMap
      .pipe(switchMap(params => {
        this.expression = params.get('expression');
        return this.crawlingService.getDetails(params.get('id'));
      }));
  }

  goToHomePage(): void {
    this.router.navigate(['/home']);
  }
}
