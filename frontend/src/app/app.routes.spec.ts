import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { Router, provideRouter } from '@angular/router';
import { RouterTestingHarness } from '@angular/router/testing';

import { routes } from './app.routes';
import { AnagramOccurrencesComponent } from './features/anagram-occurrences/anagram-occurrences.component';
import { MatchingAnagramsComponent } from './features/matching-anagrams/matching-anagrams.component';

describe('App routes', () => {
  let harness: RouterTestingHarness;

  beforeEach(async () => {
    TestBed.configureTestingModule({
      providers: [provideRouter(routes), provideHttpClient(), provideHttpClientTesting()],
    });
    harness = await RouterTestingHarness.create();
  });

  it('redirects the empty path to the matching anagrams page', async () => {
    const page = await harness.navigateByUrl('/', MatchingAnagramsComponent);

    expect(page).toBeInstanceOf(MatchingAnagramsComponent);
    expect(TestBed.inject(Router).url).toBe('/matching-anagrams');
  });

  it('shows the find anagrams page at /find-anagrams', async () => {
    const page = await harness.navigateByUrl('/find-anagrams', AnagramOccurrencesComponent);

    expect(page).toBeInstanceOf(AnagramOccurrencesComponent);
  });

  it('sends unknown URLs back to the first page', async () => {
    await harness.navigateByUrl('/does-not-exist');

    expect(TestBed.inject(Router).url).toBe('/matching-anagrams');
  });
});
