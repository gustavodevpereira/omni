import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Custom form validators for use throughout the application
 */
export class CustomValidators {
  /**
   * Validates that passwords match
   */
  static passwordsMatch(controlName: string, matchingControlName: string): ValidatorFn {
    return (formGroup: AbstractControl): ValidationErrors | null => {
      const control = formGroup.get(controlName);
      const matchingControl = formGroup.get(matchingControlName);

      if (!control || !matchingControl) {
        return null;
      }

      if (matchingControl.errors && !matchingControl.errors['passwordMismatch']) {
        return null;
      }

      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ passwordMismatch: true });
        return { passwordMismatch: true };
      } else {
        matchingControl.setErrors(null);
        return null;
      }
    };
  }

  /**
   * Validates that the input is a valid email format
   */
  static email(control: AbstractControl): ValidationErrors | null {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    const valid = emailRegex.test(control.value);
    return valid ? null : { email: true };
  }

  /**
   * Validates that the input is a valid phone number
   */
  static phoneNumber(control: AbstractControl): ValidationErrors | null {
    // Basic phone number validation (10 digits, can have formatting)
    const phoneRegex = /^(\+\d{1,3})?[-.\s]?\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$/;
    const valid = phoneRegex.test(control.value);
    return valid ? null : { phoneNumber: true };
  }

  /**
   * Validates that the value is at least a minimum number
   */
  static min(min: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value || isNaN(parseFloat(control.value))) {
        return null;
      }
      
      const value = parseFloat(control.value);
      return value < min ? { min: { min, actual: value } } : null;
    };
  }

  /**
   * Validates that the value does not exceed a maximum number
   */
  static max(max: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value || isNaN(parseFloat(control.value))) {
        return null;
      }
      
      const value = parseFloat(control.value);
      return value > max ? { max: { max, actual: value } } : null;
    };
  }

  /**
   * Validates that the input is not blank (has non-whitespace characters)
   */
  static notBlank(control: AbstractControl): ValidationErrors | null {
    const isBlank = !control.value || control.value.toString().trim().length === 0;
    return isBlank ? { notBlank: true } : null;
  }
} 