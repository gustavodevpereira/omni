import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

/**
 * Loading Service
 * 
 * Provides a centralized way to manage loading states for HTTP requests.
 * Uses RxJS to communicate loading state to components.
 */
@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  /**
   * Counter for active requests
   */
  private requestCount = 0;
  
  /**
   * Subject for the loading state
   */
  private loadingSubject = new BehaviorSubject<boolean>(false);
  
  /**
   * Observable for the loading state
   */
  public loading$: Observable<boolean> = this.loadingSubject.asObservable();

  constructor() { }

  /**
   * Starts the loading state by incrementing the request counter
   */
  startLoading(): void {
    this.requestCount++;
    if (this.requestCount === 1) {
      this.loadingSubject.next(true);
    }
  }

  /**
   * Stops the loading state by decrementing the request counter
   */
  stopLoading(): void {
    this.requestCount--;
    if (this.requestCount === 0) {
      this.loadingSubject.next(false);
    }
  }

  /**
   * Resets the loading state
   */
  resetLoading(): void {
    this.requestCount = 0;
    this.loadingSubject.next(false);
  }

  /**
   * Gets the current loading state synchronously
   * 
   * @returns True if the application is in a loading state
   */
  isLoading(): boolean {
    return this.loadingSubject.getValue();
  }
} 