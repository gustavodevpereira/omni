import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthApiService } from '../api/services/auth-api.service';
import { AuthRequests } from '../api/models/requests.model';
import { AuthResponses } from '../api/models/responses.model';
import { UserRole, UserStatus, User } from '../api/models/domain.model';

/**
 * Authentication Service
 * 
 * Handles user authentication, registration, and session management.
 * Uses the AuthApiService for HTTP requests and manages the current user state.
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // BehaviorSubject to store and observe the current authenticated user
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$: Observable<User | null> = this.currentUserSubject.asObservable();

  // BehaviorSubject to store and observe the authentication state
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$: Observable<boolean> = this.isAuthenticatedSubject.asObservable();

  constructor(
    private authApiService: AuthApiService,
    private router: Router
  ) {
    // Check if user is already logged in (from localStorage)
    this.checkAuthState();
  }

  /**
   * Check if user is logged in from localStorage
   */
  private checkAuthState(): void {
    const token = localStorage.getItem('auth_token');
    const user = localStorage.getItem('current_user');
    
    if (token && user) {
      try {
        // Convert the stored user data to domain model
        const userData = JSON.parse(user) as User;
        
        // Update authentication state
        this.currentUserSubject.next(userData);
        this.isAuthenticatedSubject.next(true);
      } catch (error) {
        // Invalid user data in localStorage
        this.logout();
      }
    }
  }

  /**
   * Check if user is authenticated
   * @returns True if user is authenticated
   */
  public isAuthenticated(): boolean {
    return this.isAuthenticatedSubject.getValue();
  }

  /**
   * Get current user
   * @returns Current user or null
   */
  public getCurrentUser(): User | null {
    return this.currentUserSubject.getValue();
  }

  /**
   * Map API auth result to domain user
   * @param authResult - Auth result data from API
   * @returns Domain user model
   */
  private mapToDomainUser(authResult: AuthResponses.AuthResult): User {
    return {
      id: '', // Default value as it's not in the auth response
      email: authResult.email,
      name: authResult.name,
      phone: '', // Default value as it's not in the auth response
      role: authResult.role as UserRole,
      status: UserStatus.Active
    };
  }

  /**
   * Log in a user with email and password
   * 
   * @param email - User email
   * @param password - User password
   * @param rememberMe - Whether to remember the user
   * @returns Observable of the user
   */
  public login(email: string, password: string, rememberMe = false): Observable<User> {
    const loginData: AuthRequests.Login = {
      email,
      password,
      rememberMe
    };
    
    return this.authApiService.login(loginData).pipe(
      map(authResult => {
        // Save auth data to localStorage
        this.storeAuthData(authResult);
        
        // Convert API user to domain user
        const user = this.mapToDomainUser(authResult);
        
        // Update authentication state
        this.currentUserSubject.next(user);
        this.isAuthenticatedSubject.next(true);
        
        return user;
      }),
      catchError(error => {
        console.error('Login error:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Store authentication data in localStorage
   * 
   * @param authResult - Authentication result from API
   */
  private storeAuthData(authResult: AuthResponses.AuthResult): void {
    localStorage.setItem('auth_token', authResult.token);
    
    // Create and store user object
    const user: User = {
      id: '', // Default value as it's not in the auth response
      email: authResult.email,
      name: authResult.name,
      phone: '', // Default value as it's not in the auth response
      role: authResult.role as UserRole,
      status: UserStatus.Active
    };
    
    localStorage.setItem('current_user', JSON.stringify(user));
  }

  /**
   * Register a new user
   * 
   * @param registerData - Registration data
   * @returns Observable of the registered user
   */
  public register(registerData: AuthRequests.Register): Observable<User> {
    return this.authApiService.register(registerData).pipe(
      map(authResult => {
        // Save auth data to localStorage
        this.storeAuthData(authResult);
        
        // Convert API user to domain user
        const user = this.mapToDomainUser(authResult);
        
        // Update authentication state
        this.currentUserSubject.next(user);
        this.isAuthenticatedSubject.next(true);
        
        return user;
      }),
      catchError(error => {
        console.error('Registration error:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Log out the current user
   * Removes token from localStorage and redirects to login page
   */
  public logout(): void {
    // Remove user from localStorage
    localStorage.removeItem('auth_token');
    localStorage.removeItem('current_user');
    
    // Update authentication state
    this.currentUserSubject.next(null);
    this.isAuthenticatedSubject.next(false);
    
    // Redirect to login page
    this.router.navigate(['/auth/login']);
  }

  /**
   * Request password reset for a user
   * 
   * @param email - User email
   * @returns Observable with success message
   */
  public forgotPassword(email: string): Observable<string> {
    return this.authApiService.forgotPassword({ email });
  }

  /**
   * Reset user password using token
   * 
   * @param token - Reset token
   * @param password - New password
   * @param confirmPassword - Confirm new password
   * @returns Observable with success message
   */
  public resetPassword(token: string, password: string, confirmPassword: string): Observable<string> {
    return this.authApiService.resetPassword({ token, password, confirmPassword });
  }
} 