/**
 * Base API response structure for all endpoints
 */
export interface ApiResponse {
  success: boolean;
  message: string;
  errors: ValidationErrorDetail[];
}

/**
 * API response with data
 */
export interface ApiResponseWithData<T> extends ApiResponse {
  data: T;
}

/**
 * Validation error detail
 */
export interface ValidationErrorDetail {
  propertyName: string;
  errorMessage: string;
  attemptedValue: any;
  errorCode: string;
} 