import { Component, OnInit } from '@angular/core';
import { LoadingService } from '../../services/loading.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { MatProgressBarModule } from '@angular/material/progress-bar';

/**
 * Loading Component
 * 
 * Displays a loading progress bar at the top of the application when HTTP requests are in progress.
 * Uses the LoadingService to determine when to show/hide the progress bar.
 */
@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [CommonModule, MatProgressBarModule],
  template: `
    <div class="loading-bar-container" *ngIf="loading$ | async">
      <mat-progress-bar mode="indeterminate" color="accent"></mat-progress-bar>
    </div>
  `,
  styles: [`
    .loading-bar-container {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      z-index: 9999;
    }
  `]
})
export class LoadingComponent implements OnInit {
  /**
   * Observable of the loading state
   */
  loading$: Observable<boolean>;

  constructor(private loadingService: LoadingService) {
    this.loading$ = this.loadingService.loading$;
  }

  ngOnInit(): void {
    // Component initialization logic if needed
  }
} 