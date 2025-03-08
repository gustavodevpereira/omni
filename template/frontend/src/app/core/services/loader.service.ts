import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

/**
 * Service responsible for managing the application's loading state.
 * 
 * @description
 * This service provides centralized control over application loading indicators,
 * useful for showing spinners or progress bars during asynchronous operations.
 * 
 * The service maintains a counter to handle nested loading operations,
 * ensuring the loader is only hidden when all operations complete.
 * 
 * @example
 * // In a component that needs to show loading state:
 * ngOnInit() {
 *   this.loaderService.isLoading().subscribe(loading => {
 *     this.showSpinner = loading;
 *   });
 * }
 * 
 * // Before loading data:
 * this.loaderService.show();
 * 
 * // After loading completes:
 * this.loaderService.hide();
 */
@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  /** 
   * Counter to track nested loading operations
   * Prevents hiding the loader prematurely when multiple
   * concurrent operations are in progress.
   */
  private loadingCount = 0;
  
  /** BehaviorSubject to track and broadcast loading state changes */
  private loading$ = new BehaviorSubject<boolean>(false);

  /**
   * Shows the loading indicator.
   * 
   * Increments the loading counter and sets loading state to true.
   * Safe to call multiple times (for nested operations).
   */
  show(): void {
    this.loadingCount++;
    
    if (this.loadingCount > 0) {
      this.loading$.next(true);
    }
  }

  /**
   * Hides the loading indicator.
   * 
   * Decrements the loading counter and sets loading state to false
   * only when all loading operations are complete.
   */
  hide(): void {
    this.loadingCount--;
    
    if (this.loadingCount <= 0) {
      // Prevent negative counts if hide is called more times than show
      this.loadingCount = 0;
      this.loading$.next(false);
    }
  }

  /**
   * Returns an Observable of the loading state.
   * 
   * @returns Observable<boolean> that emits true when loading, false when not loading
   * 
   * @example
   * this.loaderService.isLoading().subscribe(loading => {
   *   this.showSpinner = loading;
   * });
   */
  isLoading(): Observable<boolean> {
    return this.loading$.asObservable();
  }

  /**
   * Forcibly resets the loading state to not loading.
   * 
   * Use this as an emergency reset if the loading state
   * gets stuck due to errors in the application flow.
   */
  reset(): void {
    this.loadingCount = 0;
    this.loading$.next(false);
  }
  
  /**
   * Returns the current loading state value.
   * 
   * @returns boolean indicating if the app is currently loading
   */
  get loading(): boolean {
    return this.loading$.getValue();
  }
} 