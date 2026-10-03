import { Routes } from '@angular/router';

/**
 * One route per feature. Each page is lazy-loaded (loadComponent), so its code
 * is only downloaded when the user first visits it.
 */
export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'matching-anagrams' },
  {
    path: 'matching-anagrams',
    title: 'Matching anagrams · Anagram Finder',
    loadComponent: () =>
      import('./features/matching-anagrams/matching-anagrams.component').then((m) => m.MatchingAnagramsComponent),
  },
  {
    path: 'find-anagrams',
    title: 'Find the anagrams · Anagram Finder',
    loadComponent: () =>
      import('./features/anagram-occurrences/anagram-occurrences.component').then((m) => m.AnagramOccurrencesComponent),
  },
  // Unknown URLs fall back to the first feature instead of a blank page.
  { path: '**', redirectTo: 'matching-anagrams' },
];
