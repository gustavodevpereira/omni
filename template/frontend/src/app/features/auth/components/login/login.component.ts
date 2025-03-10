import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../core/services/auth.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { SharedModule } from '../../../../shared/shared.module';

/**
 * Login Component
 * 
 * Provides a form for users to log in to the application.
 */
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    SharedModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="container mt-5">
      <div class="row justify-content-center">
        <div class="col-md-6 col-sm-12">
          <mat-card>
            <mat-card-header>
              <mat-card-title>Login</mat-card-title>
              <mat-card-subtitle>Sign in to your account</mat-card-subtitle>
            </mat-card-header>
            
            <mat-card-content>
              <form [formGroup]="loginForm" (ngSubmit)="onSubmit()" class="mt-3">
                <mat-form-field appearance="outline" class="full-width">
                  <mat-label>Email</mat-label>
                  <input matInput type="email" formControlName="email" placeholder="Email">
                  <mat-error *ngIf="loginForm.get('email')?.hasError('required')">
                    Email is required
                  </mat-error>
                  <mat-error *ngIf="loginForm.get('email')?.hasError('email')">
                    Please enter a valid email
                  </mat-error>
                </mat-form-field>
                
                <mat-form-field appearance="outline" class="full-width mt-2">
                  <mat-label>Password</mat-label>
                  <input matInput type="password" formControlName="password" placeholder="Password">
                  <mat-error *ngIf="loginForm.get('password')?.hasError('required')">
                    Password is required
                  </mat-error>
                </mat-form-field>
                
                <div class="form-group mt-2">
                  <mat-checkbox formControlName="rememberMe">Remember me</mat-checkbox>
                </div>
                
                <div class="mt-4">
                  <button 
                    mat-raised-button 
                    color="primary" 
                    type="submit" 
                    class="full-width" 
                    [disabled]="loginForm.invalid || isLoading">
                    <span *ngIf="!isLoading">Login</span>
                    <mat-spinner *ngIf="isLoading" diameter="24" class="spinner-center"></mat-spinner>
                  </button>
                </div>
              </form>
            </mat-card-content>
            
            <mat-card-actions align="start">
            </mat-card-actions>
          </mat-card>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .full-width {
      width: 100%;
    }
    .spinner-center {
      margin: 0 auto;
    }
    mat-card {
      box-shadow: 0 4px 20px rgba(0,0,0,0.1);
      border-radius: 8px;
    }
    mat-card-actions {
      padding: 16px;
    }
  `]
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  isLoading = false;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    this.initForm();
    
    // Check if user is already logged in
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/products']);
    }
  }

  /**
   * Initialize the login form
   */
  private initForm(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      rememberMe: [false]
    });
  }

  /**
   * Submit the login form
   */
  onSubmit(): void {
    if (this.loginForm.invalid || this.isLoading) {
      return;
    }
    
    // Get form values
    const { email, password, rememberMe } = this.loginForm.value;
    
    // Set loading state
    this.isLoading = true;
    
    // Call auth service to login
    this.authService.login(email, password, rememberMe)
      .subscribe({
        next: (user) => {
          this.isLoading = false;
          this.notificationService.success(`Welcome back, ${user.name}!`);
          
          // Get return url from route parameters or default to '/products'
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/products';
          this.router.navigate([returnUrl]);
        },
        error: (error) => {
          this.isLoading = false;
          // Error is already handled by the interceptor
        }
      });
  }
} 