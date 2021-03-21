import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { CrawlingDetailsModel } from '../models/crawling-details.model';
import { CrawlingModel } from '../models/crawling.model';
import { CreateCrawlingRequest } from '../models/create-crawling-request';

@Injectable({
  providedIn: 'root'
})
export class CrawlingService {

  private crawlingUrl = "/api/Crawling";

  constructor(private httpClient: HttpClient) { }

  list(): Observable<CrawlingModel[]> {
    return this.httpClient.get<CrawlingModel[]>(this.crawlingUrl);
  }

  create(request: CreateCrawlingRequest): Observable<boolean> {
    return this.httpClient.post(this.crawlingUrl, request, { observe: 'response' }).pipe(map(response => response.ok), catchError(e => of(false)));
  }

  getDetails(crawlingId: string): Observable<CrawlingDetailsModel[]> {
    return this.httpClient.get<CrawlingDetailsModel[]>(`/api/CrawlingDetails/${crawlingId}`);
  }
}
