import { Component, OnInit } from '@angular/core';
import { LayoutComponent } from './core/layout/layout.component';
import { NotificationService } from './core/services/notification.service';
import { AuthService } from './core/services/auth.service';

/**
 * Root component of the application.
 * 
 * This is the entry point of the application UI. It serves as the
 * container for the main layout and handles initial application setup,
 * such as checking authentication status on startup.
 */
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [LayoutComponent],
  template: `<app-layout></app-layout>`,
  styles: []
})
export class AppComponent implements OnInit {
  title = 'Ambev Store';

  /**
   * Creates an instance of AppComponent.
   * 
   * @param authService Service for authentication operations
   * @param notificationService Service for displaying notifications to users
   */
  constructor(
    private authService: AuthService,
    private notificationService: NotificationService
  ) {}

  /**
   * Lifecycle hook called after component initialization.
   * Checks authentication status and restores user session if needed.
   */
  ngOnInit(): void {
    this.checkAuthStatus();
  }

  /**
   * Checks if user is authenticated on application initialization.
   * Displays relevant notifications based on authentication status.
   */
  private checkAuthStatus(): void {
    if (this.authService.isLoggedIn()) {
      this.authService.getCurrentUser().subscribe(user => {
        if (user) {
          this.notificationService.info(`Welcome back, ${user.name || user.email}`);
        }
      });
    }
  }
}
