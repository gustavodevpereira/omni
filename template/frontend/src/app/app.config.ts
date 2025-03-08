import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { ENVIRONMENT } from './core/tokens/environment.token';
import { environment } from '../environments/environment';

// Importar interceptors
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { loaderInterceptor } from './core/interceptors/loader.interceptor';

// Importação do MaterialModule se necessário
import { MaterialModule } from './shared/material.module';

/**
 * Application configuration providers.
 * 
 * This configuration centralizes all application-level providers and services.
 * It sets up the router, HTTP client with interceptors, and browser animations.
 * 
 * In a non-standalone application, these providers would typically be in the AppModule.
 * In the standalone architecture, they are defined here instead.
 */
export const appConfig: ApplicationConfig = {
  providers: [
    // Environment configuration
    { provide: ENVIRONMENT, useValue: environment },
    
    // Router configuration
    provideRouter(routes, withComponentInputBinding()),
    
    // HTTP configuration with interceptors
    provideHttpClient(
      withInterceptors([
        authInterceptor,
        errorInterceptor,
        loaderInterceptor
      ])
    ),
    
    // Animation support for Angular Material components
    provideAnimations(),
    
    // Import Material Module if needed
    importProvidersFrom(MaterialModule)
    
    // Adicione aqui quaisquer outros providers que estavam nos módulos
  ]
};
