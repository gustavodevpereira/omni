import { TestBed } from '@angular/core/testing';
import { LoaderService } from './loader.service';

/**
 * Test suite for LoaderService
 * 
 * @description
 * Tests the loader service that manages application-wide loading state.
 * This suite verifies that:
 * - Loading state is correctly tracked and broadcast
 * - Nested loading operations work correctly
 * - State is properly reset when needed
 */
describe('LoaderService', () => {
  let service: LoaderService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LoaderService]
    });
    
    service = TestBed.inject(LoaderService);
  });

  /**
   * Test service creation
   */
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  /**
   * Test initial loading state
   */
  it('should initialize with loading state as false', (done) => {
    service.isLoading().subscribe(loading => {
      expect(loading).withContext('Initial loading state').toBeFalse();
      done();
    });
  });

  /**
   * Test basic show/hide functionality
   */
  it('should set loading to true when show() is called', (done) => {
    // Call show
    service.show();
    
    // Check loading state
    service.isLoading().subscribe(loading => {
      expect(loading).withContext('Loading state after show()').toBeTrue();
      done();
    });
  });

  it('should set loading to false when hide() is called after show()', (done) => {
    // First show, then hide
    service.show();
    service.hide();
    
    // Check loading state
    service.isLoading().subscribe(loading => {
      expect(loading).withContext('Loading state after show() then hide()').toBeFalse();
      done();
    });
  });

  /**
   * Test nested loading operations
   */
  describe('nested loading operations', () => {
    it('should maintain loading state true when nested operations are in progress', (done) => {
      // Show loader twice (simulating two concurrent operations)
      service.show();
      service.show();
      
      // Check that loading is true
      service.isLoading().subscribe(loading => {
        expect(loading).withContext('Loading state with multiple show() calls').toBeTrue();
        done();
      });
    });

    it('should only hide loader when all operations are complete', () => {
      // Setup: show twice
      service.show();
      service.show();
      
      // Act: hide once
      service.hide();
      
      // Assert: still should be loading
      expect(service.loading).withContext('Loading state after one hide()').toBeTrue();
      
      // Act: hide again
      service.hide();
      
      // Assert: no longer loading
      expect(service.loading).withContext('Loading state after second hide()').toBeFalse();
    });
  });

  /**
   * Test reset functionality
   */
  describe('reset operation', () => {
    it('should reset loading state regardless of counter', (done) => {
      // Setup: multiple show calls
      service.show();
      service.show();
      service.show();
      
      // Act: reset
      service.reset();
      
      // Assert
      service.isLoading().subscribe(loading => {
        expect(loading).withContext('Loading state after reset()').toBeFalse();
        done();
      });
    });

    it('should allow show operations after reset', (done) => {
      // Setup: show and reset
      service.show();
      service.reset();
      
      // Act: show again
      service.show();
      
      // Assert
      service.isLoading().subscribe(loading => {
        expect(loading).withContext('Loading state after reset() and show()').toBeTrue();
        done();
      });
    });
  });

  /**
   * Test edge cases
   */
  describe('edge cases', () => {
    it('should handle hide() being called more times than show()', (done) => {
      // Call hide without previous show
      service.hide();
      service.hide();
      
      // Should still be in not-loading state
      service.isLoading().subscribe(loading => {
        expect(loading).withContext('Loading state after excess hide() calls').toBeFalse();
        done();
      });
    });
  });
});
