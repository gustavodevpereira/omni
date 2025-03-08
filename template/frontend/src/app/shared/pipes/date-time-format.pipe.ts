import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

/**
 * Formats date and time consistently throughout the application
 * 
 * Usage:
 * {{ dateValue | dateTimeFormat }}
 * {{ dateValue | dateTimeFormat:'short' }}
 * {{ dateValue | dateTimeFormat:'medium':'en-US' }}
 */
@Pipe({
  name: 'dateTimeFormat',
  standalone: true
})
export class DateTimeFormatPipe implements PipeTransform {
  constructor(private datePipe: DatePipe) {}

  transform(
    value: Date | string | number,
    format: 'short' | 'medium' | 'long' | 'full' = 'medium',
    locale: string = 'en-US'
  ): string | null {
    if (!value) {
      return '';
    }

    let dateFormat: string;

    switch (format) {
      case 'short':
        dateFormat = 'MM/dd/yy, h:mm a';
        break;
      case 'medium':
        dateFormat = 'MMM d, y, h:mm:ss a';
        break;
      case 'long':
        dateFormat = 'MMMM d, y, h:mm:ss a z';
        break;
      case 'full':
        dateFormat = 'EEEE, MMMM d, y, h:mm:ss a zzzz';
        break;
      default:
        dateFormat = 'MMM d, y, h:mm:ss a';
    }

    return this.datePipe.transform(value, dateFormat, undefined, locale);
  }
} 