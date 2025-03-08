import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

/**
 * @description
 * Footer component that displays application copyright information, navigation links,
 * contact details, and social media links. This component appears at the bottom of all
 * application pages to provide consistent navigation and branding.
 *
 * @usageNotes
 * This component should be included in the main application layout.
 * Example:
 * ```html
 * <app-layout>
 *   <app-header></app-header>
 *   <main>...</main>
 *   <app-footer></app-footer>
 * </app-layout>
 * ```
 *
 * @implements {OnInit}
 */
@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  /**
   * The current year as a string, used in the copyright notice.
   * This is automatically set to the current year when the component is instantiated.
   * 
   * @public
   * @readonly
   * @type {string}
   */
  readonly currentYear: string = new Date().getFullYear().toString();
  
  // Social media links
  readonly socialLinks = [
    { icon: 'facebook', url: 'https://facebook.com', label: 'Facebook' },
    { icon: 'photo_camera', url: 'https://instagram.com', label: 'Instagram' },
    { icon: 'linkedin', url: 'https://linkedin.com', label: 'LinkedIn' }
  ];
  
  // Quick access links
  readonly quickLinks = [
    { icon: 'shopping_bag', url: '/products', label: 'Products' },
    { icon: 'shopping_cart', url: '/cart', label: 'Cart' },
    { icon: 'info', url: '/about', label: 'About Us' },
    { icon: 'contact_support', url: '/contact', label: 'Contact' }
  ];
}
