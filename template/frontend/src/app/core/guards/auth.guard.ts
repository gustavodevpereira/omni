import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { LoggingService } from '../services/logging.service';
import { NotificationService } from '../services/notification.service';

/**
 * @description
 * Route guard that protects routes requiring authentication.
 * 
 * This guard verifies that a user is logged in before allowing access to protected routes.
 * If the user is not authenticated, they are redirected to the login page with the 
 * originally requested URL saved as a return destination after successful authentication.
 * 
 * @usageNotes
 * Apply this guard to routes that should only be accessible to authenticated users:
 * 
 * ```typescript
 * const routes: Routes = [
 *   {
 *     path: 'profile',
 *     component: ProfileComponent,
 *     canActivate: [authGuard]
 *   }
 * ];
 * ```
 * 
 * @returns `true` if the user is authenticated, otherwise a `UrlTree` redirecting to the login page
 */
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const loggingService = inject(LoggingService);
  const notificationService = inject(NotificationService);

  // Se o usuário já estiver na rota de login ou registro, permita o acesso
  if (state.url.includes('/auth/login') || state.url.includes('/auth/register')) {
    return true;
  }
  
  loggingService.logInfo(`Auth guard checking if user is logged in for route: ${state.url}`);
  
  if (authService.isLoggedIn()) {
    loggingService.logInfo(`User is authenticated, allowing access to ${state.url}`);
    return true;
  }

  loggingService.logWarning(`User is not authenticated, redirecting to login from ${state.url}`);
  notificationService.warning('Please log in to access this page');
  
  // Redirect to login with the attempted URL as the return URL
  return router.createUrlTree(['/auth/login'], { 
    queryParams: { returnUrl: state.url }
  });
};
