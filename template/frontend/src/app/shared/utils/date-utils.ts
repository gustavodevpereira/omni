/**
 * Utility functions for working with dates
 */
export class DateUtils {
  /**
   * Formats a date into a consistent string representation
   */
  static formatDate(date: Date | string | number, format: string = 'MMM dd, yyyy'): string {
    const d = new Date(date);
    
    if (isNaN(d.getTime())) {
      return '';
    }
    
    // Simple formatting function - for production, consider using date-fns or Intl.DateTimeFormat
    const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    
    const yyyy = d.getFullYear();
    const yy = yyyy.toString().slice(-2);
    const m = d.getMonth();
    const mm = m + 1; // 0-based month
    const mmPadded = mm < 10 ? `0${mm}` : mm;
    const mmm = months[m];
    const dd = d.getDate();
    const ddPadded = dd < 10 ? `0${dd}` : dd;
    
    return format
      .replace('yyyy', yyyy.toString())
      .replace('yy', yy.toString())
      .replace('MMM', mmm)
      .replace('MM', mmPadded)
      .replace('dd', ddPadded);
  }
} 