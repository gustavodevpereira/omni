/**
 * Represents the result of an operation
 */
export class Result<T> {
  public readonly isSuccess: boolean;
  public readonly isFailure: boolean;
  public readonly error: string;
  private readonly _value: T;

  private constructor(isSuccess: boolean, error?: string, value?: T) {
    this.isSuccess = isSuccess;
    this.isFailure = !isSuccess;
    this.error = error || '';
    this._value = value as T;
  }

  /**
   * Gets the value if the result is successful
   * @throws Error if the result is a failure
   */
  public getValue(): T {
    if (!this.isSuccess) {
      throw new Error('Cannot get the value of a failure result');
    }

    return this._value;
  }

  /**
   * Creates a success result
   */
  public static ok<U>(value?: U): Result<U> {
    return new Result<U>(true, undefined, value);
  }

  /**
   * Creates a failure result
   */
  public static fail<U>(error: string): Result<U> {
    return new Result<U>(false, error);
  }
} 