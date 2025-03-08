import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';
import { LoggingService } from '../services/logging.service';
import { UserRole } from '../../shared/models/user.model';

/**
 * @description
 * Route guard that protects admin-only routes.
 * 
 * This guard provides two levels of protection:
 * 1. Verifies that the user is authenticated
 * 2. Verifies that the authenticated user has admin role
 * 
 * Users who are not logged in are redirected to the login page.
 * Users who are logged in but lack admin privileges are redirected to a non-admin page
 * with an access denied notification.
 * 
 * @usageNotes
 * Apply this guard to routes that should only be accessible to administrators:
 * 
 * ```typescript
 * const routes: Routes = [
 *   {
 *     path: 'admin/dashboard',
 *     component: AdminDashboardComponent,
 *     canActivate: [adminGuard]
 *   }
 * ];
 * ```
 * 
 * @returns `true` if the user is authenticated and has admin role, otherwise a `UrlTree` 
 * redirecting to the appropriate page based on the user's status
 */
export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const notificationService = inject(NotificationService);
  const loggingService = inject(LoggingService);
  
  loggingService.logInfo(`Admin guard checking access for route: ${state.url}`);
  
  // First, check if the user is logged in at all
  if (!authService.isLoggedIn()) {
    loggingService.logWarning(`User not logged in, redirecting to login from ${state.url}`);
    notificationService.warning('You must be logged in to access this page');
    
    return router.createUrlTree(['/auth/login'], { 
      queryParams: { returnUrl: state.url }
    });
  }
  
  // Then check if the logged-in user has admin role
  return authService.getCurrentUser().pipe(
    map(user => {
      if (user && user.role === UserRole.ADMIN) {
        loggingService.logInfo(`User ${user.email} has admin role, granting access to ${state.url}`);
        return true;
      }
      
      loggingService.logWarning(`User ${user?.email || 'unknown'} lacks admin role, access denied to ${state.url}`);
      notificationService.error('You do not have permission to access this page');
      return router.createUrlTree(['/products']);
    }),
    catchError(error => {
      loggingService.logError(`Error checking admin permissions: ${error.message}`, error);
      notificationService.error('Error checking permissions');
      return of(false);
    })
  );
}; 