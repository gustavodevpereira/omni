import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';
import { Observable, map, take } from 'rxjs';
import { AuthService } from '../services/auth.service';

/**
 * Auth Guard - Protege rotas contra acesso não autenticado
 * 
 * Redireciona para a página de login caso o usuário não esteja autenticado
 */
export const authGuard: CanActivateFn = (): 
  Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree => {
  
  const authService = inject(AuthService);
  const router = inject(Router);
  
  return authService.isAuthenticated$
    .pipe(
      take(1),
      map(isAuthenticated => {
        if (isAuthenticated) {
          return true;
        }
        
        // Redirecionar para login salvando a URL atual para redirecionamento após login
        return router.createUrlTree(['/auth/login'], { 
          queryParams: { returnUrl: router.url } 
        });
      })
    );
}; 