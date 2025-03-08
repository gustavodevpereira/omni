import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';

/**
 * Application routes configuration.
 * 
 * Defines the route structure of the application, including:
 * - Public routes that don't require authentication
 * - Protected routes that require user authentication
 * - Default and fallback routes
 * 
 * Each feature module is lazy-loaded to improve initial load performance.
 */
export const routes: Routes = [
  // Public routes - accessible without authentication
  {
    path: 'auth',
    children: [
      {
        path: 'login',
        component: LoginComponent
      },
      {
        path: 'register',
        component: RegisterComponent
      },
      {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
      }
    ]
  },
  
  // Protected routes - require authentication
  {
    path: 'products',
    canActivate: [authGuard],
    loadComponent: () => import('./features/products/product-list/product-list.component')
      .then(c => c.ProductListComponent)
  },
  
  // Default route - redirects to login page
  { 
    path: '', 
    redirectTo: '/auth/login', 
    pathMatch: 'full' 
  },
  
  // Fallback route - for invalid paths
  { 
    path: '**', 
    redirectTo: '/auth/login' 
  }
];