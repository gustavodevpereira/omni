/**
 * Unit tests for LoginComponent.
 * 
 * @description
 * This test suite verifies the functionality of the login component,
 * ensuring it properly handles authentication flows, form validation,
 * error scenarios, and UI state management.
 * 
 * Key test areas include:
 * - Form initialization and validation
 * - Authentication service integration
 * - Error handling and display
 * - Loading state management
 * - Navigation after successful login
 * - UI interactions and disabled states
 */

import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, provideRouter, RouterLink, Routes } from '@angular/router';
import { provideLocationMocks } from '@angular/common/testing';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Observable, of, throwError } from 'rxjs';
import { HarnessLoader } from '@angular/cdk/testing';
import { TestbedHarnessEnvironment } from '@angular/cdk/testing/testbed';
import { MatButtonHarness } from '@angular/material/button/testing';
import { HttpErrorResponse } from '@angular/common/http';
import { By } from '@angular/platform-browser';
import { 
  AuthResponse, 
  AuthRequest 
} from '../../../shared/models/user.model';
import { ApiResponseWithData } from '../../../shared/models/api-response.models';
import { Component } from '@angular/core';

/**
 * Mock component for register link to avoid RouterLink issues
 */
@Component({
  selector: 'mock-router-outlet',
  standalone: true,
  template: ''
})
class MockRouterOutletComponent {}

/**
 * Test suite for the Login Component
 * 
 * @description
 * Tests the authentication UI component that handles user login.
 * Ensures proper form validation, service integration, error handling,
 * and UI state management throughout the authentication flow.
 */
describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;
  let notificationService: jasmine.SpyObj<NotificationService>;
  let loader: HarnessLoader;

  /**
   * Test suite setup
   * 
   * @description
   * Configures the testing module with the LoginComponent and its dependencies.
   * Sets up spy objects for services to allow mocking and verification.
   */
  beforeEach(async () => {
    // Create spies for injected services
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['login']);
    const notificationServiceSpy = jasmine.createSpyObj('NotificationService', ['success', 'error']);
    
    // Create mock routes configuration
    const routes: Routes = [
      { path: 'auth/register', component: MockRouterOutletComponent },
      { path: 'products', component: MockRouterOutletComponent }
    ];
    
    await TestBed.configureTestingModule({
      providers: [
        FormBuilder,
        provideRouter(routes),
        provideLocationMocks(),
        { provide: AuthService, useValue: authServiceSpy },
        { provide: NotificationService, useValue: notificationServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              queryParams: {}
            }
          }
        }
      ],
      imports: [
        // Import standalone components directly
        LoginComponent,
        MockRouterOutletComponent,
        BrowserAnimationsModule
      ]
    }).compileComponents();

    TestBed.inject(Router); // Ensure Router is initialized

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    notificationService = TestBed.inject(NotificationService) as jasmine.SpyObj<NotificationService>;
    
    // Create the harness loader for Material component testing
    loader = TestbedHarnessEnvironment.loader(fixture);
    
    fixture.detectChanges();
  });

  /**
   * Verifies that the component initializes correctly
   * 
   * @description
   * Ensures the component is created successfully and basic initialization works.
   */
  it('should create', () => {
    expect(component).toBeTruthy();
  });

  /**
   * Tests that the login form initializes with empty values
   * 
   * @description
   * Verifies that the form controls are created properly with empty values
   * and the form is initially in an invalid state.
   */
  it('should initialize with an empty form', () => {
    expect(component.loginForm.get('email')?.value).toBe('');
    expect(component.loginForm.get('password')?.value).toBe('');
    expect(component.loginForm.valid).toBeFalsy();
  });

  /**
   * Tests that returnUrl is properly set from query parameters
   * 
   * @description
   * Verifies that the component extracts and uses the returnUrl query parameter
   * from the route, which controls post-login navigation.
   */
  it('should set returnUrl from query params when available', () => {
    // Override the returnUrl with manual setup
    component.returnUrl = '';
    const activatedRoute = TestBed.inject(ActivatedRoute);
    // Use Object.defineProperty to override the readonly snapshot property
    Object.defineProperty(activatedRoute, 'snapshot', {
      get: () => ({ queryParams: { returnUrl: '/dashboard' } })
    });
    
    component.ngOnInit();
    expect(component.returnUrl).toBe('/dashboard');
  });

  /**
   * Tests email field validation
   * 
   * @description
   * Verifies that the email field properly validates:
   * - Required field validation
   * - Email format validation
   * - Acceptance of valid email addresses
   */
  it('should validate email field', () => {
    const emailControl = component.loginForm.get('email');
    
    // Required validation
    emailControl?.setValue('');
    expect(emailControl?.valid).toBeFalsy();
    expect(emailControl?.hasError('required')).toBeTruthy();
    
    // Email format validation
    emailControl?.setValue('invalid-email');
    expect(emailControl?.valid).toBeFalsy();
    expect(emailControl?.hasError('email')).toBeTruthy();
    
    // Valid email
    emailControl?.setValue('test@example.com');
    expect(emailControl?.valid).toBeTruthy();
  });

  /**
   * Tests password field validation
   * 
   * @description
   * Verifies that the password field properly validates:
   * - Required field validation
   * - Minimum length validation
   * - Acceptance of valid passwords
   */
  it('should validate password field', () => {
    const passwordControl = component.loginForm.get('password');
    
    // Required validation
    passwordControl?.setValue('');
    expect(passwordControl?.valid).toBeFalsy();
    expect(passwordControl?.hasError('required')).toBeTruthy();
    
    // Min length validation
    passwordControl?.setValue('12345');
    expect(passwordControl?.valid).toBeFalsy();
    expect(passwordControl?.hasError('minlength')).toBeTruthy();
    
    // Valid password
    passwordControl?.setValue('123456');
    expect(passwordControl?.valid).toBeTruthy();
  });

  /**
   * Tests that the form submission is prevented when the form is invalid
   * 
   * @description
   * Verifies that the auth service's login method is not called when
   * attempting to submit with invalid form data.
   */
  it('should not submit if form is invalid', () => {
    component.loginForm.controls['email'].setValue('');
    component.loginForm.controls['password'].setValue('');
    
    component.onSubmit();
    
    expect(authService.login).not.toHaveBeenCalled();
  });

  /**
   * Tests successful login flow
   * 
   * @description
   * Verifies the complete login success path:
   * - Correct form data is passed to auth service
   * - Loading state is properly managed
   * - Success notification is displayed
   * - User is redirected to the appropriate page
   */
  it('should call auth service and handle successful login', fakeAsync(() => {
    // Set up form with valid values
    component.loginForm.controls['email'].setValue('test@example.com');
    component.loginForm.controls['password'].setValue('password123');
    
    // Create credentials object that matches what the service expects
    const credentials: AuthRequest = {
      email: 'test@example.com',
      password: 'password123'
    };
    
    // Create auth response matching the interface in user.model.ts
    const authResponse: AuthResponse = {
      token: 'test-token',
      email: 'test@example.com',
      name: 'Test User',
      role: 'customer'
    };
    
    // Create API response with the correct structure
    const apiResponse: ApiResponseWithData<AuthResponse> = {
      success: true,
      message: 'Authentication successful',
      data: authResponse,
      errors: []
    };
    
    // Mock the auth service to return the proper response
    authService.login.and.returnValue(of(apiResponse));
    
    // Ensure router.navigateByUrl is properly set up as a spy
    // This step might be redundant if your setup is already working, but let's be thorough
    router.navigateByUrl = jasmine.createSpy('navigateByUrl').and.returnValue(Promise.resolve(true));

    // Submit the form
    component.onSubmit();
    tick();
    
    // Verify the service was called with the right data
    expect(authService.login).toHaveBeenCalledWith(credentials);
    
    // Verify success flow
    expect(component.loading).toBeFalsy();
    expect(notificationService.success).toHaveBeenCalledWith('Login successful!');
    expect(router.navigateByUrl).toHaveBeenCalledWith('/products');
  }));

  /**
   * Tests authentication error handling
   * 
   * @description
   * Verifies that authentication errors (401 Unauthorized) are properly handled:
   * - Loading state is reset
   * - Appropriate error message is displayed
   */
  it('should handle login error', fakeAsync(() => {
    // Set up form with valid values
    component.loginForm.controls['email'].setValue('test@example.com');
    component.loginForm.controls['password'].setValue('password123');
    
    // Create error response that matches the HttpErrorResponse format
    const errorResponse = new HttpErrorResponse({
      error: {
        message: 'Invalid credentials',
        errors: [{
          propertyName: 'email',
          errorMessage: 'Invalid email or password',
          attemptedValue: 'test@example.com',
          errorCode: 'AUTH_FAILED'
        }]
      },
      status: 401
    });
    
    // Mock the auth service to throw an error
    authService.login.and.returnValue(throwError(() => errorResponse));
    
    // Submit the form
    component.onSubmit();
    tick();
    
    // Verify error handling
    expect(component.loading).toBeFalsy();
    expect(component.error).toBe('Invalid email or password');
  }));

  /**
   * Tests generic server error handling
   * 
   * @description
   * Verifies that non-authentication server errors (e.g., 500) are properly handled:
   * - Loading state is reset
   * - Generic error message is displayed
   */
  it('should show generic error for server errors', fakeAsync(() => {
    // Set up form with valid values
    component.loginForm.controls['email'].setValue('test@example.com');
    component.loginForm.controls['password'].setValue('password123');
    
    // Create error response for server error
    const errorResponse = new HttpErrorResponse({
      status: 500,
      statusText: 'Internal Server Error'
    });
    
    // Mock the auth service to throw an error
    authService.login.and.returnValue(throwError(() => errorResponse));
    
    // Submit the form
    component.onSubmit();
    tick();
    
    // Verify error handling for server error
    expect(component.loading).toBeFalsy();
    expect(component.error).toBe('An error occurred during login. Please try again later.');
  }));

  /**
   * Tests loading state during authentication
   * 
   * @description
   * Verifies that the loading state is properly managed during authentication:
   * - Loading flag is set while waiting for response
   * - Loading spinner is displayed in the UI
   */
  it('should show loading spinner during authentication', fakeAsync(() => {
    // Set up form with valid values
    component.loginForm.controls['email'].setValue('test@example.com');
    component.loginForm.controls['password'].setValue('password123');
    
    // Create a delayed response
    authService.login.and.returnValue(new Observable(observer => {
      // Don't complete immediately to keep loading state active
      setTimeout(() => {}, 1000);
    }));
    
    // Submit form
    component.onSubmit();
    fixture.detectChanges();
    
    // Verify loading state
    expect(component.loading).toBeTruthy();
    
    // Check for spinner in DOM
    const spinner = fixture.debugElement.query(By.css('mat-spinner'));
    expect(spinner).toBeTruthy();
    
    // Clean up
    tick(1000);
  }));

  /**
   * Tests submit button disabled state
   * 
   * @description
   * Verifies that the submit button is:
   * - Disabled when the form is invalid
   * - Enabled when the form becomes valid
   * 
   * Uses Angular Material test harnesses for reliable component interaction.
   */
  it('should disable login button when form is invalid', async () => {
    // Form starts invalid
    fixture.detectChanges();
    
    // Get login button using Material test harness
    const buttons = await loader.getAllHarnesses(MatButtonHarness.with({text: /Sign In/}));
    expect(buttons.length).toBe(1);
    
    const loginButton = buttons[0];
    
    // Button should be disabled when form is invalid
    expect(await loginButton.isDisabled()).toBeTruthy();
    
    // Make form valid
    component.loginForm.controls['email'].setValue('test@example.com');
    component.loginForm.controls['password'].setValue('password123');
    fixture.detectChanges();
    
    // Button should now be enabled
    expect(await loginButton.isDisabled()).toBeFalsy();
  });
});
