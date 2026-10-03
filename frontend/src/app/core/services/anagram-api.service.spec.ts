import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { AnagramApiService } from './anagram-api.service';

describe('AnagramApiService', () => {
  let service: AnagramApiService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(AnagramApiService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('requests matching anagrams for a word', () => {
    service.findMatchingAnagrams('dogs').subscribe((result) => {
      expect(result.anagrams).toEqual(['gods']);
    });

    const request = http.expectOne('/api/anagrams?word=dogs');
    expect(request.request.method).toBe('GET');
    request.flush({ input: 'dogs', anagrams: ['gods'], count: 1 });
  });

  it('requests anagram occurrences with the text and word', () => {
    service.findOccurrences('thegodsanddogsweredogged', 'dog').subscribe((result) => {
      expect(result.count).toBe(3);
    });

    const request = http.expectOne(
      (req) =>
        req.url === '/api/anagrams/occurrences' &&
        req.params.get('text') === 'thegodsanddogsweredogged' &&
        req.params.get('word') === 'dog',
    );
    request.flush({ text: 'thegodsanddogsweredogged', word: 'dog', occurrences: [], count: 3 });
  });
});
