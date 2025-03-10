/**
 * Generic API Response Model
 * 
 * Provides a consistent structure for all API responses.
 * Enforces a standard format across the application.
 */
export interface ApiResponse<T> {
  /**
   * Success flag indicating if the request was successful
   */
  success: boolean;

  /**
   * Data payload returned from the API
   */
  data?: T;

  /**
   * Error message if the request failed
   */
  error?: string;

  /**
   * HTTP status code of the response
   */
  statusCode?: number;

  /**
   * Metadata for pagination or additional information
   */
  meta?: {
    /**
     * Total count of available records
     */
    totalCount?: number;
    
    /**
     * Current page number
     */
    page?: number;
    
    /**
     * Page size
     */
    pageSize?: number;
    
    /**
     * Total number of pages available
     */
    totalPages?: number;
  };
}

/**
 * Pagination parameters for API requests
 */
export interface PaginationParams {
  /**
   * Page number (1-based)
   */
  page: number;
  
  /**
   * Number of items per page
   */
  pageSize: number;
  
  /**
   * Field name to sort by
   */
  sortBy?: string;
  
  /**
   * Sort direction ('asc' or 'desc')
   */
  sortDirection?: 'asc' | 'desc';
}

/**
 * Error response from the API
 */
export interface ErrorResponse {
  /**
   * Error message
   */
  message: string;
  
  /**
   * Error code or identifier
   */
  code?: string;
  
  /**
   * Validation errors if applicable
   */
  errors?: { [key: string]: string[] };
} 