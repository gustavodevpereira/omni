import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material.module';

// Standalone components imports
import { LoadingSpinnerComponent } from './loading-spinner/loading-spinner.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { ErrorMessageComponent } from './error-message/error-message.component';
import { EmptyStateComponent } from './empty-state/empty-state.component';

/**
 * Module that provides reusable UI components across the application.
 * Contains common UI elements such as loading spinners, dialogs, and error displays.
 * 
 * All components in this module are standalone components, so they are
 * imported in the module's imports array rather than declared.
 */
@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    // Standalone components are imported here
    LoadingSpinnerComponent,
    ConfirmDialogComponent,
    ErrorMessageComponent,
    EmptyStateComponent
  ],
  exports: [
    // Export the standalone components for use in other modules
    LoadingSpinnerComponent,
    ConfirmDialogComponent,
    ErrorMessageComponent,
    EmptyStateComponent
  ]
})
export class SharedComponentsModule { } 