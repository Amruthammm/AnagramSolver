import { InjectionToken } from '@angular/core';

/**
 * Base URL of the .NET API. Defaults to '/api', which the dev server proxies
 * to the API (see proxy.conf.json), so no CORS or hard-coded hosts are needed.
 */
export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL', {
  providedIn: 'root',
  factory: () => '/api',
});
