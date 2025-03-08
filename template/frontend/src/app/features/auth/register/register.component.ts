import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { NotificationService } from '../../../core/services/notification.service';

// User status and role enums matching backend
enum UserStatus {
  Unknown = 0,
  Active = 1,
  Inactive = 2,
  Suspended = 3
}

enum UserRole {
  None = 0,
  Customer = 1,
  Manager = 2,
  Admin = 3
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSelectModule
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm: FormGroup;
  error: string = '';
  loading: boolean = false;
  
  // Expose enums to template
  userStatuses = Object.keys(UserStatus)
    .filter(key => !isNaN(Number(UserStatus[key as keyof typeof UserStatus])))
    .map(key => ({ 
      value: UserStatus[key as keyof typeof UserStatus], 
      label: key 
    }));
  
  userRoles = Object.keys(UserRole)
    .filter(key => !isNaN(Number(UserRole[key as keyof typeof UserRole])))
    .map(key => ({ 
      value: UserRole[key as keyof typeof UserRole], 
      label: key 
    }));

  constructor(
    private formBuilder: FormBuilder,
    private apiService: ApiService,
    private router: Router,
    private notificationService: NotificationService
  ) {
    this.registerForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^\+?[1-9]\d{1,14}$/)]],
      status: [UserStatus.Active, [Validators.required]],
      role: [UserRole.Customer, [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';

    this.apiService.post('users', this.registerForm.value)
      .subscribe({
        next: () => {
          this.notificationService.success('Registration successful! Please log in.');
          this.router.navigate(['/auth/login']);
        },
        error: (err) => {
          if (err.status === 400) {
            this.error = err.error?.message || 'Please check your input and try again';
          } else {
            this.error = 'An error occurred during registration. Please try again later.';
            this.notificationService.error('Registration failed: ' + (err.error?.message || 'An unexpected error occurred'));
          }
          this.loading = false;
          console.error('Registration error:', err);
        }
      });
  }
}
