import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Standalone directives imports
import { ClickOutsideDirective } from './click-outside.directive';
import { NumberOnlyDirective } from './number-only.directive';
import { AutoFocusDirective } from './auto-focus.directive';

/**
 * Module that provides reusable directives across the application.
 * Includes directives for handling common UI behaviors and interactions.
 * 
 * All directives in this module are standalone directives, so they are
 * imported in the module's imports array rather than declared.
 */
@NgModule({
  imports: [
    CommonModule,
    // Standalone directives are imported here
    ClickOutsideDirective,
    NumberOnlyDirective,
    AutoFocusDirective
  ],
  exports: [
    // Export the standalone directives for use in other modules
    ClickOutsideDirective,
    NumberOnlyDirective,
    AutoFocusDirective
  ]
})
export class SharedDirectivesModule { } 