import { Injectable } from '@angular/core';
import { Observable, of, catchError, map } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { 
  User,
} from '../../shared/models/user.model';
import { Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';

/**
 * @description
 * Authentication Facade that provides a simplified interface for all authentication-related operations.
 * 
 * This facade encapsulates the underlying authentication service implementation and provides
 * a clean, consistent API for authentication flows throughout the application. It handles
 * common authentication tasks such as:
 * - User login and logout
 * - User registration
 * - Authentication state management
 * - Role-based access control
 * - Notification of authentication events
 * 
 * The facade pattern is used here to reduce coupling between components and the authentication
 * service, making it easier to modify the authentication implementation without affecting
 * the components that depend on it.
 * 
 * @usageNotes
 * Inject this service in components that need authentication capabilities:
 * 
 * ```typescript
 * constructor(private authFacade: AuthFacade) {}
 * 
 * login(email: string, password: string): void {
 *   this.authFacade.login(email, password).subscribe(success => {
 *     if (success) {
 *       // Handle successful login
 *     }
 *   });
 * }
 * ```
 */
@Injectable({
  providedIn: 'root'
})
export class AuthFacade {
  /**
   * Creates an instance of AuthFacade.
   * 
   * @param authService - Service that handles the core authentication operations
   * @param router - Angular router for navigation after authentication events
   * @param notificationService - Service for displaying user notifications
   */
  constructor(
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService
  ) {}

  /**
   * Checks if a user is currently authenticated.
   * 
   * @returns An Observable that emits a boolean indicating whether the user is authenticated
   * 
   * @example
   * ```typescript
   * this.authFacade.isAuthenticated().subscribe(isAuth => {
   *   if (isAuth) {
   *     // User is authenticated
   *   } else {
   *     // User is not authenticated
   *   }
   * });
   * ```
   */
  isAuthenticated(): Observable<boolean> {
    return of(this.authService.isLoggedIn());
  }

  /**
   * Retrieves the currently authenticated user.
   * 
   * @returns An Observable that emits the current user object or null if not authenticated
   * 
   * @example
   * ```typescript
   * this.authFacade.getCurrentUser().subscribe(user => {
   *   if (user) {
   *     this.userName = user.name;
   *   }
   * });
   * ```
   */
  getCurrentUser(): Observable<User | null> {
    return this.authService.getCurrentUser();
  }

  /**
   * Authenticates a user with the provided credentials.
   * Handles success and error notifications automatically.
   * 
   * @param email - User's email address
   * @param password - User's password
   * @returns An Observable that emits a boolean indicating success (true) or failure (false)
   * 
   * @example
   * ```typescript
   * this.authFacade.login('user@example.com', 'password123').subscribe(success => {
   *   if (success) {
   *     this.router.navigate(['/dashboard']);
   *   }
   * });
   * ```
   */
  login(email: string, password: string): Observable<boolean> {
    return this.authService.login({ email, password }).pipe(
      map(response => {
        this.notificationService.success('Login successful');
        return true;
      }),
      catchError(error => {
        this.notificationService.error(error.message || 'Login failed');
        return of(false);
      })
    );
  }

  /**
   * Logs out the current user and clears the authentication state.
   * Optionally navigates to the login page and displays a notification.
   * 
   * @param navigateToLogin - Whether to navigate to login page after logout (defaults to true)
   * 
   * @example
   * ```typescript
   * // Logout and navigate to login page
   * this.authFacade.logout();
   * 
   * // Logout without navigation
   * this.authFacade.logout(false);
   * ```
   */
  logout(navigateToLogin: boolean = true): void {
    this.authService.logout();
    
    if (navigateToLogin) {
      this.router.navigate(['/auth/login']);
    }
    
    this.notificationService.info('You have been logged out');
  }

  /**
   * Registers a new user with the provided information.
   * Handles success and error notifications automatically.
   * 
   * @param registerData - User registration data including email, password, and other details
   * @returns An Observable that emits a boolean indicating success (true) or failure (false)
   * 
   * @example
   * ```typescript
   * const userData = {
   *   username: 'newuser',
   *   email: 'newuser@example.com',
   *   password: 'secure123',
   *   role: UserRole.CUSTOMER
   * };
   * 
   * this.authFacade.register(userData).subscribe(success => {
   *   if (success) {
   *     this.router.navigate(['/auth/login']);
   *   }
   * });
   * ```
   */
  register(registerData: any): Observable<boolean> {
    return this.authService.register(registerData).pipe(
      map(response => {
        this.notificationService.success('Registration successful');
        return true;
      }),
      catchError(error => {
        this.notificationService.error(error.message || 'Registration failed');
        return of(false);
      })
    );
  }

  /**
   * Navigates to the default route for authenticated users.
   * Typically used after successful authentication.
   * 
   * @example
   * ```typescript
   * this.authFacade.login(email, password).subscribe(success => {
   *   if (success) {
   *     this.authFacade.navigateToDefault();
   *   }
   * });
   * ```
   */
  navigateToDefault(): void {
    this.router.navigate(['/products']);
  }

  /**
   * Checks if the current user has a specific role.
   * Used for role-based access control throughout the application.
   * 
   * @param role - Role to check (e.g., 'admin', 'customer')
   * @returns An Observable that emits a boolean indicating if the user has the specified role
   * 
   * @example
   * ```typescript
   * this.authFacade.hasRole('admin').subscribe(isAdmin => {
   *   if (isAdmin) {
   *     // Show admin features
   *   } else {
   *     // Hide admin features
   *   }
   * });
   * ```
   */
  hasRole(role: string): Observable<boolean> {
    return this.getCurrentUser().pipe(
      map(user => !!user && user.role === role),
      catchError(() => of(false))
    );
  }
} 