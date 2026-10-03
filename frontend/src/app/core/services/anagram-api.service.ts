import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { AnagramOccurrencesResult, MatchingAnagramsResult } from '../models/anagram.models';
import { API_BASE_URL } from './api-base-url.token';

/** Thin, typed wrapper around the anagram endpoints. Components never touch HttpClient directly. */
@Injectable({ providedIn: 'root' })
export class AnagramApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${inject(API_BASE_URL)}/anagrams`;

  /** Feature 1: GET /api/anagrams?word=dogs */
  findMatchingAnagrams(word: string): Observable<MatchingAnagramsResult> {
    const params = new HttpParams().set('word', word);
    return this.http.get<MatchingAnagramsResult>(this.baseUrl, { params });
  }

  /** Feature 2: GET /api/anagrams/occurrences?text=...&word=... */
  findOccurrences(text: string, word: string): Observable<AnagramOccurrencesResult> {
    const params = new HttpParams().set('text', text).set('word', word);
    return this.http.get<AnagramOccurrencesResult>(`${this.baseUrl}/occurrences`, { params });
  }
}
