import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

// Standalone pipes imports
import { TruncatePipe } from './truncate.pipe';
import { SafeHtmlPipe } from './safe-html.pipe';
import { DateTimeFormatPipe } from './date-time-format.pipe';

/**
 * Module that provides reusable pipes across the application.
 * Contains pipes for data transformation and formatting operations.
 * 
 * All pipes in this module are standalone pipes, so they are
 * imported in the module's imports array rather than declared.
 */
@NgModule({
  imports: [
    CommonModule,
    // Standalone pipes are imported here
    TruncatePipe,
    SafeHtmlPipe,
    DateTimeFormatPipe
  ],
  providers: [
    // Provide Angular's built-in DatePipe for use in the DateTimeFormatPipe
    DatePipe
  ],
  exports: [
    // Export the standalone pipes for use in other modules
    TruncatePipe,
    SafeHtmlPipe,
    DateTimeFormatPipe
  ]
})
export class SharedPipesModule { } 