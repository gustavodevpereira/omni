/**
 * Utility functions for working with strings
 */
export class StringUtils {
  /**
   * Truncates a string to the specified length and adds an ellipsis
   */
  static truncate(value: string, maxLength: number = 50, ellipsis: string = '...'): string {
    if (!value || value.length <= maxLength) {
      return value;
    }
    
    return value.substring(0, maxLength) + ellipsis;
  }

  /**
   * Capitalizes the first letter of a string
   */
  static capitalize(value: string): string {
    if (!value) {
      return value;
    }
    
    return value.charAt(0).toUpperCase() + value.slice(1);
  }

  /**
   * Formats a string to title case
   */
  static toTitleCase(value: string): string {
    if (!value) {
      return value;
    }
    
    return value
      .toLowerCase()
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  }

  /**
   * Removes all HTML tags from a string
   */
  static stripHtml(value: string): string {
    if (!value) {
      return value;
    }
    
    return value.replace(/<[^>]*>/g, '');
  }

  /**
   * Converts a string to kebab-case
   */
  static toKebabCase(value: string): string {
    if (!value) {
      return value;
    }
    
    return value
      .replace(/([a-z])([A-Z])/g, '$1-$2')
      .replace(/\s+/g, '-')
      .toLowerCase();
  }

  /**
   * Generates a random string of the specified length
   */
  static randomString(length: number = 10): string {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';
    
    for (let i = 0; i < length; i++) {
      result += characters.charAt(Math.floor(Math.random() * characters.length));
    }
    
    return result;
  }
} 