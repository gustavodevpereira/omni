import { Pipe, PipeTransform } from '@angular/core';

/**
 * Truncates text to a specified length and adds an ellipsis
 * 
 * Usage:
 * {{ someText | truncate:20 }}
 * {{ someText | truncate:20:'...' }}
 */
@Pipe({
  name: 'truncate'
})
export class TruncatePipe implements PipeTransform {
  transform(value: string, limit: number = 25, trail: string = '...'): string {
    if (!value) {
      return '';
    }

    const result = value.length > limit ? value.substring(0, limit) + trail : value;
    return result;
  }
} 