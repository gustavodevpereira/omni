import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgIf, AsyncPipe } from '@angular/common';
import { AuthService } from '../services/auth.service';

/**
 * @description
 * Header component that provides application-wide navigation and authentication controls.
 * 
 * This component serves as the main navigation header for the application, displaying:
 * - The application logo and brand
 * - Primary navigation links (conditional based on authentication status)
 * - Authentication controls (login/register or user profile/logout)
 * 
 * The component dynamically adjusts its display based on the user's authentication state,
 * showing different navigation options for authenticated users versus guests.
 * 
 * @usageNotes
 * Place this component at the top of your main layout:
 * ```html
 * <app-header></app-header>
 * <main>
 *   <router-outlet></router-outlet>
 * </main>
 * <app-footer></app-footer>
 * ```
 */
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, NgIf, AsyncPipe],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  /**
   * Creates an instance of HeaderComponent.
   * 
   * @param authService - Service that provides authentication status and user information
   * The service is injected as public to allow direct access from the template
   */
  constructor(public authService: AuthService) {}

  /**
   * Logs out the current user by delegating to the authentication service.
   * This method is triggered when the user clicks the logout link.
   * 
   * @example
   * ```html
   * <a (click)="logout()">Logout</a>
   * ```
   */
  logout(): void {
    this.authService.logout();
  }
}
