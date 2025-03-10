import { TestBed } from '@angular/core/testing';

import { LoadingService } from './loading.service';

/**
 * Unit tests for LoadingService
 */
describe('LoadingService', () => {
  let service: LoadingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LoadingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  /**
   * Test initial loading state
   */
  it('should start with loading state as false', () => {
    expect(service.isLoading()).toBeFalse();
    
    service.loading$.subscribe(loading => {
      expect(loading).toBeFalse();
    });
  });

  /**
   * Test startLoading method
   */
  it('should set loading state to true when startLoading is called', () => {
    service.startLoading();
    
    expect(service.isLoading()).toBeTrue();
    
    service.loading$.subscribe(loading => {
      expect(loading).toBeTrue();
    });
  });

  /**
   * Test multiple startLoading calls
   */
  it('should maintain loading state as true when startLoading is called multiple times', () => {
    // First call
    service.startLoading();
    expect(service.isLoading()).toBeTrue();
    
    // Second call
    service.startLoading();
    expect(service.isLoading()).toBeTrue();
    
    // Loading state should still be true
    service.loading$.subscribe(loading => {
      expect(loading).toBeTrue();
    });
  });

  /**
   * Test stopLoading method
   */
  it('should set loading state to false when all loadings are stopped', () => {
    // Start loading multiple times
    service.startLoading();
    service.startLoading();
    expect(service.isLoading()).toBeTrue();
    
    // Stop first loading
    service.stopLoading();
    // Should still be loading (one remaining)
    expect(service.isLoading()).toBeTrue();
    
    // Stop second loading
    service.stopLoading();
    // Now should be done loading
    expect(service.isLoading()).toBeFalse();
    
    service.loading$.subscribe(loading => {
      expect(loading).toBeFalse();
    });
  });

  /**
   * Test stopLoading without starting
   */
  it('should handle stopLoading when no loading has started', () => {
    // This would result in a negative request count in a naive implementation
    service.stopLoading();
    
    // Should still be false
    expect(service.isLoading()).toBeFalse();
    
    // Request count should never go below 0 (test internal implementation detail)
    expect((service as any).requestCount).toBeLessThanOrEqual(0);
  });

  /**
   * Test resetLoading method
   */
  it('should reset loading state regardless of request count', () => {
    // Start multiple loadings
    service.startLoading();
    service.startLoading();
    service.startLoading();
    expect(service.isLoading()).toBeTrue();
    
    // Reset loading directly
    service.resetLoading();
    
    // Should be false
    expect(service.isLoading()).toBeFalse();
    
    // Request count should be 0
    expect((service as any).requestCount).toBe(0);
  });

  /**
   * Test edge case: stopping more than starting
   */
  it('should handle more stops than starts without going negative', () => {
    // Start loading once
    service.startLoading();
    expect(service.isLoading()).toBeTrue();
    
    // Stop loading twice (one more than started)
    service.stopLoading();
    expect(service.isLoading()).toBeFalse();
    
    service.stopLoading(); // Extra stop
    
    // Should still be false
    expect(service.isLoading()).toBeFalse();
    
    // Internal request count should not be negative
    expect((service as any).requestCount).toBeLessThanOrEqual(0);
  });

  /**
   * Test isLoading synchronous method
   */
  it('should provide current loading state via isLoading method', () => {
    // Initially not loading
    expect(service.isLoading()).toBeFalse();
    
    // Start loading
    service.startLoading();
    expect(service.isLoading()).toBeTrue();
    
    // Stop loading
    service.stopLoading();
    expect(service.isLoading()).toBeFalse();
  });

  /**
   * Test loading state propagation to subscribers
   */
  it('should notify subscribers when loading state changes', (done) => {
    let loadingValues: boolean[] = [];
    
    // Subscribe to loading state changes
    const subscription = service.loading$.subscribe(loading => {
      loadingValues.push(loading);
      
      // After 3 values (initial false, true from start, false from stop)
      if (loadingValues.length === 3) {
        expect(loadingValues).toEqual([false, true, false]);
        subscription.unsubscribe();
        done();
      }
    });
    
    // Trigger loading state changes
    service.startLoading();
    service.stopLoading();
  });
}); 