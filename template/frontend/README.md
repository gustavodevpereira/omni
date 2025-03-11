# Frontend Project Structure Guide

## Project Structure

```
src/
├── app/
│   ├── core/           # Singleton services, interceptors, and guards
│   ├── features/       # Feature modules (lazy loaded)
│   ├── shared/         # Shared components, directives, and pipes
│   └── app.module.ts   # Root module
```

### Core Module (`/core`)
The core module contains singleton services and features that are loaded once during application startup:

- **Interceptors**: HTTP request/response handling
  - `auth.interceptor.ts` - Handles authentication
  - `error.interceptor.ts` - Global error handling
  - `loading.interceptor.ts` - Loading state management

- **Guards**: Route protection
  - `auth.guard.ts` - Protects routes requiring authentication

- **Services**: Application-wide services
  - `auth.service.ts` - Authentication management
  - `loading.service.ts` - Global loading state
  - Other core services

### Features Module (`/features`)
Each feature is a self-contained module with its own routing, components, and services. Features are lazy-loaded for better performance.

Current features include:

- **Auth Feature** (`/features/auth`)
  - Login

- **Cart Feature** (`/features/cart`)
  - Shopping cart management
  - Checkout process
  - Order history

- **Products Feature** (`/features/products`)
  - Product listing

### Shared Module (`/shared`)
Contains reusable modules used in the application:

- **Material Module**: Angular Material components configuration
- **SharedModule**: Re-export modules for use in feature modules

## Lazy Loading
The application implements lazy loading through the route configuration in `app-routing.module.ts`. Each feature module is loaded on demand when its route is accessed:

```typescript
{
  path: 'products',
  loadChildren: () => import('./features/products/products.routes')
    .then(m => m.PRODUCTS_ROUTES)
}
```
