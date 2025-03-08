import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';

/**
 * Main layout component that structures the application's UI.
 * 
 * @description
 * This component provides the core layout structure of the application,
 * organizing the UI into three main sections:
 * 1. Header - Contains navigation controls and user information
 * 2. Main content area - Displays the active route via router-outlet
 * 3. Footer - Contains legal information and secondary links
 * 
 * The component is responsible for maintaining consistent layout across 
 * all authenticated pages of the application.
 * 
 * @usageNotes
 * This component should be included in the root application routing 
 * as a parent component for all authenticated routes:
 * 
 * ```typescript
 * const routes: Routes = [
 *   {
 *     path: '',
 *     component: LayoutComponent,
 *     children: [
 *       { path: 'dashboard', component: DashboardComponent },
 *       // other authenticated routes
 *     ]
 *   },
 *   // public routes outside the layout
 * ];
 * ```
 */
@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {
  /**
   * Creates an instance of the LayoutComponent.
   * No parameters required as this is a container component.
   */
  constructor() { }
}
