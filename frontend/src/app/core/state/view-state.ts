import { HttpErrorResponse } from '@angular/common/http';
import { Observable, OperatorFunction, catchError, map, of, startWith } from 'rxjs';

/** Everything a template needs to render a request: loading, data, or an error. */
export type ViewState<T> =
  | { status: 'loading' }
  | { status: 'success'; data: T }
  | { status: 'error'; message: string };

/**
 * Turns an HTTP observable into a stream of ViewStates:
 * loading -> success | error. Lets templates bind with the async pipe
 * instead of components subscribing and tracking flags by hand.
 */
export function toViewState<T>(): OperatorFunction<T, ViewState<T>> {
  return (source: Observable<T>) =>
    source.pipe(
      map((data): ViewState<T> => ({ status: 'success', data })),
      startWith<ViewState<T>>({ status: 'loading' }),
      catchError((error: unknown) => of<ViewState<T>>({ status: 'error', message: describeError(error) })),
    );
}

function describeError(error: unknown): string {
  if (error instanceof HttpErrorResponse) {
    if (error.status === 0) {
      return 'Could not reach the API. Is it running?';
    }

    const validationErrors = error.error?.errors as Record<string, string[]> | undefined;
    if (validationErrors) {
      return Object.values(validationErrors).flat().join(' ');
    }
  }

  return 'Something went wrong. Please try again.';
}
