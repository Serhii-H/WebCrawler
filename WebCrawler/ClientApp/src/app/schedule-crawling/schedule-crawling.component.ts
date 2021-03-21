import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CrawlingService } from '../services/crawling.service';

@Component({
  selector: 'app-schedule-crawling',
  templateUrl: './schedule-crawling.component.html',
  styleUrls: ['./schedule-crawling.component.css']
})
export class ScheduleCrawlingComponent {

  formGroup: FormGroup;
  isLoading = false;
  statusMessage: string;

  get expression() { return this.formGroup.get('expression'); }
  get url() { return this.formGroup.get('url'); }

  constructor(
    private router: Router,
    private crawlingService: CrawlingService) {
    const urlValidationRegex = /^[A-Za-z][A-Za-z\d.+-]*:\/*(?:\w+(?::\w+)?@)?[^\s/]+(?::\d+)?(?:\/[\w#!:.?+=&%@\-/]*)?$/;

    this.formGroup = new FormGroup({
      expression: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
      url: new FormControl('', [Validators.required, Validators.pattern(urlValidationRegex), Validators.maxLength(500)])
    });
  }

  scheduleCrawling(): void {
    if (this.formGroup.invalid) {
      this.displayValidationErrors();
      return;
    }

    this.isLoading = true;
    this.statusMessage = 'Scheduling... Please wait.';

    this.crawlingService.create({
      expression: this.expression.value,
      url: this.url.value
    }).subscribe(isSuccess => {
      if (isSuccess) {
        this.goToHomePage();
      }

      this.statusMessage = 'An error occured while scheduling crawling job. Please try again later.';
      this.isLoading = false;
    });
  }

  private displayValidationErrors(): void {
    Object.keys(this.formGroup.controls).forEach(controlName => {
      const control = this.formGroup.get(controlName);
      control.markAsTouched({ onlySelf: true });
    });
  }

  goToHomePage(): void {
    this.router.navigate(['/home']);
  }
}
